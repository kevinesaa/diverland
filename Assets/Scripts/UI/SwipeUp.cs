using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeUp : MonoBehaviour {


	public GameController gameController;
	public float minSwipeDistance;
	private Vector2 startPositionSwipe;
	private bool inside = false;
	private DiverAnimationUI diverAnimation;

	// Update is called once per frame
	private void Awake()
	{
		diverAnimation = GetComponent<DiverAnimationUI>();
	}

	void Update () 
	{
		if (inside && Input.touchCount==1)
		{
			Touch touch = Input.touches[0];
            
			if (touch.phase == TouchPhase.Began)
			{
				startPositionSwipe = touch.position;
			}

			if (touch.phase==TouchPhase.Ended)
			{
				Vector2 endedPosition = touch.position;
				float distance = Mathf.Abs(startPositionSwipe.y - endedPosition.y);
              
				if (startPositionSwipe.y > endedPosition.y && distance > minSwipeDistance)
				{
					StartCoroutine(StartGame());
				}
				inside = false;
			}
		}
	}
    
	public void Inside()
	{
		inside = true;
	}

	private IEnumerator StartGame()
	{
		diverAnimation.SwimmingDown();
		yield return new WaitForSeconds(1.2f);
		gameController.play();
	}
}
