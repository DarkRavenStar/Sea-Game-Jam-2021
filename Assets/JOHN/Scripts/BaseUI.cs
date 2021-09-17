using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.DemiLib;
using DG.Tweening;

public class BaseUI : MonoBehaviour
{



    protected void PlayAnimation(CanvasGroup canvas, float endValue = 1, float duration = 0.4f, System.Action action = null)
    {
        canvas.DOFade(endValue, duration).OnComplete(() =>
        {
            action.Invoke();
        });
    }
}
