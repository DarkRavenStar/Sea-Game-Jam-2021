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

public abstract class BaseInteractableObject : MonoBehaviour
{
    public ColliderType colliderType = ColliderType.NONE;
    public Vector3 squareSize;
    public float radiusSize;
    protected Action mActionCallback;
    protected BoxCollider boxCollider;
    protected SphereCollider SphereCollider;

    protected virtual void Start()
    {

        if (colliderType == ColliderType.BOX)
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = squareSize;
        }
        else if (colliderType == ColliderType.CIRCLE)
        {
            SphereCollider = GetComponent<SphereCollider>();
            SphereCollider.radius = radiusSize;
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
