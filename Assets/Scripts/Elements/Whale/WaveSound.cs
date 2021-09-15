using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSound : MonoBehaviour {

    //WaveSound for whale
    public float warningObjectMarging;
    public GameObject whale;

    private static AudioMixerController audioMixerController;

    void Start()
    {
        if (audioMixerController == null)
        {
            audioMixerController = GameObject.Find("AudioMixerController").GetComponent<AudioMixerController>();
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerController.sfxGroup;
    }

    void Update()
    {
        float positionX = rightBorderScreen() - warningObjectMarging;
        float positionY = whale.transform.position.y;

        Vector3 position = transform.position;
        position.x = positionX;
        position.y = positionY;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(whale))
        {
            Destroy(this.gameObject, 0);
        }
    }

    private float rightBorderScreen()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
        return vec.x;
    }
}
