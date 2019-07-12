using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;

    private AnimationConfig animationCfg;

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;

        
    }

    private void LateUpdate()
    {
        //TODO: Process abrsact hud-objects with abstact anchors
        
//            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(userBodyHUDPoint.position);
//            Vector2 localPoint;
//            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
//            {
//                userBodyHUD.localPosition = localPoint;
//            }
        
    }
}
