using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : MonoBehaviour {


    public Animator characterAnimation;

  
    public void CharacterSwimmingDown()
    {
        characterAnimation.SetTrigger("character");
    }
}
