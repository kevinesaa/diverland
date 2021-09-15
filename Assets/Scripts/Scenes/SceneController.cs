using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [System.Serializable]
    public enum SceneEnum
    {
        PLAY=0,
        SHOP=1,
        INVENTORY=2
    }
    public Slider loadingBar;
    
    private const string PLAY_NAME = "DiverLand";
    private const string SHOP_NAME = "Shop";
    private const string INVENTORY_NAME = "Inventory";

    private void Start()
    {
        UpdateProgress(0);
    }
    
    public void ChangeScena( int sceneEnum)
    {
        ChangeScena((SceneEnum)sceneEnum);
    }

    public void ChangeScena(SceneEnum scene)
    {
        string sceneName=SelectScena(scene);
        StartCoroutine(Loading(sceneName));
    }

    private string SelectScena(SceneEnum scene)
    {
        string sceneName = "";
        switch (scene)
        {
            case SceneEnum.PLAY: sceneName = PLAY_NAME; break;
            case SceneEnum.SHOP: sceneName = SHOP_NAME; break;
            case SceneEnum.INVENTORY: sceneName = INVENTORY_NAME; break;
        }
        return sceneName;
    }

    private void UpdateProgress(float progress)
    {
        if(loadingBar!=null)
        {
            loadingBar.value = progress;
        }
    }

    private IEnumerator Loading(string sceneName)
    {
        AsyncOperation scenaAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!scenaAsyncOperation.isDone)
        {
            UpdateProgress(scenaAsyncOperation.progress);
            yield return null;
        }
        StopCoroutine(Loading(sceneName));
    }
}
