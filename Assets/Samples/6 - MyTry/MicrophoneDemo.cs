using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using VR4VET.Transcription;
using Whisper.Utils;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace Whisper.Samples
{
    /// <summary>
    /// Record audio clip from microphone and make a transcription.
    /// </summary>
    public class MicrophoneDemo : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public bool streamSegments = true;
        public bool printLanguage = true;

        [Header("Server Transcription")]
        [Tooltip("The ServerTranscriptionManager component for server-side transcription")]
        public ServerTranscriptionManager serverTranscription;
        [Tooltip("Whether to use server-side transcription instead of local Whisper")]
        public bool useServerTranscription = false;

        [Tooltip("Enable hold-to-record functionality")]
        public bool holdToRecord = false;

        [Header("UI")]
        public Button button;
        public Text buttonText;
        public Text outputText;
        public Text timeText;
        public Dropdown languageDropdown;
        public Toggle translateToggle;
        public Toggle vadToggle;
        public Toggle serverToggle;
        public ScrollRect scroll;

        private string _buffer;
        private Stopwatch serverTimer = new Stopwatch();

        private void Awake()
        {
            whisper.OnNewSegment += OnNewSegment;
            whisper.OnProgress += OnProgressHandler;

            microphoneRecord.OnRecordStop += OnRecordStop;

            button.onClick.AddListener(OnButtonPressed);
            languageDropdown.value = languageDropdown.options
                .FindIndex(op => op.text == whisper.language);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            translateToggle.isOn = whisper.translateToEnglish;
            translateToggle.onValueChanged.AddListener(OnTranslateChanged);

            vadToggle.isOn = microphoneRecord.vadStop;
            vadToggle.onValueChanged.AddListener(OnVadChanged);

            // Initialize server transcription if available
            if (serverToggle != null && serverTranscription != null)
            {
                serverToggle.isOn = useServerTranscription;
                serverToggle.onValueChanged.AddListener(OnServerToggleChanged);

                // Set up server transcription events
                serverTranscription.OnTranscriptionComplete += OnServerTranscriptionComplete;
                serverTranscription.OnProgress += OnServerProgressHandler;
                serverTranscription.OnTranscriptionError += OnServerTranscriptionError;
            }
            else if (serverToggle != null)
            {
                // Disable server toggle if there's no server transcription manager
                serverToggle.interactable = false;
            }
        }

        private void Update()
        {
            // Handle hold-to-record functionality
            if (holdToRecord)
            {
                if (Input.GetKeyDown(KeyCode.E) && !microphoneRecord.IsRecording)
                {
                    StartRecording();
                }
                else if (Input.GetKeyUp(KeyCode.E) && microphoneRecord.IsRecording)
                {
                    StopRecording();
                }
            }
        }

        private void OnVadChanged(bool vadStop)
        {
            microphoneRecord.vadStop = vadStop;
        }

        private void OnServerToggleChanged(bool useServer)
        {
            useServerTranscription = useServer;

            // Update UI based on transcription mode
            translateToggle.interactable = !useServerTranscription;
            if (useServerTranscription && serverTranscription != null)
            {
                // Set the server language to match local
                serverTranscription.SetLanguage(whisper.language);
            }
        }

        private void OnButtonPressed()
        {
            if (!microphoneRecord.IsRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        private void StartRecording()
        {
            microphoneRecord.StartRecord();
            buttonText.text = "Stop";
        }

        private void StopRecording()
        {
            microphoneRecord.StopRecord();
            buttonText.text = "Record";
        }

        private async void OnRecordStop(AudioChunk recordedAudio)
        {
            buttonText.text = "Record";
            _buffer = "";

            // Start processing timer
            var sw = new Stopwatch();
            sw.Start();

            if (useServerTranscription && serverTranscription != null)
            {
                // Process with server transcription
                timeText.text = "Processing with server...";
                serverTimer.Reset();
                serverTimer.Start(); // Start server transcription timer
                serverTranscription.ProcessAudioChunk(recordedAudio);
                // The result will be handled by OnServerTranscriptionComplete
            }
            else
            {
                // Process with local Whisper
                var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
                if (res == null || !outputText)
                    return;

                var time = sw.ElapsedMilliseconds;
                var rate = recordedAudio.Length / (time * 0.001f);
                timeText.text = $"Time: {time} ms\nRate: {rate:F1}x\nProcessor: Local Whisper";

                var text = res.Result;
                if (printLanguage)
                    text += $"\n\nLanguage: {res.Language}";

                outputText.text = text;
                UiUtils.ScrollDown(scroll);
            }
        }

        private void OnServerTranscriptionComplete(string text, string language = null, ServerTranscriptionManager.ServerTranscriptionInfo info = null)
        {
            if (!outputText)
                return;

            serverTimer.Stop(); // Stop server transcription timer
            float elapsedSeconds = serverTimer.ElapsedMilliseconds / 1000f;

            string result = text;

            // Display server info if available
            if (info != null)
            {
                timeText.text = $"Server Transcription Info:\n{info}\nTime: {elapsedSeconds:F2}s";
                UnityEngine.Debug.Log($"Server transcription completed: {info.Processor}, time: {info.ProcessingTimeSeconds}s");
            }
            else
            {
                timeText.text = $"Server transcription complete\nTime: {elapsedSeconds:F2}s";
            }

            if (printLanguage && !string.IsNullOrEmpty(language))
                result += $"\n\nLanguage: {language}";

            outputText.text = result;
            UiUtils.ScrollDown(scroll);
        }

        private void OnServerProgressHandler(int progress)
        {
            if (!timeText)
                return;

            timeText.text = $"Server progress: {progress}%";
        }

        private void OnServerTranscriptionError(string errorMessage)
        {
            if (!outputText)
                return;

            serverTimer.Stop(); // Stop server transcription timer
            float elapsedSeconds = serverTimer.ElapsedMilliseconds / 1000f;

            outputText.text = $"Error: {errorMessage}";
            timeText.text = $"Server transcription failed\nTime: {elapsedSeconds:F2}s";
            UiUtils.ScrollDown(scroll);
        }

        private void OnLanguageChanged(int ind)
        {
            var opt = languageDropdown.options[ind];
            whisper.language = opt.text;

            // Also update server language if server transcription is available
            if (serverTranscription != null)
            {
                serverTranscription.SetLanguage(opt.text);
            }
        }

        private void OnTranslateChanged(bool translate)
        {
            whisper.translateToEnglish = translate;
        }

        private void OnProgressHandler(int progress)
        {
            if (!timeText)
                return;
            timeText.text = $"Progress: {progress}%";
        }

        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || !outputText)
                return;

            _buffer += segment.Text;
            outputText.text = _buffer + "...";
            UiUtils.ScrollDown(scroll);
        }
    }
}