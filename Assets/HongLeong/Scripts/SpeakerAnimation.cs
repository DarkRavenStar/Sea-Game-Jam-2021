using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeakerAnimation : MonoBehaviour
{
    private Action mAnimEndCb;

    public void SetAnimationEndCb(Action _animEndCb)
    {
        mAnimEndCb = _animEndCb;
    }

    public void AnimaEndCallback()
    {
        mAnimEndCb?.Invoke();
    }
}
