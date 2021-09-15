using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlowfishDraw : ElementBase {

    public float visionRadius;
    public float speed;

    public int timeValue;
    public bool destroyCollision = false;
    public GameObject clockAnimationPrefab;

    private Animator blowfishAnimator;
    private SpriteRenderer spriteFlip;

    private bool facing = true;
    private bool onCooldown;

    GameObject player;
    Vector3 initialPosition;

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

        if (timeValue > 0)
        player.addTimeAnimation();

        GameObject clockAnimationObject = Instantiate(clockAnimationPrefab);
        ClockAnimation clockAnimation = clockAnimationObject.GetComponent<ClockAnimation>();
        clockAnimation.setTarget(player.gameObject);

        if (destroyCollision)
        {
            enableRender(false);
            Destroy(this.gameObject, audioClip.length);
        }

        blowfishAnimator.SetBool("follow", true);

        if (spriteFlip.flipX == false)
        {
            spriteFlip.flipX = true;
            return;
        }
        //else
        //{
        //spriteFlip.flipX = false;
        //}
    }

    public override void applyEffectOnTriggerExit(ref DiverController player)
    {
        blowfishAnimator.SetBool("follow", false);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        blowfishAnimator = GetComponent<Animator>();
        spriteFlip = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        Vector3 target = initialPosition;
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist < visionRadius)
            target = player.transform.position;

        float fixedSpeed = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, fixedSpeed);

        Debug.DrawLine(transform.position, target, Color.green);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

}
