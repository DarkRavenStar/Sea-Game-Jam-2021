using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HL - Interactable Door open by specific senario
/// </summary>
public class Door : BaseInteractableObject
{
    [Header("Door Settings")]
    public PlayerAbility playerDetect = PlayerAbility.NONE;
    public GameObject doorCollider;

    private bool mIsDoorOpened = false;

    protected override void OnTriggerEnter(Collider other)
    {
        //get player
        if (playerDetect == PlayerAbility.INFUSE)
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null && player.playerAbility == PlayerAbility.INFUSE)
            {
                //set player action click callback
                player.OnInteract += ActionCall;
            }
        }
        else if (playerDetect == PlayerAbility.DISPEL)
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null && player.playerAbility == PlayerAbility.DISPEL)
            {
                //set player action click callback
                player.OnInteract += ActionCall;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //get player
        if (playerDetect == PlayerAbility.INFUSE)
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null && player.playerAbility == PlayerAbility.INFUSE)
            {
                //remove player action click callback
                player.OnInteract -= ActionCall;
            }
        }
        else if (playerDetect == PlayerAbility.DISPEL)
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player != null && player.playerAbility == PlayerAbility.DISPEL)
            {
                //remove player action click callback
                player.OnInteract -= ActionCall;
            }
        }
    }

    protected override void ActionCall()
    {
        if (mIsDoorOpened) return;

        mIsDoorOpened = true;

        //disable collider
        doorCollider.GetComponent<Collider>().enabled = false;
        doorCollider.SetActive(false);
    }
}
