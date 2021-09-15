using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    public Animator animator;
    public ChangeCharacterAnimationHandler animationHandler;
    public Image entryImage;
    public Image currentImage;
    public Text nameCharacterText;

    private InventoryItem currentCharacter;
    private int currentCharacterIndex;
    private IList<string> characterList;
    private IDictionary<string, InventoryItem> charactersDictionary;
    private InventoryController inventoryController;
    private bool runningAnimation;
    private IList<Action> transitions;

    private const string ANIMATION_RIGTH = "swipeRigth";
    private const string ANIMATION_LEFT = "swipeLeft";

    private void Start()
    {
        animationHandler.OnEntryFinishEvent += OnTransitionFinish;
        transitions = new List<Action>();
    }

    public void Setup(IDictionary<string, InventoryItem> characters,string selectedCharacter, InventoryController inventoryController)
    {
        this.charactersDictionary = characters;
        this.inventoryController = inventoryController;
        this.currentCharacter = charactersDictionary[selectedCharacter];
        this.characterList = new List<string>();
        foreach (string key in charactersDictionary.Keys)
        {
            characterList.Add(key);
            if (key.Equals(selectedCharacter))
            {
                currentCharacterIndex = characterList.Count - 1;
            }
        }
        this.currentImage.sprite = currentCharacter.baseItem.Image;
        this.nameCharacterText.text = currentCharacter.baseItem.name;
    }
    
    public void NextRigth()
    {
        if (!runningAnimation)
        {
            runningAnimation = true;
            currentCharacterIndex++;
            if (currentCharacterIndex >= characterList.Count)
                currentCharacterIndex = 0;
            StartTransitonCharacter();
            animator.SetTrigger(ANIMATION_RIGTH);
        }
        else
        {
            transitions.Add(NextRigth);
        }
    }

    public void NextLeft()
    {
        if (!runningAnimation)
        {
            runningAnimation = true;
            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
                currentCharacterIndex = characterList.Count - 1;
            StartTransitonCharacter();
            animator.SetTrigger(ANIMATION_LEFT);
        }
        else
        {
            transitions.Add(NextLeft);
        }
    }

    private void StartTransitonCharacter()
    {
        currentImage.sprite = currentCharacter.baseItem.Image;
        string characterId = characterList[currentCharacterIndex];
        currentCharacter = charactersDictionary[characterId];
        entryImage.sprite = currentCharacter.baseItem.Image;
    }

    private void OnTransitionFinish()
    {
        runningAnimation = false;
        currentImage.sprite = currentCharacter.baseItem.Image;
        nameCharacterText.text = currentCharacter.baseItem.name;
        if (transitions.Count == 0)
        {
            inventoryController.ChangeCharacter(currentCharacter);
        }
        else
        {
            Action t = transitions[0];
            transitions.RemoveAt(0);
            t();
        }
    }
}
