using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public static event Action OnAnyHealthDamage;
	public event Action OnAnyDamage;
	public event Action OnMissHealthBreak;
	public event Action OnDmgHealthBreak;
	public event Action OnTimeHealthBreak;
	public event Action OnHealthEnded;

	[Header("Healths")]
	[SerializeField] private HealthInstance[] healths;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool isInvincible = false;

	public float DefaultMaxHealth => GetHealth(HealthType.White).MaxHealth;
	public float DefaultCurrentHealth => GetHealth(HealthType.White).CurrentHealth;
	public bool IsInvincible => isInvincible;

	private void Awake()
	{
		foreach (var health in healths)
			health.Initialize(this);
	}

	public void TakeDamage(float damage, HealthType type)
	{
		GetHealth(type)?.TakeDamage(damage);
	}

	public void Heal(float heal, HealthType type)
	{
		GetHealth(type)?.Heal(heal);
	}

	public void ToggleInvincible()
	{
		isInvincible = !isInvincible;
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

	public void RaiseMissDebuffEvent() => OnMissHealthBreak?.Invoke();
	public void RaiseDmgDebuffEvent() => OnDmgHealthBreak?.Invoke();
	public void RaiseTimeDebuffEvent() => OnTimeHealthBreak?.Invoke();
	public void RaiseHealthEndedEvent() => OnHealthEnded?.Invoke();
	public void RaiseDamageEvent()
	{
		OnAnyDamage?.Invoke();
		OnAnyHealthDamage?.Invoke();
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
	private Health parentHealth;

	public HealthType Type => healthType;
	public float MaxHealth => maxHealth;
	public float CurrentHealth
	{
		get { SanitizeHealth(); return currentHealth; }
		set { currentHealth = value; SanitizeHealth(); }
	}

	public void Initialize(Health health)
	{
		CurrentHealth = maxHealth;
		parentHealth = health;
	}

	public void TakeDamage(float damage)
	{
		if (damage > 0)
			parentHealth.RaiseDamageEvent();

		if (parentHealth.IsInvincible == false)
			CurrentHealth -= Mathf.Abs(damage);
		if (CurrentHealth <= 0)
		{
			switch (Type)
			{
				case HealthType.White:
					parentHealth.RaiseHealthEndedEvent();
					break;
				case HealthType.Red:
					parentHealth.RaiseDmgDebuffEvent();
					break;
				case HealthType.Blue:
					parentHealth.RaiseTimeDebuffEvent();
					break;
				case HealthType.Purple:
					parentHealth.RaiseMissDebuffEvent();
					break;
			}
		}
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