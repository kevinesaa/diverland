using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour {


    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public Text distanceTxt;
    public Text timeTxt;
    public Text coinTxt;
    public Text totalCoinsMainMenu;
    public Text totalCoinGameOver;
    public Text distanceFinalTxt;
    public Text coinsCollectInRound;
	public Text bestDistanceRecord;
    public Image moreTime;
	public OxygenAlertController oxygenAlertController;

	private Animator pauseAnimator;
    private Image fundido;

	
	void Start () 
    {
        fundido = gameOverPanel.GetComponent<Image>();
        moreTime.CrossFadeAlpha(0, 0, false);
		pauseAnimator = pausePanel.GetComponentInChildren<Animator>();
	}
	
    public void play()
    {
		playPanels();
    }

	public void playFromPause()
	{
		StartCoroutine(playFromPauseCoroutine());
	}

	private void playPanels()
	{
		gamePanel.SetActive(true);
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        fundido.CrossFadeAlpha(0, 1, false);
        moreTime.enabled = false;
	}

    public void pause()
    {
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
		pauseAnimator.SetTrigger("pause_in");
    }

    public void ToMainMenu()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void EnabledOxygenAlert(bool enabled)
	{
		timeTxt.enabled = !enabled;
		oxygenAlertController.EnabledOxgenAlert(enabled);
	}

    public void OxygenAlert(int time)
	{
		if (time < 0)
			time = 0;
		oxygenAlertController.OxygenAlertTime(time);
	}


    public void gameOver(float distance, int coins, float bestDistance)
    {
        fundido.CrossFadeAlpha(1, 1, false);
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
        distanceFinalTxt.text = distance.ToString("F2");
        coinsCollectInRound.text = coins.ToString();
		if (distance > bestDistance)
		{
			StartCoroutine(RecordAnimation(distance));
		}
		else 
		{
			bestDistanceRecord.text = bestDistance.ToString("F2") + " mts";
		}
    }

    public void ShowTotalCoins(int coins)
    {
        totalCoinsMainMenu.text = coins.ToString();
        totalCoinGameOver.text = coins.ToString();
    }

    public void displayDistance(float distance)
    {
        if (distance > 0)
            distanceTxt.text = distance.ToString("F2");
        else
            distanceTxt.text = "0.00";
    }

    public void displayCoins(int coins)
    {
        if (coins > 0)
            coinTxt.text = coins.ToString();
        else
            coinTxt.text = "0";
    }

    public void displayTime(int timeInSeconds)
    {
        if (timeInSeconds>0)
        {
            int minutes = timeInSeconds / 60;
            int seconds = timeInSeconds % 60;
            timeTxt.text = minutes.ToString() + ":" + seconds.ToString().PadLeft(2, '0');
        }
        else{
            timeTxt.text = "00:00";
        }
    }

	public void ImageMoreTime()
	{
        StartCoroutine(CloseImageMoreTime());
    }

    IEnumerator CloseImageMoreTime()
    {
        moreTime.enabled = true;
        moreTime.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(1);
        moreTime.CrossFadeAlpha(0, 0.5f, false);
        StopCoroutine(CloseImageMoreTime());
    }

    IEnumerator playFromPauseCoroutine()
    {
        pauseAnimator.SetTrigger("pause_out");
        yield return new WaitForSeconds(1f);
        playPanels();
        StopCoroutine(playFromPauseCoroutine());
    }

	IEnumerator RecordAnimation(float record)
	{
		bestDistanceRecord.text = "NEW RECORD";
		yield return new WaitForSeconds(1f);
		const float duration = 2f; // In seconds
		float normalizedTime = 0;
		float acum = 0;

		while (normalizedTime < duration)
		{
			bestDistanceRecord.text = acum.ToString("F2");
			normalizedTime += Time.deltaTime / duration;
			acum = record * (normalizedTime / duration);
			yield return null;
		}

		bestDistanceRecord.text = record.ToString("F2") + " mts";
        StopCoroutine(RecordAnimation(record));
	 }
}
