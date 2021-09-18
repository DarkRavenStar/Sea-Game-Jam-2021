using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HL - Moving Trap
/// </summary>
public class TrapRoll : BaseInteractableObject
{
    [Header("Trap Settings")]
    public Transform[] points;
    public float speed;

    private bool mIsVerified = false;
    private Vector3 mTargetPosition;
    private int pointIndex = 0;

    private void Awake()
    {
        if (points.Length < 2)
        {
            Debug.LogError("[TrapRoll] Don't have enough point to move to.");
        }
        else
        {
            mIsVerified = true;
            transform.position = points[0].position;
            mTargetPosition = points[1].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mIsVerified) return;

        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, mTargetPosition, step);

        if (Vector3.Distance(transform.position, mTargetPosition) < 0.001f)
        {
            pointIndex++;
            if (pointIndex >= points.Length)
            {
                pointIndex = 0;
            }
            mTargetPosition = points[pointIndex].position;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        BasePlayer player = other.GetComponent<BasePlayer>();

        if (player != null)
        {
            //hit player
            player.Damage();
        }
    }
}
