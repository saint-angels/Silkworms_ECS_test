using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationConfig", menuName = "Config/AnimationConfig")]
public class AnimationConfig : ScriptableObject
{
    [Header("Body view")]
    public float bodyStanceChangeDuration = .3f;
    public Ease bodyStanceChangeEase = Ease.OutCirc;

    public float bodyDamagedFlashDuration = .3f;

    public float bodyAttackWarmupMinScaleY = .5f;
    public float bodyAttackWarmupCycleDuration = .5f;

    [Header("UI")]
    public float fenceLabelHideOffsetY = 400f;
    public float fenceLabelHideDuration = 1f;
    public Ease fenceLabelHideEase = Ease.InQuart;

    public float blockComboLabelShakeDuration = 1f;
    public float blockComboLabelShakeStrength = 1f;
    public int blockComboLabelShakeVibrato = 1;
    public float blockComboLabelShakeRandomness = 1f;

    public float buttonsPromptFadeDuration = 1f;
}
