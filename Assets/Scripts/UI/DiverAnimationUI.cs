using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverAnimationUI : MonoBehaviour {

	public Animator diverAnimator;
	public Animator dialogueAnimator;

	private bool swimming;
	private const string IDLE = "idle";
	private const string COME_ON = "come-on";
	private const string SWIMMING_DOWN = "swimming-down";

	private void OnEnable()
	{
		swimming = false;
		Idle();
		InvokeRepeating("AutomaticComeOn", 1, 5);
	}

	private void OnDisable()
	{
		CancelInvoke("AutomaticComeOn");
	}

	public void ComeOn()
	{
		diverAnimator.SetTrigger(COME_ON);
		dialogueAnimator.SetTrigger(COME_ON);
	}

    public void Idle()
	{
		diverAnimator.SetTrigger(IDLE);
		dialogueAnimator.SetTrigger(IDLE);
	}

    public void SwimmingDown()
	{
		if (!swimming)
		{
			swimming = true;
			diverAnimator.SetTrigger(SWIMMING_DOWN);
			dialogueAnimator.SetTrigger(SWIMMING_DOWN);
		}
	}

	private void AutomaticComeOn()
	{
		if (!swimming)
			ComeOn();
	}
}
