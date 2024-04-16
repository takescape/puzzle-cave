using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[Header("Player: Debug")]
	[SerializeField, ReadOnly] private PieceData[] calculatedDamages;
	[SerializeField, ReadOnly] private float testCounter;

	protected override void Awake()
	{
		base.Awake();

		TurnManager.OnPlayerTurnEnded += CalculateDamage;
	}

	private void OnDestroy()
	{
		TurnManager.OnPlayerTurnEnded -= CalculateDamage;
	}

	protected override void OnTimeDebuff()
	{
		TurnManager.SetTimeDebuff(true);
	}

	protected override void OnDmgDebuff()
	{
		hasDmgDebuff = true;
		TurnManager.SetPlayerDmgDebuff(damageReductionWithDebuff);
	}

	private void CalculateDamage()
	{
		if (IsAlive == false)
			return;

		calculatedDamages = TurnManager.CurrentTurnDamages;
		for (int i = 0; i < calculatedDamages.Length; i++)
		{
			DamageTarget(calculatedDamages[i].Damage, calculatedDamages[i].DamageOn);
		}

		TurnManager.ResetScore();
	}
}
