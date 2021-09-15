using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : ElementBase {

    public int coinValue = 1;
	public string targetImageName;
	public float speed;

	private static Image target;
	private bool startMove = false;

    public override void applyEffectOnCollisionEnter(ref DiverController player)
    {
        
    }
    public override void applyEffectOnCollisionExit(ref DiverController player)
    {
        
    }
    public override void applyEffectOnTriggerEnter(ref DiverController player)
    {
        player.addCoin(coinValue);
        playMySound();
		startMove = true;

    }
    public override void applyEffectOnTriggerExit(ref DiverController player)
    {
        
    }

	// Use this for initialization
	void Start () 
    {
		Animator animator = GetComponent<Animator>();
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));

		if (target == null)
		{
			target = GameObject.Find(targetImageName).GetComponent<Image>();
		}
 	}
	
	// Update is called once per frame
	void Update () 
    {
		if (startMove)
		{
			Vector3 targetVector = Camera.main.ScreenToWorldPoint(target.transform.position);
			transform.position = Vector3.MoveTowards(transform.position, targetVector, speed * Time.deltaTime);
			if (Vector3.Equals(transform.position,targetVector+Vector3.one))
			{
				Destroy(this.gameObject);
			}
		}
	}


}
