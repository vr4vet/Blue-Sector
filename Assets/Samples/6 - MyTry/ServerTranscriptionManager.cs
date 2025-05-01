using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Whisper.Utils;

namespace VR4VET.Transcription
{
    /// <summary>
    /// Manages transcription of audio through the server API
    /// </summary>
    public class ServerTranscriptionManager : MonoBehaviour
    {
        [Header("Server Configuration")]
        [Tooltip("The URL of the transcription API endpoint")]
        [SerializeField] private string transcriptionApiUrl = "http://localhost:8000/transcribe";

        [Tooltip("Language code for transcription (leave empty for auto-detection)")]
        [SerializeField] private string languageCode = "";

        // Events
        public delegate void TranscriptionCompleteHandler(string text, string language = null, ServerTranscriptionInfo info = null);
        public event TranscriptionCompleteHandler OnTranscriptionComplete;

        public delegate void TranscriptionProgressHandler(int progressPercent);
        public event TranscriptionProgressHandler OnProgress;

        public delegate void TranscriptionErrorHandler(string errorMessage);
        public event TranscriptionErrorHandler OnTranscriptionError;

        // Status properties
        public bool IsProcessing { get; private set; }

        /// <summary>
        /// Class to hold detailed server transcription information
        /// </summary>
        public class ServerTranscriptionInfo
        {
            public bool IsServerProcessed { get; set; }
            public float ProcessingTimeSeconds { get; set; }
            public string Processor { get; set; }

            public override string ToString()
            {
                return $"Processed by: {Processor}\nTime: {ProcessingTimeSeconds:F3}s";
            }
        }

        /// <summary>
        /// Send audio data to the server for transcription
        /// </summary>
        /// <param name="audioData">Raw audio data</param>
        /// <param name="frequency">Audio sample rate</param>
        /// <param name="channels">Number of audio channels</param>
        /// <returns>A task that completes when transcription is done</returns>
        public async Task<(string text, ServerTranscriptionInfo info)> TranscribeAudioAsync(float[] audioData, int frequency, int channels)
        {
            if (audioData == null || audioData.Length == 0)
            {
                OnTranscriptionError?.Invoke("No audio data provided");
                return (null, null);
            }

            IsProcessing = true;
            OnProgress?.Invoke(0);

            try
            {
                // Convert to WAV and send to server
                byte[] wavData = ConvertToWav(audioData, frequency, channels);
                var result = await SendAudioToServerAsync(wavData);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Transcription error: {e.Message}");
                OnTranscriptionError?.Invoke($"Failed to transcribe: {e.Message}");
                return (null, null);
            }
            finally
            {
                IsProcessing = false;
                OnProgress?.Invoke(100);
            }
        }

        /// <summary>
        /// Process audio from MicrophoneRecord
        /// </summary>
        public async void ProcessAudioChunk(AudioChunk audioChunk)
        {
            if (audioChunk.Data == null || audioChunk.Data.Length == 0)
            {
                OnTranscriptionError?.Invoke("No audio data provided");
                return;
            }

            var (text, info) = await TranscribeAudioAsync(audioChunk.Data, audioChunk.Frequency, audioChunk.Channels);
            if (!string.IsNullOrEmpty(text))
            {
                OnTranscriptionComplete?.Invoke(text, null, info);
            }
        }

        /// <summary>
        /// Convert float audio data to WAV format
        /// </summary>
        private byte[] ConvertToWav(float[] audioData, int frequency, int channels)
        {
            try
            {
                // Convert to 16-bit PCM
                Int16[] intData = new Int16[audioData.Length];
                for (int i = 0; i < audioData.Length; i++)
                {
                    intData[i] = (Int16)(audioData[i] * 32767);
                }

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream))
                    {
                        // WAV header
                        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
                        writer.Write(36 + intData.Length * 2); // File size
                        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
                        writer.Write(new char[] { 'f', 'm', 't', ' ' });
                        writer.Write(16); // Chunk size
                        writer.Write((ushort)1); // Audio format (1 = PCM)
                        writer.Write((ushort)channels); // Channels
                        writer.Write(frequency); // Sample rate
                        writer.Write(frequency * channels * 2); // Byte rate
                        writer.Write((ushort)(channels * 2)); // Block align
                        writer.Write((ushort)16); // Bits per sample
                        writer.Write(new char[] { 'd', 'a', 't', 'a' });
                        writer.Write(intData.Length * 2); // Data size

                        // Convert Int16 to bytes and write to stream
                        byte[] byteData = new byte[intData.Length * 2];
                        Buffer.BlockCopy(intData, 0, byteData, 0, byteData.Length);
                        writer.Write(byteData);
                    }

                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error converting audio to WAV: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Send audio data to the transcription server
        /// </summary>
        private async Task<(string text, ServerTranscriptionInfo info)> SendAudioToServerAsync(byte[] wavData)
        {
            // Create a form with the audio data
            WWWForm form = new WWWForm();
            form.AddBinaryData("audio", wavData, "recording.wav", "audio/wav");

            // Add language if specified
            if (!string.IsNullOrEmpty(languageCode))
            {
                form.AddField("language", languageCode);
            }

            // Use coroutine-based approach with TaskCompletionSource
            TaskCompletionSource<(string, ServerTranscriptionInfo)> tcs = new TaskCompletionSource<(string, ServerTranscriptionInfo)>();

            // Start coroutine to handle the web request
            StartCoroutine(SendWebRequestCoroutine(transcriptionApiUrl, form, tcs));

            // Return the task that will complete when the coroutine finishes
            return await tcs.Task;
        }

        private IEnumerator SendWebRequestCoroutine(string url, WWWForm form, TaskCompletionSource<(string, ServerTranscriptionInfo)> tcs)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                // Log the request being sent
                Debug.Log($"Sending audio to server at {url}");

                // Send the request and wait for completion
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Server error: {request.error}");
                    tcs.SetException(new Exception($"Server error: {request.error}"));
                    yield break;
                }

                try
                {
                    // Parse the response
                    string responseText = request.downloadHandler.text;
                    Debug.Log($"Server response received: {responseText}");

                    TranscriptionResponse response = JsonUtility.FromJson<TranscriptionResponse>(responseText);

                    if (!response.success)
                    {
                        tcs.SetException(new Exception(response.error ?? "Unknown server error"));
                        yield break;
                    }

                    // Create server info object from response
                    var serverInfo = new ServerTranscriptionInfo
                    {
                        IsServerProcessed = response.server_processed,
                        ProcessingTimeSeconds = response.processing_time_seconds,
                        Processor = response.processor ?? "Unknown"
                    };

                    // Complete the task with the transcription result and server info
                    tcs.SetResult((response.transcription, serverInfo));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
        }

        /// <summary>
        /// Set the language for transcription
        /// </summary>
        public void SetLanguage(string language)
        {
            languageCode = language;
        }

        /// <summary>
        /// Structured response from the server
        /// </summary>
        [Serializable]
        private class TranscriptionResponse
        {
            public bool success;
            public string transcription;
            public string error;
            public bool server_processed;
            public float processing_time_seconds;
            public string processor;
        }
    }
}