using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	[Header("Player: Debug")]
	[SerializeField, ReadOnly] private PieceData[] calculatedDamages;

	protected override void Awake()
	{
		base.Awake();

		TurnManager.OnPlayerTurnEnded += CalculateDamage;
		Health.OnHealthEnded += LoseLevel;
	}

	private void OnDestroy()
	{
		TurnManager.OnPlayerTurnEnded -= CalculateDamage;
		Health.OnHealthEnded -= LoseLevel;
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

	private void LoseLevel()
	{
		GameManager.Defeat();
	}
}
