using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //La obliga que el inpector Unity lo lea.
public class ObjectContainer  {

    public GameObject prefab;
    public float yPositionMin;
    public float yPositionMax;
    public float xSpacingMin;
    public float xSpacingMax;
    public float rotationMin;
    public float rotationMax;
    public float generationProbability;
    public float sizeX;
    public float sizeY;
    //==============
    private float multiplicativeFactor;
    private float multiplicativeTime;
    private uint maxCount;
    private static List<GameObject> instance;

    //==============
}
