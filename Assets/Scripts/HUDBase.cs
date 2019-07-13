using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class HUDBase : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI textLabel = null;

    private EntityBase owner;

    public void SetOwner(EntityBase owner)
    {
        this.owner = owner;
        owner.OnDebugInfoUpdate += SetText;
    }

    private void SetText(string text)
    {
        textLabel.text = text;
    }

    public void Clear()
    {
        owner.OnDebugInfoUpdate -= SetText;
        owner = null;
    }
}
