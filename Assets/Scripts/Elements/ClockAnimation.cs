using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimation : MonoBehaviour {

	public float lifeTime = 1f;

	private GameObject target;
	   

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, lifeTime);

	}
   
	// Update is called once per frame
	void Update () 
	{
		if (target != null)
		{
			Vector3 position = target.transform.position;
           
			position.y += gameObject.transform.lossyScale.y;
			gameObject.transform.position = position;
		}
	}

	public void setTarget(GameObject target)
	{
		this.target = target;

	}

    

}
