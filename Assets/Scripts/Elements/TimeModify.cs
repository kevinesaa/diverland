using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeModify : ElementBase {

    public int timeValue;
	public bool destroyCollision = false;
	public GameObject clockAnimationPrefab;

    public override void applyEffectOnCollisionEnter(ref DiverController player)
    {
        
    }

    public override void applyEffectOnCollisionExit(ref DiverController player)
    {
        
    }

    public override void applyEffectOnTriggerEnter(ref DiverController player)
    {
        player.addTime(timeValue);
        playMySound();
        
		if(timeValue > 0)
            player.addTimeAnimation();
		
		GameObject clockAnimationObject = Instantiate(clockAnimationPrefab);
		ClockAnimation clockAnimation = clockAnimationObject.GetComponent<ClockAnimation>();
		clockAnimation.setTarget(player.gameObject);

		if (destroyCollision)
		{
			enableRender(false);
			Destroy(this.gameObject, audioClip.length);
		}
    }

    public override void applyEffectOnTriggerExit(ref DiverController player)
    {
        
    }
    
}
