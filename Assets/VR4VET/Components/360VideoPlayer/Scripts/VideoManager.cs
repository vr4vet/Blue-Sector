/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager videoManager;
    [SerializeField] private SkyboxHolder SkyboxHolder;
    [SerializeField] private Task.Skill observantBadge;
    [SerializeField] private DeadfishDialog deadfishDialog;

    private LayerMask oldCulingMask;
    private Camera VRCamera;

    [HideInInspector] public VideoPlayer videoPlayer;
    [SerializeField] private MaintenanceManager mm;


    /// <summary>
    /// Unity start method
    /// </summary>
    private void Awake()
    {
        if (videoManager == null)
            videoManager = this;
        else if (videoManager != this)
            Destroy(gameObject);

        videoPlayer = GetComponent<VideoPlayer>();

        //Get the cameras original cullingMask
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        oldCulingMask = VRCamera.cullingMask;
    }


    /// <summary>
    /// Add the clip to the video manager, apply to the skybox and play the video
    /// </summary>
    /// <param name="clip"></param>
    public void ShowVideo(VideoClip clip, bool startDeadfishDialog)
    {
        //change the video clip
        videoPlayer.clip = clip;

        // play the video using the videoPlayer attatched to the platform
        videoPlayer.Play();

        StartCoroutine(applyVideo(startDeadfishDialog));

    }

    /// <summary>
    /// This method will wait untill the video clip is fully changed then apply it to the skybox
    /// </summary>
    /// <returns></returns>
    IEnumerator applyVideo(bool startDeadfishDialog)
    {
        while (!videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        // set skybox to video
        SkyboxHolder.applyVideoTextureToSkybox();

        // hide everything in sceene exept potantially the player and teleporting prefab (given not recomended setup)
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        VRCamera.cullingMask = 1 << LayerMask.NameToLayer("360Video");


        while (videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        StopVideo();
        mm.BadgeChanged.Invoke(observantBadge);
        if (startDeadfishDialog)
        {
            deadfishDialog.UpdateDialog();
        }
        // Task.Step videoStep = mm.GetStep("Håndforing", "Se 360 Video");
        // mm.CompleteStep(videoStep);

    }


    /// <summary>
    /// Set the default skybox again and stop the video
    /// </summary>
    public void StopVideo()
    {
        videoPlayer.Stop();
        // Show everything in the scene again
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        VRCamera.cullingMask = oldCulingMask;

        SkyboxHolder.applyDefaultSkybox();
    }


}
