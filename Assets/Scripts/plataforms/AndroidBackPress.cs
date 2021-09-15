using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackPress : MonoBehaviour {
    
	#if UNITY_ANDROID
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AndroidJavaObject activity =
                    new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                    .GetStatic<AndroidJavaObject>("currentActivity");

            activity.Call<bool>("moveTaskToBack", true);
        }
    }
#endif
}
