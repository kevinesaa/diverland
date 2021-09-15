using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour {

	public AudioMixer audioMixer;
	public AudioMixerGroup masterGroup;
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup sfxGroup;

	public const string MASTER_GROUP_VOL = "masterVol";
	public const string MUSIC_GROUP_VOL = "musicVol";
	public const string SFX_GROUP_VOL = "sfxVol";
	public const float MIN_VALUE = 0.001f;
	public const float MAX_VALUE = 1f;

	private float masterVol;
	private float musicVol;
	private float sfxVol;

	private void Awake()
	{
        
		musicVol = PlayerPrefs.GetFloat(MUSIC_GROUP_VOL, MAX_VALUE);
		sfxVol = PlayerPrefs.GetFloat(SFX_GROUP_VOL, MAX_VALUE);
		SetVolumeMasterGroup(MAX_VALUE);
		SetVolumeMusicGroup(musicVol);
		SetVolumeSfxGroup(sfxVol);
	}

    public float GetVolumeMasterGroup()
	{
		return masterVol;
	}

	public float GetVolumeMusicGroup()
	{
		return musicVol;
	}

    public float GetVolumeSfxGroup()
	{
		return sfxVol;
	}

    public void SetVolumeMasterGroup(float volumen)
	{
		SetVolume(MASTER_GROUP_VOL, ref volumen);
		masterVol = volumen;
	}

    public void SetVolumeMusicGroup(float volumen)
	{
		SetVolume(MUSIC_GROUP_VOL, ref volumen);
		musicVol = volumen;
	}

	public void SetVolumeSfxGroup(float volumen)
	{
		SetVolume(SFX_GROUP_VOL, ref volumen);
		sfxVol = volumen;
	}

	private void SetVolume(string name, ref float volumen)
	{
		volumen = Range(volumen);
		audioMixer.SetFloat(name, Convert(volumen));
		PlayerPrefs.SetFloat(name, volumen);
		PlayerPrefs.Save();
	}

	private float Range(float vol)
	{
		if (vol < MIN_VALUE)
		{
			vol = MIN_VALUE;
		}
		if (vol > MAX_VALUE)
		{
			vol = MAX_VALUE;
		}
		return vol;
	}

	private float Convert(float vol)
	{
		return Mathf.Log(vol) * 20; 
	}

}
