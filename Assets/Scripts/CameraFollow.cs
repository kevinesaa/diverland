using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private GameObject targetObject;
	private float distanceToTarget;
    

	void LateUpdate ()
    {
        if (targetObject != null)
        {
            float targetObjectX = targetObject.transform.position.x;

            Vector3 newCameraPosition = transform.position;
            newCameraPosition.x = targetObjectX + distanceToTarget;
            transform.position = newCameraPosition;
        }
	}

    public void SetTarget(GameObject target)
    {
        targetObject = target;
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }
}
