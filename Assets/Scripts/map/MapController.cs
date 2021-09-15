using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	public float Height { get { return stage.transform.localScale.y; } }
	public float Width { get { return stage.transform.localScale.x; }}

    
	public GameObject stage;
    public List<Stage> stageList;
	public int minRepeat;
	public int maxRepeat;

    private GameObject player;
    private int repeat;
    private int currentStageBackground = 0;
    private GameObject preStage;
    private StageController preStageController,stageController;
    private Vector3 withVector;
    
    void Awake () 
	{
		repeat = Random.Range(minRepeat, maxRepeat);
		stageController = stage.GetComponent<StageController>();
		withVector = Vector3.right * Width;
		InitPreStage();
    }
	
	
	void Update () {

        if (player != null && player.transform.position.x > stage.transform.position.x)
        {
            preStage.transform.position += withVector * 2;
            changePreStageBackground();
            swap(ref preStage, ref stage);
            swap(ref preStageController, ref stageController);
        }
        
	}

    private void swap<T>(ref T var1,ref T var2)
    {
        var temp = var1; 
        var1 = var2;
        var2 = temp;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void changePreStageBackground()
    {
		int next = currentStageBackground;
		repeat--;
		if (repeat <= 0)
		{
			repeat = Random.Range(minRepeat,maxRepeat);
			do
			{
				next = Random.Range(0, stageList.Count);
			} while (next == currentStageBackground);
		}
        currentStageBackground = next;
		preStageController.ChangeLayers(stageList[next].backgroundLayerList);

    }

    public void InitPreStage()
    {
		Vector3 scale = stage.transform.localScale;
		preStage = Instantiate(stage, transform);
		preStageController = preStage.GetComponent<StageController>();
		stageController.ChangeLayers(stageList[0].backgroundLayerList);
		Vector3 position = preStage.transform.localPosition;
		position.x -= scale.x;
		preStage.transform.localPosition = position;
    }

    public void RestartMap()
    {
		repeat = Random.Range(minRepeat, maxRepeat);
        float zPosition = stage.transform.position.z;
        Vector3 startPosition= new Vector3(0, 0, zPosition);
        stage.transform.position = startPosition;
        preStage.transform.position = startPosition;
        Vector3 preStageLocalPosition = preStage.transform.localPosition;
        preStageLocalPosition.x -= Width;
        preStage.transform.localPosition = preStageLocalPosition;
        currentStageBackground = 0;
		preStageController.ChangeLayers(stageList[0].backgroundLayerList);
		stageController.ChangeLayers(stageList[0].backgroundLayerList);
    }
    
}
