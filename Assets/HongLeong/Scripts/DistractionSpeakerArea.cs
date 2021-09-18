using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// HL - Detect soldier within area ( collider size )
/// </summary>
public class DistractionSpeakerArea : BaseInteractableObject
{
    [HideInInspector]
    public Action<GameObject> addSoldierCallback;
    public Action<GameObject> removeSoldierCallback;

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<GuardAI>())
        {
            addSoldierCallback?.Invoke(other.gameObject);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GuardAI>())
        {
            removeSoldierCallback?.Invoke(other.gameObject);
        }
    }
}
