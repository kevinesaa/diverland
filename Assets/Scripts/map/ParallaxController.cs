using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    
    public float parallaxSpeedX = 0; // Controla la velocidad de parallax
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void Update ()
    {
        if (myRenderer.materials.Length>0)
        {
            Vector2 offset = Vector2.right * Time.deltaTime * parallaxSpeedX; 
            myRenderer.material.mainTextureOffset += offset;
            float x = myRenderer.material.mainTextureOffset.x;

            if (Mathf.Abs(x) > 1)
            {
                x = 0;

            }
            offset.x = x;
            myRenderer.material.mainTextureOffset = offset;            
        }
    }

    public void setMaterial(Material material)
    {
        myRenderer.material = material;
    }

}
