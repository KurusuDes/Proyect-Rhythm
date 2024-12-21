using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RP.Core;

public class UITempoController : MonoBehaviour
{
    [FoldoutGroup("Settings"), SerializeField] private UISlider _leftSlider;
    [FoldoutGroup("Settings"), SerializeField] private UISlider _rightSlider;

    void Start()
    {
        GameManager.Instance.TempoController.Tick += StartSliders;
    }
    public void StartSliders()
    {
        SetPrecciseTool(_leftSlider);
        SetPrecciseTool(_rightSlider);
    }
    public void SetPrecciseTool(UISlider slider)
    {
        slider.value = 0;
        DOTween.To(() => slider.value, x => slider.value = x, 1, GameManager.Instance.TempoController.GetIterval())
               .SetEase(Ease.InCubic);
    }
}
