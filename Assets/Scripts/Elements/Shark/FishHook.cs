using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHook : MonoBehaviour{

    public float warningObjectMarging;
    public float top;
    public float bottom;
    public float speed;
    public GameObject shark;
    public GameObject sharkMouth;
    public Animator sharkMouthAnimation;
	public AudioClip radarHigh;

    private bool sharkEnter;
    private Animator warningAnimator;
    private static int maxColumn = -1;
    private static GameObject player;
    private static IList<GameObject> others;
	private AudioSource audioSource;
	private static AudioMixerController audioMixerController;

    private void OnDestroy()
    {
        others.Remove(this.gameObject);
    }
    
    private void Start()
    {
        warningAnimator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		if (audioMixerController == null)
		{
            audioMixerController = GameObject.Find("AudioMixerController").GetComponent<AudioMixerController>();
        }
		audioSource.outputAudioMixerGroup = audioMixerController.sfxGroup;

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (others == null)
        {
            others = new List<GameObject>();
        }
        others.Add(this.gameObject);
        if (maxColumn < 0)
        {
            maxColumn = (int)(Mathf.Abs(top - bottom) / player.transform.lossyScale.y);
            maxColumn = maxColumn / 2;
        }
    }
    
    void Update() 
    { 

        float positionX = rightBorderScreen() - warningObjectMarging;
        Vector3 positionVector = transform.position;
        int index = others.IndexOf(this.gameObject);
        float modifyY = (index % maxColumn) * player.transform.lossyScale.y;
        float modifyX = (index) * (transform.lossyScale.y / maxColumn);

        if ((index + 1) > maxColumn)
        {
            positionX += modifyX;
        }

        positionVector.x = positionX;

        if (!sharkEnter)
        {
            float target = player.transform.position.y;

            if (positionVector.y != target)
            {

                if ((target + modifyY) < top)
                    positionVector.y = Mathf.MoveTowards(positionVector.y, target + modifyY, speed);

                else
                    positionVector.y = Mathf.MoveTowards(positionVector.y, target - modifyY, speed);
                transform.position = positionVector;
            }
        }

        if (positionVector.y > top)
            positionVector.y = Mathf.MoveTowards(positionVector.y, top, speed);

        if (positionVector.y < bottom)
            positionVector.y = Mathf.MoveTowards(positionVector.y, bottom, speed);

        transform.position = positionVector;
    
    }

    public bool isSharkEnter()
    {
        return sharkEnter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(sharkMouth))
        {
            sharkEnter = true;
            changeAnimation(true);
			audioSource.clip = radarHigh;
			audioSource.Play();
        }

        if (collision.gameObject.Equals(shark))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(sharkMouth))
        {
            sharkEnter = false;
            changeAnimation(false);
        }
    }

    private void changeAnimation(bool animationChange)
    {
        sharkMouthAnimation.SetBool("open_mouth", animationChange);
        if (warningAnimator != null)
            warningAnimator.SetBool("fast", animationChange);
    }

    private float rightBorderScreen()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
        return vec.x;
    }
}
