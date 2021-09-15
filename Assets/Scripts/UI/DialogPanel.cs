using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : MonoBehaviour {

	public GameObject dialogPanel;

    public void Show()
    {
        dialogPanel.SetActive(true);
    }

    public void Hide()
    {
        dialogPanel.SetActive(false);
    }

}

