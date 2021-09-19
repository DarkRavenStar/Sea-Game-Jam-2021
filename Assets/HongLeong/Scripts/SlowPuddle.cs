using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPuddle : BaseInteractableObject
{
    protected override void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            //add slow player
            playerMovement.ChangePlayerSpeed(true);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            //remove slow player
            playerMovement.ChangePlayerSpeed();
        }
    }
}
