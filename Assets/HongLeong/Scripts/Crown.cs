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
        if (other.tag.Equals("Player1"))
        {
            //set player action click callback
        }

        if (other.tag.Equals("Player2"))
        {
            //set player action click callback
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player1"))
        {
            //set player action click callback
        }

        if (other.tag.Equals("Player2"))
        {
            //set player action click callback
        }
    }

    protected override void ActionCall()
    {
        if (mIsSteal) return;

        mIsSteal = true;
        crownHat.SetActive(false);
    }
}
