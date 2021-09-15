using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {


    public List<ObjectContainer> objectsToGenerate; 
    public float delayGenerator; 
    public float frecuency; 
	public int maxCountObject;

    private const int BUFFER_SIZE_COLLIDER_2D = 16;
    private ObjectContainer lastObjectInstance; 
    private IList<GameObject> objectsInScenes; 
    private Collider2D[] bufferCollider2D=new Collider2D[BUFFER_SIZE_COLLIDER_2D];

	
	void Start () {
        objectsInScenes = new List<GameObject>();
	}

	private void OnDestroy()
	{
		CancelAutomaticGenerator();
	}

	public void GenerateObject(int index)
    {
        if (objectsToGenerate!=null && 
            index >=0 && 
            index <objectsToGenerate.Count)
        {
            if (objectsToGenerate[index].prefab != null)
            {

                float[] position=GenerateAtRandomPosition(index, 5);
                if (position != null)
                {
                    float xPosition = position[0];
                    float yPosition = position[1];


                    GameObject myGameObject = Instantiate(objectsToGenerate[index].prefab);
                    Vector3 positionVector = myGameObject.transform.position;
                    positionVector.Set(xPosition, yPosition, 0);
                    myGameObject.transform.position = positionVector;
                    objectsInScenes.Add(myGameObject);
                    lastObjectInstance = objectsToGenerate[index];
					DeleteFromListOnDestroy deleteFromList = myGameObject.AddComponent<DeleteFromListOnDestroy>();
					deleteFromList.setOwnerList(ref objectsInScenes);
                }

            }
        }
    }

    public float[] GenerateAtRandomPosition(int index, int attempts)
    {
        float[] position=null;
        if (objectsToGenerate != null && index >= 0 && index < objectsToGenerate.Count)
        {
            Vector3 size=new Vector3(objectsToGenerate[index].sizeX,objectsToGenerate[index].sizeY);
            do
            {
                
                float xPosition = rightBorderScreen() + objectsToGenerate[index].xSpacingMin;
                xPosition += Random.Range(objectsToGenerate[index].xSpacingMin, objectsToGenerate[index].xSpacingMax);
                float yPosition = Random.Range(objectsToGenerate[index].yPositionMin, objectsToGenerate[index].yPositionMax);

                if (lastObjectInstance != null)
                {
                    xPosition += lastObjectInstance.xSpacingMax;
                }
                Vector3 temp = new Vector3(xPosition, yPosition);
                if (IsEmpty(temp, size))
                {
                    position = new float[3];
                    position[0] = temp.x;
                    position[1] = temp.y;
                    position[2] = temp.z;
                    break;
                }
                attempts--;
            }while (attempts > 0);
        }

        return position;
    }

    public bool IsEmpty(Vector3 position,Vector3 size)
    {
        Vector2 positionVector2 = new Vector2(position.x, position.y);
        Vector2 sizeVector2 = new Vector2(size.x, size.y);

        return Physics2D.OverlapBoxNonAlloc(positionVector2,sizeVector2,0,bufferCollider2D) <= 0;
    }

    private void AutomaticGenerator()
    {
		if (GameController.getGameState().Equals(GameController.GameState.PLAYING) && objectsInScenes.Count < maxCountObject )
        {
            int index = Random.Range(0, objectsToGenerate.Count);
            GenerateObject(index);
        }
    }

    public void StartAutomaticGenerator()
    {
        InvokeRepeating("AutomaticGenerator", delayGenerator,frecuency); 
    }

    public void CancelAutomaticGenerator()
    {
        CancelInvoke("AutomaticGenerator");
        DestroyObjectScene();
    }

    public void DestroyObjectScene()
    {
        foreach (GameObject element in objectsInScenes) 
        {
            Destroy(element);
        }
        objectsInScenes.Clear();
    }

    private float rightBorderScreen()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
        return vec.x;
    }
}
