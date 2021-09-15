using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage  {

    [System.Serializable]
    public enum LayerTypeEnum {FOREGROUND, MIDDLEGROUND, BACKGROUND }; //Capas nombrada por Tag. 

    [System.Serializable]
    public struct StageBackgroundLayer {
        public LayerTypeEnum layerType;
        public float parallaxSpeedX; 
        public Material material;
    }

    public List<StageBackgroundLayer> backgroundLayerList; //Se agrega la lista Capas de un Fondo.
}
