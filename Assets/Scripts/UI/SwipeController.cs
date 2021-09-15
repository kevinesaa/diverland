using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour {

    public event Action<Vector2, Vector2> OnSwipeAction;

    public float minSwipeDistance;
    private Vector2 startPosition;
    private bool insideSwipeArea=false;

    // Update is called once per frame
    void Update ()
    {
        if (insideSwipeArea && Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (insideSwipeArea && OnSwipeAction!=null)
                {
                    Vector2 endedPosition = touch.position;
                    float distance = Vector2.Distance(startPosition, endedPosition);
                    distance = Mathf.Abs(distance);
                    if (distance > minSwipeDistance)
                        OnSwipeAction(startPosition, endedPosition);
                }
                insideSwipeArea = false;
            }
        }

    }

    public void InsideSwipeAreaFuncActive()
    {
        insideSwipeArea = true;
    }
}
