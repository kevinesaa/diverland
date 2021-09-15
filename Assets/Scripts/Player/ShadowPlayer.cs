using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPlayer : MonoBehaviour
{

    public float yPosition;
    public GameObject owner;
    public Vector3 maxScale;

    
    void Update()
    {
        UpdatePositionY();
        updateScale();
    }

	private void UpdatePositionY ()
	{
        Vector3 position = transform.position;
        position.y = yPosition;
        transform.position = position;
	}

    private void updateScale()
    {
        float d = distance();
        if (d <= 1f)
        {
            d = 1f;
        }

        Vector3 scale = maxScale/d;

        transform.localScale = scale;
    }

    private float distance()
    {
        return Mathf.Abs(transform.position.y - owner.transform.position.y);
    }
}

