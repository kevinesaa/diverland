using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

	public ParallaxController foregroundParallaxController;
	public ParallaxController middlegroundParallaxController;
	public ParallaxController backgroundParallaxController;
    
	public void ChangeLayers(List<Stage.StageBackgroundLayer> backgroundLayersList)
	{
		foreach (Stage.StageBackgroundLayer element in backgroundLayersList)
		{
			switch(element.layerType)
			{
                case Stage.LayerTypeEnum.FOREGROUND:
                    layerToChange(ref foregroundParallaxController, element);
                    break;

                case Stage.LayerTypeEnum.MIDDLEGROUND:
                    layerToChange(ref middlegroundParallaxController, element);
                    break;

                case Stage.LayerTypeEnum.BACKGROUND:
                    layerToChange(ref backgroundParallaxController, element);
                    break;
            }
		}
	}

	private void layerToChange(ref ParallaxController parallaxController, Stage.StageBackgroundLayer layer)
    {
        parallaxController.setMaterial(layer.material);
        parallaxController.parallaxSpeedX = layer.parallaxSpeedX;
    }

}
