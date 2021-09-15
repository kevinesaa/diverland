using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComingsSoon : MonoBehaviour {

	public GameObject comingSoonPanel;

    public void show()
	{
		comingSoonPanel.SetActive(true);
	}

    public void hide()
	{
		comingSoonPanel.SetActive(false);
	}

}
