using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class BaseItemServer : BaseItem
{
    [SerializeField]
    [HideInInspector]
    private string imageBase64;

    [System.NonSerialized]
    private Sprite image;

    public override Sprite Image
    {
        get
        {
            if (image == null && imageBase64 == null)
                return null;

            if (image == null)
                image = ToSpite(imageBase64);
            if (imageBase64 == null)
                imageBase64 = ToBase64(image);

            return image;
        }
    }

    public void SetImage(Sprite image)
    {        
        this.image = image;
        this.imageBase64 = ToBase64(image);
    }

    private string ToBase64(Sprite image)
    {
        byte[] bytes = image.texture.EncodeToPNG();
        return System.Convert.ToBase64String(bytes);
    }

    private Sprite ToSpite(string stringBase64)
    {
        byte[] bytes = System.Convert.FromBase64String(stringBase64);
        Texture2D texture = new Texture2D(1,1);
        texture.LoadImage(bytes);
        texture.Apply();
        return Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2());
    }
}
