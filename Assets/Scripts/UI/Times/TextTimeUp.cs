using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTimeUp : MonoBehaviour {

    //public TextMesh numero;
    public float speedUpward;
    public float timeUp;

    [SerializeField]
    private TextMesh textMeshToUse;
    [SerializeField]
    private bool fadeOnStart = false;
    [SerializeField]
    private float timeMultiplier;
 
    Transform time;
    TimeModify timeValue;

    // Use this for initialization
    void Start()
    {
        if (textMeshToUse)
        {
            textMeshToUse = GetComponent<TextMesh>();
            Initial(10);
        }

        if (fadeOnStart)
        {
                StartCoroutine(FadeOutText(timeMultiplier, textMeshToUse));
        }
    }

    public void Initial(int numeros)
    {
        time = Camera.main.transform;
        textMeshToUse.text = "+ " + numeros.ToString();
        Destroy(gameObject, timeUp);
    }

    // Update is called once per frame
    void Update () 
    {
        if (time != null)
        {
            transform.LookAt(-time.position);
            transform.position += Vector3.up * speedUpward * Time.deltaTime;
        }
	}

    private IEnumerator FadeOutText(float timeSpeed, TextMesh textMesh)
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);

        while(textMesh.color.a > 0.0f)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textMesh.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }

    public void FadeOutText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }

        StartCoroutine(FadeOutText(timeSpeed, textMeshToUse));
    }

}
