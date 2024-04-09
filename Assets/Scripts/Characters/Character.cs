using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Character : MonoBehaviour
{
	#region Fields
	[Header("References")]
	[SerializeField] protected Character target;
	[Header("Debug")]
	[SerializeField, ReadOnly] protected Health health;
	[SerializeField, ReadOnly] protected bool isAlive;
	#endregion

	public Health Health => health;
	public bool IsAlive => isAlive = health.DefaultCurrentHealth > 0;

	#region Unity Messages
	protected virtual void Awake()
	{
		Assert.IsNotNull(target);
		health = GetComponent<Health>();
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

		target.Health.TakeDamage(damage, type);
		//Debug.Log($"{name} damaged {target.name} for {damage} damage");
	}
	#endregion
}
