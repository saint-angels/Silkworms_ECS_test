using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Config/AIConfig")]
public class AIConfig : ScriptableObject
{
    public int maxDifficulty = 10;

    [Header("Choosing position")]
    public float stanceChooseDurationDefault = 1.5f;
    public float stanceChooseDurationMin = .1f;

    public float stanceChooseDelayDefault = 1f;
    public float stanceChooseDelayMin = .1f;

    public float stancePostChoiceDelayDefault = .5f;
    public float stancePostChoiceDelayMin = .1f;

    [Header("Attacking")]
    public float attackWarmupDurationDefault = 1f;
    public float attackWarmupDurationMin = 1f;


    public float attackStanceHoldDuration = 1f;


    public float postAttackDelayDefault = 2f;
    public float postAttackDelayMin = .1f;
}
