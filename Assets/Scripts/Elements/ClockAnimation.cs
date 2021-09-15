using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimation : MonoBehaviour {

	public float lifeTime = 1f;
    public float speedUpward;
    public float timeUp;

	private GameObject target;

    [SerializeField]
    private TextMesh textMeshToUse;
    [SerializeField]
    private bool fadeOnStart = false;
    [SerializeField]
    private float timeMultiplier;

    Transform time;

    // Use this for initialization
    void Start () 
	{
        if (textMeshToUse)
        {
            textMeshToUse = GetComponent<TextMesh>();
            //Initial(10); Con esta funcion imprime numeros de textMesh
        }

        if (fadeOnStart)
        {
            StartCoroutine(FadeOutText(timeMultiplier, textMeshToUse));
        }

        Destroy(this.gameObject, lifeTime);
    }
   
	// Update is called once per frame
	void Update () 
	{
		if (target != null)
		{
			Vector3 position = target.transform.position;
           
			position.y += gameObject.transform.lossyScale.y;
			gameObject.transform.position = position;
        }

        /*if (time != null)
        {
           transform.LookAt(-time.position);
           transform.position += Vector3.up * speedUpward * Time.deltaTime; // Esta funcion haces que se desplaza hacia arriba cierto tiempo desaparece.
        }*/
    }

    public void Initial(int numeros)
    {
        time = Camera.main.transform;
        textMeshToUse.text = "+ " + numeros.ToString();
        Destroy(gameObject, timeUp);
    }

    public void setTarget(GameObject target)
	{
		this.target = target;
	}

    private IEnumerator FadeOutText(float timeSpeed, TextMesh textMesh)
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);

        while (textMesh.color.a > 0.0f)
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
