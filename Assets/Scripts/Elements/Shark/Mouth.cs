using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : ElementBase {

	public Animator bodyAnimator;
    public Animator mouthAnimator;
    public const string animatorID = "open_mouth";

    public override void applyEffectOnCollisionEnter(ref DiverController player)
    {
        
    }

    public override void applyEffectOnCollisionExit(ref DiverController player)
    {
        
    }

    public override void applyEffectOnTriggerEnter(ref DiverController player)
    {
        mouthAnimator.SetBool(animatorID,true);
    }

    public override void applyEffectOnTriggerExit(ref DiverController player)
    {
        mouthAnimator.SetBool(animatorID, false);
		AnimatorStateInfo bodyStateInfo = bodyAnimator.GetCurrentAnimatorStateInfo(0);
		AnimatorStateInfo headStateInfo = mouthAnimator.GetCurrentAnimatorStateInfo(0);
		bodyAnimator.speed = 0;
		mouthAnimator.speed = 0;
		mouthAnimator.Play(headStateInfo.fullPathHash, -1, bodyStateInfo.normalizedTime);
		bodyAnimator.speed = 1;
		mouthAnimator.speed = 1;
    }

}
