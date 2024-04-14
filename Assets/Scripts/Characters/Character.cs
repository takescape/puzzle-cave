using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
	public event Action<Character> OnMissedTarget;

	#region Fields
	[Header("Settings")]
	[SerializeField, Range(0, 1)] protected float percentageToHitWithDebuff = .5f;
	[SerializeField, Range(0, 1)] protected float damageReductionWithDebuff = .5f;
	[Header("References")]
	[SerializeField] protected Character target;
	[Header("Debug")]
	[SerializeField, ReadOnly] protected Health health;
	[SerializeField, ReadOnly] protected bool isAlive;
	[SerializeField, ReadOnly] protected bool hasMissDebuff;
	[SerializeField, ReadOnly] protected bool hasDmgDebuff;
	#endregion

	public Health Health => health;
	public bool IsAlive => isAlive = health.DefaultCurrentHealth > 0;

	#region Unity Messages
	protected virtual void Awake()
	{
		Assert.IsNotNull(target);
		health = GetComponent<Health>();

		hasMissDebuff = false;
		hasDmgDebuff = false;

		health.OnMissHealthBreak += OnMissDebuff;
		health.OnDmgHealthBreak += OnDmgDebuff;
		health.OnTimeHealthBreak += OnTimeDebuff;
	}

	private void OnDestroy()
	{
		health.OnMissHealthBreak -= OnMissDebuff;
		health.OnDmgHealthBreak -= OnDmgDebuff;
		health.OnTimeHealthBreak -= OnTimeDebuff;
	}
	#endregion

	#region Public Methods
	[Button]
	public void TestAttackTarget()
	{
		// hardcoded damage to test
		DamageTarget(2, HealthType.White);
	}
	#endregion

	#region Protected Methods
	protected virtual void DamageTarget(int damage, HealthType type)
	{
		if (target == null)
			throw new NullReferenceException($"{name} does not defines a target.");

		if (hasMissDebuff)
		{
			if (Random.Range(0f, 1f) < percentageToHitWithDebuff)
				target.Health.TakeDamage(damage, type);
			else
				OnMissedTarget?.Invoke(this);
		}
		else
			target.Health.TakeDamage(damage, type);
		//Debug.Log($"{name} damaged {target.name} for {damage} damage");
	}

	protected void OnMissDebuff() => hasMissDebuff = true;
	protected virtual void OnDmgDebuff() => hasDmgDebuff = true;
	protected virtual void OnTimeDebuff() { }
	#endregion
}
