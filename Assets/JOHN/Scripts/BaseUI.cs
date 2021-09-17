using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.DemiLib;
using DG.Tweening;

public class BaseUI : MonoBehaviour
{
    protected void PlayAnimation(CanvasGroup canvas, float endValue = 1, float duration = 0.4f, System.Action action = null, bool show = true)
    {
        canvas.DOFade(endValue, duration).OnStart(() => { canvas.interactable = show?true:false; canvas.blocksRaycasts = show ? true : false; }).OnComplete(() =>
        {
            action.Invoke();
        });
    }

    public void _PlayAnimation(CanvasGroup canvas, float endValue = 1, float duration = 0.4f, System.Action action = null, bool show = true)
    {
        PlayAnimation(canvas, endValue, duration, action, show);
    }
}
