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
    public Animation ownAnim;

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
                PlayAnimation(true);
            }
        }
        else
        {
            mSecondOnTrapCount += Time.deltaTime;

            if (mSecondOnTrapCount >= secondOnTrap)
            {
                mIsTrapEnabled = false;
                mSecondOnTrapCount = 0f;
                PlayAnimation(false);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!mIsTrapEnabled) return;

        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            //hit player
            player.Damage();
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!mIsTrapEnabled) return;

        BasePlayer player = other.GetComponent<BasePlayer>();
        if (player != null)
        {
            //hit player
            player.Damage();
        }
    }

    private void PlayAnimation(bool _isUp)
    {
        if (_isUp)
        {
            Debug.LogWarning("haha up");
            ownAnim.Play("trap-spike-up");
        }
        else
        {
            Debug.LogWarning("haha down");
            ownAnim.Play("trap-spike-down");
        }
    }
}
