using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[Header("Player: Debug")]
	[SerializeField, ReadOnly] private Piece[] calculatedDamages;
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

			//testCounter += Time.deltaTime;
			//if (testCounter > 1) // increasing turn score every second
			//{
			//	GameManager.AddScore(1);
			//	testCounter = 0;
			//}
		}
	}

	private void CalculateDamage()
	{
		if (IsAlive == false)
			return;

		calculatedDamages = GameManager.CurrentTurnDamages;
		for (int i = 0; i < calculatedDamages.Length; i++)
		{
			DamageTarget(calculatedDamages[i].Damage, calculatedDamages[i].DamageOn);
		}

		GameManager.ResetScore();
	}
}
