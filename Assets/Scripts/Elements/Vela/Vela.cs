using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vela : ElementBase {

    public float speed;
    public float borderEnter;

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

    void Update()
    {
        Vector3 vectorSpeed = Vector3.left * speed * Time.deltaTime;


		if (GameController.getGameState().Equals(GameController.GameState.PLAYING) ||
		    GameController.getGameState().Equals(GameController.GameState.GAME_OVER))
			
            {
                transform.Translate(vectorSpeed);
            }
    }
}
