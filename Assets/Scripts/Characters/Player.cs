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

		GameManager.OnPlayerTurnEnded += CalculateDamage;
	}

	private void OnDestroy()
	{
		GameManager.OnPlayerTurnEnded -= CalculateDamage;
	}

	protected override void OnTimeDebuff()
	{
		GameManager.SetTimeDebuff(true);
	}

	protected override void OnDmgDebuff()
	{
		hasDmgDebuff = true;
		GameManager.SetPlayerDmgDebuff(damageReductionWithDebuff);
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
