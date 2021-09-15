using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraScaling : MonoBehaviour {

    public float targetWidth;
    public float pixelsToUnits;

    private Camera mainCamera;


	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
        mainCamera.orthographicSize = height / pixelsToUnits / 2;
	}
}
