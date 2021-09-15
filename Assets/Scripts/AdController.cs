using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour {

	public string androidGameId;
	public string IosGameId;
	public bool testMode;

	private string gameId;
	private int count = 3;
	private bool finish = true;

	private void Awake()
	{
		gameId = IosGameId;

        #if UNITY_ANDROID
		    gameId = androidGameId;
        #endif

		Advertisement.Initialize(gameId, testMode);
	}

    public void ShowAd()
	{
		if (finish)
			StartCoroutine(ShowAdWhenIsReady());
	}

	private IEnumerator ShowAdWhenIsReady()
	{
		finish = false;
		while (!Advertisement.IsReady())
		{
			yield return null;
		}
		finish = true;
		count++;
		if (count >= 3 && CanShow())
		{
			count = 0;
			ShowOptions showOptions = new ShowOptions();
			showOptions.resultCallback = AdCallback;
			Advertisement.Show(showOptions);
		}
        StopCoroutine(ShowAdWhenIsReady());
    }

	private bool CanShow()
	{
		return GameController.getGameState().Equals(GameController.GameState.GAME_OVER);
	}

	private void AdCallback(ShowResult showResult)
	{
		switch (showResult)
		{
			case ShowResult.Failed: break;
			case ShowResult.Finished: break;
			case ShowResult.Skipped: break;
		}
	}
}
