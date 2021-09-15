using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

	public Slider musicSlider;
	public Slider sfxSlider;


	// Use this for initialization
	void Start () {
		musicSlider.value = PlayerPrefs.GetFloat(AudioMixerController.MUSIC_GROUP_VOL, AudioMixerController.MAX_VALUE);
		sfxSlider.value = PlayerPrefs.GetFloat(AudioMixerController.SFX_GROUP_VOL, AudioMixerController.MAX_VALUE);
	}
	

}
