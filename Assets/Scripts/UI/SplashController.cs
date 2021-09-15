using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SplashController : MonoBehaviour {

    public AudioSource splash1Audio;
    public AudioSource splash2Audio;

    public void PlaySplash1()
    {
        splash1Audio.Play();
    }

    public void PlaySplash2()
    {
        splash2Audio.Play();
    }

}
