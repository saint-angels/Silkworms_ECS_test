using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public AIConfig AI => aiConfig;
    public AnimationConfig Animation => animationConfig;
    public CameraConfig CameraConfig => cameraConfig;
    public EatersConfig EatersConfig => eatersConfig;
    public FoodConfig FoodConfig => foodConfig;
    public EntitiesAssets EntitiesAssets => entitiesAssets;

    [SerializeField] private AIConfig aiConfig = null;
    [SerializeField] private AnimationConfig animationConfig = null;
    [SerializeField] private CameraConfig cameraConfig = null;
    [SerializeField] private EatersConfig eatersConfig = null;
    [SerializeField] private FoodConfig foodConfig = null;
    [SerializeField] private EntitiesAssets entitiesAssets = null;
    
}
