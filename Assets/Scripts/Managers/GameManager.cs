using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
	public static event Action OnPlayerTurnEnded;
	public static event Action OnEnemyTurn;

	#region Fields
	[Header("Random")]
    [SerializeField] private int seed;
	[Header("Turn")]
    [SerializeField] private float secondsPlayerTurn = 30;
	[Header("Debug")]
    [SerializeField, ReadOnly] private bool isGameStarted;
    [SerializeField, ReadOnly] private bool isGameOverWin;
	[SerializeField, ReadOnly] private bool isGameOverDefeat;
    [SerializeField, ReadOnly] private bool isGamePaused;
	[SerializeField, ReadOnly] private bool isPlayerTurn;
	[SerializeField, ReadOnly] private int currentTurnScore;
	[SerializeField, ReadOnly] private int currentTurn;
	[SerializeField, ReadOnly] private float turnTime;
	#endregion

	#region Properties
	public static bool IsGameOver => Instance.isGameOverWin || Instance.isGameOverDefeat;
    public static bool IsGameOverWin => Instance.isGameOverWin;
	public static bool IsGameOverDefeat => Instance.isGameOverDefeat;
	public static bool IsGamePaused => Instance.isGamePaused;
	public static bool IsGameStarted => Instance.isGameStarted;
	public static int CurrentTurn => Instance.currentTurn;
	public static bool IsPlayerTurn => Instance.isPlayerTurn;
	public static int TurnScore => Instance.currentTurnScore;
	public static float MaxTurnTime => Instance.secondsPlayerTurn;
	public static float TurnTime => Instance.turnTime;
	#endregion

	#region Unity Messages

	protected override void Awake()
    {
		base.Awake();
        Random.InitState(seed);

		currentTurn = 0;
		isPlayerTurn = true;
		turnTime = secondsPlayerTurn;

		// unpause game on scene init
		Time.timeScale = 1f;
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			TogglePause();

		if (isPlayerTurn)
		{
			// allows match 3

			turnTime -= Time.deltaTime;
			if (turnTime < 0)
			{
				// player does damage
				OnPlayerTurnEnded?.Invoke();

				isPlayerTurn = false;

				// enemy does damage
				OnEnemyTurn?.Invoke();

				turnTime = secondsPlayerTurn;
				currentTurn++;

				isPlayerTurn = true;
			}
		}
	}
    #endregion

    #region Public Methods
	public static void StartGame()
	{
		Instance.isGameStarted = true;
		NextLevel();
	}

	public static void Win()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = true;
		Instance.isGameOverDefeat = false;
	}

	public static void Defeat()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;
	}

    public static void Retry()
	{
		SceneTransition.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void TogglePause()
	{
		Instance.isGamePaused = !Instance.isGamePaused;
		Time.timeScale = Instance.isGamePaused ? 0f : 1f;
	}

	public static void NextLevel()
	{
		SceneTransition.TransitionToNextLevel();
	}

	public static void AddScore(int score)
	{
		Instance.currentTurnScore += score;
	}

	public static void ResetScore()
	{
		Instance.currentTurnScore = 0;
	}
	#endregion

	#region Private Methods
	#endregion
}
