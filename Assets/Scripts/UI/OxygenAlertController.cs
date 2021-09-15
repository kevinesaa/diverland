using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenAlertController : MonoBehaviour {

	private AudioSource audioSource;
	private Text oxygenAlertTxt;
	private Animator animator;

	void Awake () 
	{
		audioSource = GetComponent<AudioSource>();
		oxygenAlertTxt = GetComponent<Text>();
		animator = GetComponent<Animator>();
	}

	public void EnabledOxgenAlert(bool enabled)
	{
        oxygenAlertTxt.text = enabled?".":"";
	}

	public void OxygenAlertTime(int time)
	{
		oxygenAlertTxt.text = time.ToString();
		animator.SetTrigger("count");
		audioSource.Play();
	}
	

}
