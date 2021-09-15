using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : ElementBase {
    
    public float speed;
    public GameObject fishHook;

    private FishHook fishHookController;


    public override void applyEffectOnCollisionEnter(ref DiverController player)
    {
        
    }
    public override void applyEffectOnCollisionExit(ref DiverController player)
    {
        
    }
    public override void applyEffectOnTriggerEnter(ref DiverController player)
    {
        playMySound();
        player.gameOver();

    }
    public override void applyEffectOnTriggerExit(ref DiverController player)
    {
        
    }

    void Start () {

        fishHookController = fishHook.GetComponent<FishHook>();
	}
	
	void Update () 
    {
        float nextX =-1* speed * Time.deltaTime;
        if (fishHook != null && !fishHookController.isSharkEnter())
        {
            Vector3 position = transform.position;
            position.y = fishHook.transform.position.y;
            transform.position = position;
        }

		if (GameController.getGameState().Equals(GameController.GameState.PLAYING) ||
		    GameController.getGameState().Equals(GameController.GameState.GAME_OVER) ){
            transform.Translate(nextX,0,0);
        }
	}
}
