using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ColliderType
{
    NONE,
    BOX,
    CIRCLE,
}

/// <summary>
/// HL - Base class for interactable / non-interactable object
/// </summary>
public abstract class BaseInteractableObject : MonoBehaviour
{
    public ColliderType colliderType = ColliderType.NONE;
    public Vector3 squareSize;
    public float radiusSize;
    protected Action mActionCallback;
    protected BoxCollider mBoxCollider;
    protected SphereCollider mSphereCollider;

    protected virtual void Start()
    {

        if (colliderType == ColliderType.BOX)
        {
            mBoxCollider = GetComponent<BoxCollider>();
            mBoxCollider.size = squareSize;
        }
        else if (colliderType == ColliderType.CIRCLE)
        {
            mSphereCollider = GetComponent<SphereCollider>();
            mSphereCollider.radius = radiusSize;
        }
    }

    protected virtual void ActionCall() { }

    protected virtual void OnTriggerEnter(Collider other)
    {

    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }
}
