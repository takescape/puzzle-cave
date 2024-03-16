using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	[Header("Enemy")]
	[SerializeField] private int damage = 2;

	protected override void Awake()
	{
		base.Awake();

		GameManager.OnEnemyTurn += DealDamage;
	}

	private void OnDestroy()
	{
		GameManager.OnEnemyTurn -= DealDamage;
	}

	private void DealDamage()
	{
		if (IsAlive == false)
			return;

		DamageTarget(damage);
	}
}
