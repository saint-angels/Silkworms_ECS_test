using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private HUDBase hudPrefab;
    
    private AnimationConfig animationCfg;

    private List<HUDBase> activeHUDS = new List<HUDBase>();
    

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;
        
        ObjectPool.Preload(hudPrefab, 10);
    }

    public void ShowHUDInfos(List<SystemHUDInfo.HUDInfo> hudInfos)
    {
        int delta = activeHUDS.Count - hudInfos.Count;

        bool shouldRemoveExcess = delta > 0;
        bool shouldAquireMore = delta < 0;

        if (shouldRemoveExcess)
        {
            for (int i = activeHUDS.Count - 1; i >= activeHUDS.Count - delta; i--)
            {
                ObjectPool.Despawn(activeHUDS[i]);
            }
            
            activeHUDS.RemoveRange(activeHUDS.Count - delta, delta);
        }
        else if (shouldAquireMore)
        {
            for (int i = 0; i < Mathf.Abs(delta); i++)
            {
                HUDBase newHud = ObjectPool.Spawn(hudPrefab, Vector3.zero, Quaternion.identity, hudContainer);
                activeHUDS.Add(newHud);
            }
        }

        for (int hudInfoIdx = 0; hudInfoIdx < hudInfos.Count; hudInfoIdx++)
        {
            var hudInfo = hudInfos[hudInfoIdx];
            
            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(hudInfo.position + Vector3.up);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
            {
                activeHUDS[hudInfoIdx].transform.localPosition = localPoint;
                activeHUDS[hudInfoIdx].SetText(hudInfo.infoString);
            }
        }

    }
}
