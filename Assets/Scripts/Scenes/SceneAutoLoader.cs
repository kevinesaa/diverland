using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAutoLoader : MonoBehaviour {


    public SceneController sceneController;
    public SceneController.SceneEnum scene;

    void Start () {

        sceneController.ChangeScena(scene);
    }

}
