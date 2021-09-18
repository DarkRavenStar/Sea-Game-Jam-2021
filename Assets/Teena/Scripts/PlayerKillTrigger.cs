using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillTrigger : BaseInteractableObject
{
    public System.Action<GameObject> OnDeath = delegate { };

    public System.Action<GameObject> OnRevive = delegate { };

    public System.Action OnInteract = delegate { };
    
    protected override void OnTriggerEnter(Collider other)
    {
        BasePlayer ply = other.gameObject.GetComponent<BasePlayer>();
        if (ply != null)
        {
            Debug.Log("TeenaTest.GuardAI.KillCollidedPlayer");
            ply.Damage();
        }
        else
        {
            Debug.Log("TeenaTest.GuardAI.KillCollidedPlayer.FAKE");
        }
    }
}
