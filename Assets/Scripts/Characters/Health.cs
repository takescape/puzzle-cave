using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private HealthInstance[] healths;

	public float DefaultMaxHealth => GetHealth(HealthType.White).MaxHealth;
	public float DefaultCurrentHealth => GetHealth(HealthType.White).CurrentHealth;

	private void Awake()
	{
		foreach (var health in healths)
			health.Initialize();
	}

	public void TakeDamage(float damage, HealthType type)
	{
		GetHealth(type)?.TakeDamage(damage);
	}

	public void Heal(float heal, HealthType type)
	{
		GetHealth(type)?.Heal(heal);
	}

	public HealthInstance GetHealth(HealthType type)
	{
		foreach (var health in healths)
		{
			if (health.Type != type)
				continue;
			else
				return health;
		}

		return null;
	}

	[Button]
	private void TakeDamageTest() => TakeDamage(1, HealthType.White);
	[Button]
	private void HealTest() => Heal(1, HealthType.White);
}

public enum HealthType { White, Red, Blue, Purple }

[System.Serializable]
public class HealthInstance
{
	[SerializeField] private HealthType healthType;
	[SerializeField] private float maxHealth;
	private float currentHealth;

	public HealthType Type => healthType;
	public float MaxHealth => maxHealth;
	public float CurrentHealth
	{
		get { SanitizeHealth(); return currentHealth; }
		set { currentHealth = value; SanitizeHealth(); }
	}

	public void Initialize()
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
}