using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverController : MonoBehaviour
{
    public ParticleSystem oxygen;
    public float oxygenBubble = 100.0f;
    public float movementSpeed = 3.0f;
	public float aceleration = 0;
	public float delayAceleration;
	public float frencuencyAceleration;
	public float maxAceleration;

	private float acelerationAcum = 0;
	private bool acelerationBool = false;
    private Vector3 lastPosition;
    private float gravity;
    private bool dead = false;
	private bool swimmingUp;
    private Rigidbody2D rb2d;
    private Animator animator;
    private GameController gameController;
    private IDictionary<string, float> animationDictionary;
	private AudioSource audioBreathing;

    private void Awake()
    {
        audioBreathing = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        initAnimationDictionary();
    }

    void Start()
    {       
        rb2d = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        gravity = rb2d.gravityScale; 
    }

   
    public void addCoin(int count )
    {
        gameController.addCoins(count);
    }

    public void addTime(int extraTime)
    {
        gameController.addTime(extraTime);
    }

    public void setGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    void Update()
    {
		if (GameController.getGameState().Equals(GameController.GameState.START) ||
            GameController.getGameState().Equals(GameController.GameState.PAUSE))
		{
			return;
		}

		if (GameController.getGameState().Equals(GameController.GameState.GAME_OVER))
        {
            if (!dead)
            {
                animator.SetBool("dead", true);
                StartCoroutine(gameOverAnimation());
                audioBreathing.Stop();
            }
            return;
        }
		swimmingUp = Input.GetButton("Fire1");
		swimmingUp = swimmingUp && !dead;
		animator.SetBool("swimming-up", swimmingUp); 
		AdjustOxygen(swimmingUp); 
        lastPosition = transform.position;

    }

    void FixedUpdate()
    {
        if (GameController.getGameState().Equals(GameController.GameState.START) ||
            GameController.getGameState().Equals(GameController.GameState.PAUSE))
        {
            transform.position = lastPosition;
            rb2d.gravityScale = 0;
            return;
        }
        
		if (GameController.getGameState().Equals(GameController.GameState.PLAYING))
		{
			rb2d.gravityScale = gravity;
            rb2d.AddForce(acelerationAcum * Vector2.down);
			if (swimmingUp)
            {
                rb2d.AddForce((oxygenBubble + acelerationAcum) * Vector3.up);
            }

            if (!dead)
            {
                Vector2 newVelocity = rb2d.velocity;
                newVelocity.x = movementSpeed + acelerationAcum;
                rb2d.velocity = newVelocity;
            }

		}
    }

    public void gameOver()
    {
        GameController.setGameState(GameController.GameState.GAME_OVER);
    }

    public void RestartPlayer()
    {
		CancelAceleration();
        dead = false; 
        animator.SetBool("dead", false);
		audioBreathing.Play();
		StartAceleration();
    }

    public void addTimeAnimation()
    {
        gameController.ImageMoreTime();
    }

    void AdjustOxygen(bool jetpackActive)
    {
        int value = jetpackActive ? 25 : 3;
        ParticleSystem.MainModule main = oxygen.main;
        main.maxParticles = value;
    }

    private void initAnimationDictionary()
    {
        animationDictionary = new Dictionary<string, float>();
  
        AnimationClip[] clipsArray = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clipsArray)
        {
            animationDictionary.Add(clip.name, clip.length);
        }
    }

    private IEnumerator gameOverAnimation() 
    {
        dead = true;
        gameController.PauseOff();
        yield return new WaitForSeconds(animationDictionary["deading"]+2);
        gameController.GameOver();
    }

	private void Aceleration()
	{
		if (GameController.getGameState().Equals(GameController.GameState.PLAYING))
			acelerationAcum += aceleration;
		if (acelerationAcum > maxAceleration)
		{
			acelerationAcum = maxAceleration;
		}
	}

	private void StartAceleration()
	{
		if (!acelerationBool)
		{
			InvokeRepeating("Aceleration", delayAceleration, frencuencyAceleration);
			acelerationBool = true;
		}
	}

	private void CancelAceleration()
	{
		CancelInvoke("Aceleration");
		acelerationAcum = 0;
		acelerationBool = false;
	}
}