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
	#endregion

	#region Properties
	public static int CurrentTurn => Instance.currentTurn;
	public static bool IsPlayerTurn => Instance.isPlayerTurn;
	public static PieceData[] CurrentTurnDamages => Instance.currentTurnDamages;
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

		currentTurn = 0;
		isPlayerTurn = true;
		turnTime = secondsPlayerTurn;
		hasPlayerTimeBuff = false;
		hasEnemyTimeDebuff = false;
		SetupDamages();
		GameManager.StopCoroutines();
	}

	private void Update()
	{
		if (isPlayerTurn)
		{
			turnTime -= Time.deltaTime;
			if (turnTime < 0)
			{
				// player does damage
				OnPlayerTurnEnded?.Invoke();
				GameManager.DoAfterSeconds(delayToStartClock, () => AudioManager.Instance.PlaySound(turnEnemyClockSound, 6));
				//AudioManager.Instance.PlaySoundOneShot(turnEndSound, 4);

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
				AudioManager.Instance.StopTrack(6);
				//AudioManager.Instance.PlaySoundOneShot(turnEndSound, 4);

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
