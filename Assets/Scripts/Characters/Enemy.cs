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
	[SerializeField, Tooltip("Used for the time mock/visual damage calculation")] private float timeDelayPerDamage = .5f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private PieceData[] calculatedDamages;

	protected override void Awake()
	{
		base.Awake();

		TurnManager.OnPlayerTurnEnded += CalculateMockDamage;
		TurnManager.OnEnemyTurn += DealDamage;
		Health.OnHealthEnded += WinLevel;
	}

	private void OnDestroy()
	{
		TurnManager.OnPlayerTurnEnded -= CalculateMockDamage;
		TurnManager.OnEnemyTurn -= DealDamage;
		Health.OnHealthEnded -= WinLevel;
	}

	protected override void OnTimeDebuff()
	{
		TurnManager.SetTimeDebuff(false);
	}

	private void CalculateMockDamage()
	{
		if (IsAlive == false)
			return;

		int randomHealthsToDmg = Random.Range(1, 5);
		StartCoroutine(CalculateMockDamageCoroutine(randomHealthsToDmg));
	}

	private IEnumerator CalculateMockDamageCoroutine(int randomHealthsToDmg)
	{
		if (timeDelayPerDamage * randomHealthsToDmg > TurnManager.MaxTurnTime)
			Debug.LogError("Enemy mock damage calculation per time will probably cause bugs. The delay is greater than the turn time.");

		for (int i = 0; i < randomHealthsToDmg; i++)
		{
			yield return new WaitForSeconds(timeDelayPerDamage);

			HealthType randomHealth = (HealthType)Random.Range(0, 4);
			Vector2Int randomRange = damagesPerHealth.Find(x => x.HealthType == randomHealth).DamageRange;
			int randomDmg = Random.Range(randomRange.x, randomRange.y + 1);

			int actualDmg = hasDmgDebuff ? Mathf.RoundToInt(randomDmg * damageReductionWithDebuff) : randomDmg;
			TurnManager.AddDamage(actualDmg, randomHealth);
		}
	}

	private void DealDamage()
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

	private void WinLevel()
	{
		GameManager.Win();
	}
}
