using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	[System.Serializable]
	public struct DamagePerHealth
	{
		[MinMaxSlider(0, 100)] public Vector2Int DamageRange;
		public HealthType HealthType;
	}

	[Header("Enemy")]
	[SerializeField] private List<DamagePerHealth> damagesPerHealth = new List<DamagePerHealth>();

	protected override void Awake()
	{
		base.Awake();

		GameManager.OnEnemyTurn += DealDamage;
	}

	private void OnDestroy()
	{
		GameManager.OnEnemyTurn -= DealDamage;
	}

	protected override void OnTimeDebuff()
	{
		GameManager.SetTimeDebuff(false);
	}

	private void DealDamage()
	{
		if (IsAlive == false)
			return;

		int randomHealthsToDmg = Random.Range(0, 4);
		for (int i = 0; i <= randomHealthsToDmg; i++)
		{
			HealthType randomHealth = (HealthType)i;
			Vector2Int randomRange = damagesPerHealth.Find(x => x.HealthType == randomHealth).DamageRange;
			int randomDmg = Random.Range(randomRange.x, randomRange.y+1);

			int actualDmg = hasDmgDebuff ? Mathf.RoundToInt(randomDmg * damageReductionWithDebuff) : randomDmg;

			DamageTarget(actualDmg, randomHealth);
		}
	}
}
