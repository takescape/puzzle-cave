using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[Header("Debug")]
	[SerializeField, ReadOnly] private float currentHealth;

	public float MaxHealth => maxHealth;
	public float CurrentHealth
	{
		get { SanitizeHealth(); return currentHealth; }
		set { currentHealth = value; SanitizeHealth(); }
	}

	private void Awake()
	{
		CurrentHealth = maxHealth;
	}

	public void TakeDamage(float damage)
	{
		CurrentHealth -= Mathf.Abs(damage);
	}

	public void Heal(float heal)
	{
		CurrentHealth += Mathf.Abs(heal);
	}

	private void SanitizeHealth()
	{
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;

		if (currentHealth < 0)
			currentHealth = 0;
	}

	[Button]
	private void TakeDamageTest() => TakeDamage(1);
	[Button]
	private void HealthTest() => Heal(1);
}
