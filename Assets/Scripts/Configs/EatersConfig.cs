using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EatersConfig", menuName = "Config/EatersConfig")]
public class EatersConfig : ScriptableObject
{
    public float maxFullness;
    public float hungerSpeed;
    
    public float eatSpeed;
}
