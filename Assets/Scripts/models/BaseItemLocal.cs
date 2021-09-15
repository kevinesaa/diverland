using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItemLocal : BaseItem
{
    [SerializeField]
    private Sprite sprite;

    public override Sprite Image
    {
        get
        {
            return sprite;
        }
    }
}
