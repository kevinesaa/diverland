using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurWeb : MonoBehaviour {

    public void OpenWeb()
	{
		Application.OpenURL(Constants.WEB);
	}

}
