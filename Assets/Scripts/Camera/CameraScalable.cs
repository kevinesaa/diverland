using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalable : MonoBehaviour {

    public float targetWidth;
    public float targetHeight;
    public int pixelsToUnits;

    private float targetRatio;
    private float currentRatio;

	// Use this for initialization
	void Start () {
        targetRatio = targetWidth / targetHeight;
        currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio>=targetRatio)
        {
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits;
        
        }else{
            
            float difference = targetRatio / currentRatio;
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits * difference;
        }
	}
	
	// Update is called once per frame
	void Update () {
        targetRatio = targetWidth / targetHeight;
        currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= targetRatio)
        {
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits;

        }
        else{
            float difference = targetRatio / currentRatio;
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits * difference;
        }
	}
}
