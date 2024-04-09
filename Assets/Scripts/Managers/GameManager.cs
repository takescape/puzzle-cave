using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
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
	[SerializeField, ReadOnly] private PieceData[] currentTurnDamages = new PieceData[4];
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
	public static PieceData[] CurrentTurnDamages => Instance.currentTurnDamages;
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
		SetupDamages();

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

	public static void AddDamage(int dmg, HealthType type)
	{
		for (int i = 0; i < Instance.currentTurnDamages.Length; i++)
		{
			if (Instance.currentTurnDamages[i].DamageOn != type)
				continue;

			PieceData dmgTemp = Instance.currentTurnDamages[i];
			dmgTemp.Damage += dmg;
			Instance.currentTurnDamages[i] = dmgTemp;
		}
	}

	public static int GetCurrentDamage(HealthType type)
	{
		for (int i = 0; i < Instance.currentTurnDamages.Length; i++)
		{
			if (Instance.currentTurnDamages[i].DamageOn == type)
				return Instance.currentTurnDamages[i].Damage;
		}

		return 0;
	}

	public static void ResetScore()
	{
		Instance.SetupDamages();
	}
	#endregion

	#region Private Methods
	private void SetupDamages()
	{
		currentTurnDamages = new PieceData[4];
		for (int i = 0; i < currentTurnDamages.Length; i++)
		{
			PieceData dmg = new PieceData();
			dmg.Damage = 0;
			dmg.DamageOn = HealthType.White + i;
			currentTurnDamages[i] = dmg;
		}
	}
	#endregion
}
