using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[Header("Player: Debug")]
	[SerializeField, ReadOnly] private int calculatedDamage;
	[SerializeField, ReadOnly] private float testCounter;

	protected override void Awake()
	{
		base.Awake();

		GameManager.OnPlayerTurnEnded += CalculateDamage;
	}

	private void OnDestroy()
	{
		GameManager.OnPlayerTurnEnded -= CalculateDamage;
	}

	private void Update()
	{
		if (GameManager.IsPlayerTurn)
		{
			// this is hardcoded for testing
			// this score should increase with match 3

			testCounter += Time.deltaTime;
			if (testCounter > 1) // increasing turn score every second
			{
				GameManager.AddScore(1);
				testCounter = 0;
			}
		}
	}

	private void CalculateDamage()
	{
		if (IsAlive == false)
			return;

		calculatedDamage = GameManager.TurnScore;
		DamageTarget(calculatedDamage);

		GameManager.ResetScore();
	}
}
