using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ErrorController : MonoBehaviour {

    public Button tryAgainButton;
    public Button cancelButton;
    public Text errorDescriptionText;
    public ScrollRect scrollRect;

    private void Start()
    {
        cancelButton.onClick.AddListener(Hide);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void ShowCancelButton(bool show)
    {
        cancelButton.gameObject.SetActive(show);
    }

	public void Setup(string errorDescription, UnityAction tryAgainAction)
    {
        tryAgainButton.onClick.RemoveAllListeners();
        tryAgainButton.onClick.AddListener(tryAgainAction);
        tryAgainButton.onClick.AddListener(Hide);
        errorDescriptionText.text = errorDescription;
        scrollRect.verticalNormalizedPosition = 1;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
