using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HL - Trap hurt player according to period of time
/// </summary>
public class TrapSpike : BaseInteractableObject
{
    [Header("Trap Settings")]
    public float secondTriggerTrap = 3f;
    public float secondOnTrap = 3f;

    private float mSecondCount = 0f;
    private float mSecondOnTrapCount = 0f;
    private bool mIsTrapEnabled = false;

    // Update is called once per frame
    private void Update()
    {
        if (!mIsTrapEnabled)
        {
            mSecondCount += Time.deltaTime;

            if (mSecondCount >= secondTriggerTrap)
            {
                mIsTrapEnabled = true;
                mSecondCount = 0f;
            }
        }
        else
        {
            mSecondOnTrapCount += Time.deltaTime;

            if(mSecondOnTrapCount >= secondOnTrap)
            {
                mIsTrapEnabled = false;
                mSecondOnTrapCount = 0f;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!mIsTrapEnabled) return;
        if (other.tag.Equals("player1") || other.tag.Equals("player2"))
        {
            //hit player
        }
    }
}