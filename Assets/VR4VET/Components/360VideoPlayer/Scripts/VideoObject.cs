/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using UnityEngine;
using UnityEngine.Video;
//using UnityEngine.XR.Interaction.Toolkit;
using BNG;
using System.Collections;

public class VideoObject : MonoBehaviour
{
    public VideoClip videoClip;
    public bool triggerSpecialEventOnVideoEnd;
    public bool hideVideoPlayerAfterFirstWatch;

    [Header("Video is a task")]
    public VideoIsTask videoIsTask;
    public bool videoIsTaskBool;
    


    private VideoPlayer videoPlayer;
    private int rotataionSpeed = 50;
    private TextMesh hintText;
    private Quaternion OriginalRotation;
    private Vector3 OriginalPosition;
    private bool VideoIsPlayedOnce;
    //private XRGrabInteractable XRGI; 
    private Grabbable BNGG;
    private VideoObject[] Headsets;

    /// <summary>
    /// Unity start method
    /// </summary>
    private void Start()
    {
        hintText = GetComponentInChildren<TextMesh>();
        videoPlayer = VideoManager.videoManager.videoPlayer;
        hintText.transform.SetParent(null);
        OriginalRotation = transform.rotation;
        OriginalPosition = transform.position;
      //  XRGI = GetComponent<XRGrabInteractable>();
        BNGG = GetComponent<Grabbable>();
        Headsets = FindObjectsOfType<VideoObject>();
    }

    /// <summary>
    /// Unity update method
    /// </summary>
    void LateUpdate()
    {
        //Text position
        hintText.gameObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        hintText.gameObject.transform.position = transform.position + new Vector3(0, 0.2f ,0);


        if (BNGG.BeingHeld)
        {

            if (!videoPlayer.isPlaying)
            {
                if (!VideoIsPlayedOnce)
                {
                    PlayVideo();
                    VideoIsPlayedOnce = true;
                }
            }

        }
        else if(VideoIsPlayedOnce)
        {
            StopVideo();
            VideoIsPlayedOnce = false;
        }else
        {
            HeadsetMovement();
        }
    }


    /// <summary>
    /// Stop the video and make the object start rotating
    /// </summary>
    void StopVideo()
    {   
        foreach (VideoObject headset in Headsets)
        {
            headset.gameObject.SetActive(true);
            headset.hintText.gameObject.SetActive(true);
        }

        //Video controll
        VideoManager.videoManager.StopVideo();

        transform.rotation = OriginalRotation; //reset the rotation to start rotation
        hintText.text = "Grab To Play";

        if (hideVideoPlayerAfterFirstWatch && VideoIsPlayedOnce)
        {
            HideVideoPlayer();
        }

    }

    void HeadsetMovement()
    {
        //rotation
        transform.Rotate(0.0f, 0.0f, 1.0f * rotataionSpeed * Time.deltaTime, Space.Self);

        //position
        float y = Mathf.PingPong(0.3f * Time.time, 1) * 0.1f + 1f;
        transform.position = new Vector3(OriginalPosition.x, OriginalPosition.y, OriginalPosition.z);

        //scale
        transform.localScale = new Vector3(1f, 1f, 1f);

    }


    /// <summary>
    /// Play the video and stop objects rotating
    /// </summary>
    void PlayVideo()
    {
        foreach (VideoObject headset in Headsets)
            if (headset != this)
            {
                headset.gameObject.SetActive(false);
                headset.hintText.gameObject.SetActive(false);
            }

        VideoManager.videoManager.ShowVideo(videoClip, triggerSpecialEventOnVideoEnd);
        hintText.text = "Release To Stop";

        if(videoIsTaskBool)
        {
            videoIsTask.activateGameObjects();
            videoIsTask.invokeEventOnVideoPlay();
        }

        //scale
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

    }

    public void HideVideoPlayer()
    {
        hintText.text = null;
        gameObject.SetActive(false);
    }

    public void ShowVideoPlayer()
    {
        hintText.text = hintText.text = "Grab To Play"; ;
        gameObject.SetActive(true);
    }



}
