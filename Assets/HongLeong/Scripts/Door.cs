using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseInteractableObject
{
    [Header("Door Settings")]
    public PlayerType playerDetect = PlayerType.NONE;
    public GameObject doorCollider;

    private bool mIsDoorOpened = false;

    protected override void OnTriggerEnter(Collider other)
    {
        //get player
        if (playerDetect == PlayerType.PLAYER_1)
        {
            if (other.tag.Equals("Player1"))
            {
                //set player action click callback
            }
        }
        else if (playerDetect == PlayerType.PLAYER_2)
        {
            if (other.tag.Equals("Player2"))
            {
                //set player action click callback
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //get player
        if (playerDetect == PlayerType.PLAYER_1)
        {
            if (other.tag.Equals("Player1"))
            {
                //remove player action click callback
            }
        }
        else if (playerDetect == PlayerType.PLAYER_2)
        {
            if (other.tag.Equals("Player2"))
            {
                //remove player action click callback
            }
        }
    }

    protected override void ActionCall()
    {
        mIsDoorOpened = true;

        //disable collider
        doorCollider.GetComponent<Collider>().enabled = false;
        doorCollider.SetActive(false);
    }
}
