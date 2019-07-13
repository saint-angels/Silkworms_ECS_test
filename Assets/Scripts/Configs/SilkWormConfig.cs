using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SilkWormConfig", menuName = "Config/SilkWormConfig")]
public class SilkWormConfig : ScriptableObject
{

    [Header("Hunger")]
    public float startFullness = 20f;
    public float hungerSpeed = 1f;
    

}
