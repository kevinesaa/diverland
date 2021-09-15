using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public event Action OnStartMainMenu;
    public event Action OnPlayGame;
    public event Action OnPauseGame;
    public event Action OnRestartGame;
    /** parámetros: 
          coins: amount of coins collected in a round
          distance: distance traveled in a round
    */
    public event Action<int, float> OnGameOver;

    public GameObject map;
    public GameObject containerGeneratorElements;
    public Transform initialPlayerPosition;
    public CharacterLoader characterLoader;
    public CameraFollow cameraFollow;
    public AudioMixerController mixerController;

    public UIController uiController;
	public int oxygenTime;
	public const int OXYGEN_ALERT = 9;

    private GameObject player;
    private GameObject garbageCollector;
    private float garbageDistance;
	private GeneratorController[] generatorControllersList;
    private int userCoins;
    private int coinsCollectedInRound;
    private float distance = 0;
    private Vector3 startPoint; 
    private float oxygenTimmer = 1;
    private int startOxygenTime;
	private AdController adController;
    private ICoinsService coinsService;

    public string User { get; private set; }
    public static GameController Instance { get; private set; }

    public enum GameState { START, PLAYING, PAUSE, GAME_OVER };

    private static GameState myState = GameState.START;


    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        coinsService = new CoinLocalService();
        coinsService.OnCoinsLoadedSuccessful += OnGetCoinSuccessful;
        coinsService.OnCoinsLoadedError += OnGetCoinsFail;

        characterLoader.OnCharacterLoad += OnCharacterLoader;
        startPoint = initialPlayerPosition.position;
        cameraFollow.SetTarget(initialPlayerPosition.gameObject);
        generatorControllersList = containerGeneratorElements.GetComponentsInChildren<GeneratorController>();
		startOxygenTime = oxygenTime;
        BuildGarbageCollector();
		adController = GetComponent<AdController>();
        characterLoader.LoadCharacter(startPoint);
        User = PlayerPrefs.GetString(Constants.USER,"");
        coinsService.GetUserCoins(User);
    }

    private void OnDestroy()
    {
        coinsService.OnCoinsLoadedSuccessful -= OnGetCoinSuccessful;
        coinsService.OnCoinsLoadedError -= OnGetCoinsFail;
    }

    void Update()
    {
        if (!myState.Equals(GameState.PLAYING))
            return;

        if (myState.Equals(GameState.PLAYING))
        {
            UpdateGarbagePosition();
            distance = Mathf.Abs(startPoint.x - player.transform.position.x);
            UpdateOxygenTimmer();
        }
    }

    private void UpdateOxygenTimmer()
    {
        oxygenTimmer -= Time.deltaTime;
        if (oxygenTimmer <= 0)
        {
            oxygenTimmer = 1;
            addTime(-1);
        }
    }

    private void OnGUI()
    {
        if (!myState.Equals(GameState.PLAYING))
            return;
        DisplayUI();
    }

    private void OnCharacterLoader(GameObject character)
    {
        player = character;
        player.GetComponent<DiverController>().setGameController(Instance);
        cameraFollow.SetTarget(player);
        map.GetComponent<MapController>().SetPlayer(player);
        player.SetActive(false);
    }

    private void OnGetCoinSuccessful(int coins)
    {
        userCoins = coins;
        uiController.ShowTotalCoins(coins);
    }

    private void OnGetCoinsFail(string error, string message)
    {

    }

    public void StartMainMenu()
    {
        myState = GameState.START;
        if (OnStartMainMenu != null)
            OnStartMainMenu();
    }

    public void PlayGame()
    {
        myState = GameState.PLAYING;
        if (OnPlayGame != null)
            OnPlayGame();
    }

    public void PauseGame()
    {
        myState = GameState.PAUSE;
        if (OnPauseGame != null)
            OnPauseGame();
    }

    public void RestartGame()
    {
        myState = GameState.PLAYING;
        if (OnRestartGame != null)
            OnRestartGame();
    }
    
    //todo remove todo param
    public void GameOver(int todo)
    {
        myState = GameState.GAME_OVER;
        if (OnGameOver != null)
            OnGameOver(coinsCollectedInRound, distance);
    }

	public void addTime(int oxygenTime)
    {
        this.oxygenTime += oxygenTime; 
		uiController.EnabledOxygenAlert(this.oxygenTime <= OXYGEN_ALERT);
		if (this.oxygenTime <= OXYGEN_ALERT)
		{
			uiController.OxygenAlert(this.oxygenTime);
		}

        if (this.oxygenTime <= 0)
        {
            myState = GameState.GAME_OVER;
            this.oxygenTime = 0; 
        }
    }

    public void addCoins(int coin)
    {
        this.coinsCollectedInRound += coin;
        if (this.coinsCollectedInRound < 0)
            this.coinsCollectedInRound = 0;
    }

    void DisplayUI()
    {
        uiController.displayTime(oxygenTime);
        uiController.displayCoins(coinsCollectedInRound);
        uiController.displayDistance(distance);
    }

    public void play()
    {
        player.SetActive(true);
		player.GetComponent<DiverController>().RestartPlayer();
        initGame();
        myState = GameState.PLAYING;
        uiController.play();
		uiController.EnabledOxygenAlert(false);
		foreach (GeneratorController generator in generatorControllersList)
			generator.StartAutomaticGenerator();
    }

    public void playFromPause()
	{
		StartCoroutine(PlayFromPauseCoroutine());
	}

    public void pause()
    {
        myState = GameState.PAUSE;
        uiController.pause();
    }

    public void GameOver()
    {
        myState = GameState.GAME_OVER;
		float bestDistance = PlayerPrefs.GetFloat(Constants.BEST_DISTANCE, 0);
		
		uiController.gameOver(distance, coinsCollectedInRound, bestDistance);
		if (distance > bestDistance)
		{
			PlayerPrefs.SetFloat(Constants.BEST_DISTANCE, distance);
		}
        uiController.ShowTotalCoins(userCoins + coinsCollectedInRound);
        coinsService.AddCoinsToUser(User,coinsCollectedInRound);
		PlayerPrefs.Save();
		adController.ShowAd();
    }
    
    public void ToMainMenu()
    {
        myState = GameState.START;
        initGame();
        player.SetActive(false);
        uiController.ToMainMenu();
    }

    private void initGame()
    {
        player.transform.position = startPoint;
        oxygenTimmer = 1;
        coinsCollectedInRound = 0;
        oxygenTime = startOxygenTime;
        player.GetComponent<DiverController>().RestartPlayer();
        map.GetComponent<MapController>().RestartMap();
		foreach (GeneratorController generator in generatorControllersList)
		{
			generator.CancelAutomaticGenerator();
            generator.DestroyObjectScene();
		}
    }

    public static void setGameState(GameState newGameState)
    {
        GameController.myState = newGameState;
    }

    public static GameState getGameState()
    {
        return myState;
    }

    public void ImageMoreTime()
    {
        uiController.ImageMoreTime();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (myState.Equals(GameState.PLAYING))
            {
                this.pause();
            }
        }
    }

    private void BuildGarbageCollector()
    {
        MapController mapController = map.GetComponent<MapController>();
        float heightCollectorY = 2 * mapController.Height;
        float startPositionCollector = -1.65f * mapController.Width;
        garbageCollector = new GameObject("garbageCollector");
        garbageCollector.transform.localScale = new Vector3(1, heightCollectorY, 0);
        garbageCollector.transform.position = startPositionCollector * Vector3.right;
        garbageCollector.AddComponent<BoxCollider2D>();
        var rigidBody=garbageCollector.AddComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        garbageCollector.AddComponent<GarbageController>();
        garbageDistance = startPoint.x - garbageCollector.transform.position.x;
    }

    private void UpdateGarbagePosition()
    {
        Vector3 current = garbageCollector.transform.position;
        current.x = player.transform.position.x - garbageDistance;
        garbageCollector.transform.position = current;
    }

	private IEnumerator PlayFromPauseCoroutine()
	{
		uiController.playFromPause();
		yield return new WaitForSeconds(1);
		myState = GameState.PLAYING;
        StopCoroutine(PlayFromPauseCoroutine());
	}
}