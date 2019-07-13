using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public AIConfig AI => aiConfig;
    public AnimationConfig Animation => animationConfig;
    public CameraConfig CameraConfig => cameraConfig;
    public SilkWormConfig SilkWormConfig => silkWormConfig;

    [SerializeField] private AIConfig aiConfig = null;
    [SerializeField] private AnimationConfig animationConfig = null;
    [SerializeField] private CameraConfig cameraConfig = null;
    [SerializeField] private SilkWormConfig silkWormConfig = null;

}
