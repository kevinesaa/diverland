using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public Action<GameObject> OnCharacterLoad;
    public CharacterContainer[] characters;

    private IDictionary<string, GameObject> dictionary;


    private void Awake()
    {
        dictionary = new Dictionary<string, GameObject>();
        foreach (CharacterContainer e in characters)
        {
            dictionary[e.id] = e.prefab;
        }
    }

    public void LoadCharacter(Vector3 position)
    {
        //string characterSelectId = PlayerPrefs.GetString(Constants.DEFAULT_CHARACTER_ID, Constants.DEFAULT_CHARACTER_ID);
        string characterSelectId = PlayerPrefs.GetString(Constants.CHARACTER, Constants.DEFAULT_CHARACTER_ID);
        GameObject prefab= dictionary[Constants.DEFAULT_CHARACTER_ID];
        if (dictionary.ContainsKey(characterSelectId))
             prefab = dictionary[characterSelectId];
        GameObject instance = GameObject.Instantiate(prefab,position,Quaternion.identity);
        instance.name = "Player";
        if (OnCharacterLoad != null)
            OnCharacterLoad(instance);
    }

    [System.Serializable]
    public struct CharacterContainer
    {
        public string name;
        public string id;
        public GameObject prefab;
    }
}
