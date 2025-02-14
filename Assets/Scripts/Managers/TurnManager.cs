using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
	public static event Action OnPlayerTurnEnded;
	public static event Action OnEnemyTurnEnded;

	#region Fields
	[Header("Turn")]
	[SerializeField] private float secondsPlayerTurn = 10;
	[SerializeField] private float secondsEnemyTurn = 5;
	[SerializeField] private float secondsToAddOnBuff = 5;
	[SerializeField] private float secondsToRemoveOnDebuff = 5;
	[Header("Damage Setup")]
	[SerializeField] private List<HealthType> healthTypes = new List<HealthType>();
	[Header("Audio")]
	//[SerializeField] private string turnEndSound = "ring";
	[SerializeField] private string turnEnemyClockSound = "clock";
	[SerializeField] private float delayToStartClock = .8f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool isPlayerTurn;
	[SerializeField, ReadOnly] private PieceData[] currentTurnDamages = new PieceData[4];
	[SerializeField, ReadOnly] private int currentTurn;
	[SerializeField, ReadOnly] private float turnTime;
	[SerializeField, ReadOnly] private bool hasPlayerTimeBuff;
	[SerializeField, ReadOnly] private bool hasEnemyTimeDebuff;
	[SerializeField, ReadOnly] private bool hasPlayerDmgDebuff;
	[SerializeField, ReadOnly] private float playerDmgReduction;
	[SerializeField, ReadOnly] private bool hasTurnStarted;
	#endregion

	#region Properties
	public static int CurrentTurn => Instance.currentTurn;
	public static bool IsPlayerTurn => Instance.isPlayerTurn;
	public static bool HasTurnStarted => Instance.hasTurnStarted;
	public static PieceData[] CurrentTurnDamages => Instance.currentTurnDamages;
	public static List<HealthType> HealthTypes => Instance.healthTypes;
	public static float PlayerTime
	{
		get
		{
			float time = Instance.secondsPlayerTurn;
			if (Instance.hasPlayerTimeBuff)
				time += Instance.secondsToAddOnBuff;
			if (Instance.hasEnemyTimeDebuff)
				time -= Instance.secondsToRemoveOnDebuff;

			return time;
		}
	}
	public static float MaxTurnTime => IsPlayerTurn ? PlayerTime : Instance.secondsEnemyTurn;
	public static float TurnTime => Instance.turnTime;
	#endregion

	#region Unity Messages
	protected override void Awake()
	{
		base.Awake();

		hasTurnStarted = false;
		currentTurn = 0;
		isPlayerTurn = true;
		turnTime = secondsPlayerTurn;
		hasPlayerTimeBuff = false;
		hasEnemyTimeDebuff = false;
		SetupDamages();
		GameManager.StopCoroutines();

		MoviePieces.OnInput += StartTurn;
		GameManager.OnGameOver += StopTurnTrack;
	}

	private void OnDestroy()
	{
		MoviePieces.OnInput -= StartTurn;
		GameManager.OnGameOver -= StopTurnTrack;
	}

	private void Update()
	{
		if (!hasTurnStarted)
			return;

		if (GameManager.IsGameOver || GameManager.IsGamePaused)
		{
			StopTurnTrack(false);
			return;
		}

		if (isPlayerTurn)
		{
			turnTime -= Time.deltaTime;
			if (turnTime < 0)
			{
				// player does damage
				OnPlayerTurnEnded?.Invoke();
				GameManager.DoAfterSeconds(delayToStartClock, () => AudioManager.Instance.PlaySound(turnEnemyClockSound, 6));

				turnTime = secondsEnemyTurn;
				currentTurn++;

				isPlayerTurn = false;
			}
		}
		else
		{
			turnTime -= Time.deltaTime;
			if (turnTime < 0)
			{
				// enemy does damage
				OnEnemyTurnEnded?.Invoke();
				StopTurnTrack(false);

				turnTime = PlayerTime;
				currentTurn++;

				isPlayerTurn = true;
			}
		}
	}
	#endregion

	#region Public Methods
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
				return Instance.hasPlayerDmgDebuff ? Mathf.RoundToInt(Instance.currentTurnDamages[i].Damage * Instance.playerDmgReduction) : Instance.currentTurnDamages[i].Damage;
		}

		return 0;
	}

	public static void ResetScore()
	{
		Instance.SetupDamages();
	}

	public static void SetTimeDebuff(bool isPlayer)
	{
		if (isPlayer)
			Instance.hasEnemyTimeDebuff = true;
		else
			Instance.hasPlayerTimeBuff = true;
	}

	public static void SetPlayerDmgDebuff(float damageReduction)
	{
		Instance.hasPlayerDmgDebuff = true;
		Instance.playerDmgReduction = damageReduction;
	}
	#endregion

	#region Private Methods
	private void SetupDamages()
	{
		currentTurnDamages = new PieceData[healthTypes.Count];
		for (int i = 0; i < healthTypes.Count; i++)
		{
			PieceData dmg = new PieceData();
			dmg.Damage = 0;
			dmg.DamageOn = healthTypes[i];
			currentTurnDamages[i] = dmg;
		}
	}

	private void StartTurn()
	{
		hasTurnStarted = true;
	}

	private void StopTurnTrack(bool win)
	{
		GameManager.StopCoroutines();
		AudioManager.Instance.StopTrack(6);
	}
	#endregion
}
