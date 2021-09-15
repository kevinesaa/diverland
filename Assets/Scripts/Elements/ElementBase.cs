using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementBase : MonoBehaviour
{  
    
    public AudioClip audioClip;
    private AudioSource audioSouce;
    private Renderer myRender;
	private static AudioMixerController audioMixerController;

    public abstract void applyEffectOnCollisionEnter(ref DiverController player);
    public abstract void applyEffectOnCollisionExit(ref DiverController player);
    public abstract void applyEffectOnTriggerEnter(ref DiverController player);
    public abstract void applyEffectOnTriggerExit(ref DiverController player);

    private void Awake()
    {
        audioSouce = gameObject.AddComponent<AudioSource>();
        myRender = GetComponent<SpriteRenderer>();

		if (audioMixerController == null)
		{
            audioMixerController = GameObject.Find("AudioMixerController").GetComponent<AudioMixerController>();
		}

		audioSouce.outputAudioMixerGroup = audioMixerController.sfxGroup;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (isPlayer(otherCollider))
        {
            DiverController player = otherCollider.gameObject.GetComponent<DiverController>(); 
            applyEffectOnTriggerEnter(ref player);
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (isPlayer(otherCollider))
        {
            DiverController player = otherCollider.gameObject.GetComponent<DiverController>();
            applyEffectOnTriggerExit(ref player);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlayer(collision))
        {
            DiverController player = collision.gameObject.GetComponent<DiverController>();
            applyEffectOnCollisionEnter(ref player);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPlayer(collision))
        {
            DiverController player = collision.gameObject.GetComponent<DiverController>();
            applyEffectOnCollisionExit(ref player);
        }
    }

    public bool isPlayer(Collision2D other)
    {
        DiverController player = other.gameObject.GetComponent<DiverController>();
        return player != null;
    }

    public bool isPlayer(Collider2D other)
    {
        DiverController player = other.gameObject.GetComponent<DiverController>();
        return player != null;
    }

    public void enableRender(bool enabled){
        myRender.enabled = enabled;
    }

    public void playMySound()
    {
        audioSouce.PlayOneShot(audioClip);
    }

}