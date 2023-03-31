using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BNGPlayerLocator
{
    private BNGPlayerController playerController;

    public BNGPlayerController CurrentPlayerController
    {
        get
        {
            if (playerController == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                playerController = player != null
                    ? player.GetComponentInChildren<BNGPlayerController>()
                    : Object.FindObjectOfType<BNGPlayerController>();
            }

            return playerController;
        }
    }
}
