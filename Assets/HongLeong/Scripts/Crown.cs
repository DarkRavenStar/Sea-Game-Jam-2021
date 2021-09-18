using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HL - Crown can be steal by player
/// </summary>
public class Crown : BaseInteractableObject
{
    [Header("Crown Settings")]
    public GameObject crownHat;

    private bool mIsSteal = false;

    protected override void OnTriggerEnter(Collider other)
    {
        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            //set player action click callback
            player.OnInteract += ActionCall;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            //remove player action click callback
            player.OnInteract -= ActionCall;
        }
    }

    protected override void ActionCall()
    {
        if (mIsSteal) return;

        mIsSteal = true;
        crownHat.SetActive(false);
    }
}
