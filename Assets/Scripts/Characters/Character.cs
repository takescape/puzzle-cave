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
	[SerializeField] private Character target;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Health health;
	#endregion

	public Health Health => health;

	#region Unity Messages
	private void Awake()
	{
		Assert.IsNotNull(target);
		health = GetComponent<Health>();
	}
	#endregion

	#region Public Methods
	[Button]
	public void AttackTarget()
	{
		// hardcoded damage to test
		DamageTarget(2);
	}
	#endregion

	#region Private Methods
	private void DamageTarget(int damage)
	{
		if (target == null)
			throw new NullReferenceException($"{name} does not defines a target.");

		target.Health.TakeDamage(damage);
		Debug.Log($"{name} damaged {target.name} for {damage} damage");
	}
	#endregion
}
