using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherFish : MonoBehaviour
{

    public float speed;

    void Update()
    {
        Vector3 vectorSpeed = Vector3.left * speed * Time.deltaTime;
        transform.Translate(vectorSpeed); 
    }
}
