using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
	public static event Action<Character> OnAnyMissedTarget;

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
	public virtual bool IsPlayer => false;

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
		PieceData[] pieceDatas = new PieceData[1];
		PieceData data = new PieceData();
		data.Damage = 2;
		data.DamageOn = HealthType.White;
		pieceDatas[0] = data;
		DamageTarget(pieceDatas);
	}
	#endregion

	#region Protected Methods
	protected virtual void DamageTarget(PieceData[] pieceDatas)
	{
		if (target == null)
			throw new NullReferenceException($"{name} does not defines a target.");

		void DoDamage()
		{
			for (int i = 0; i < pieceDatas.Length; i++)
			{
				if (pieceDatas[i].Damage <= 0) continue;
				target.Health.TakeDamage(pieceDatas[i].Damage, pieceDatas[i].DamageOn);
			}
		}

		if (hasMissDebuff)
		{
			if (Random.Range(0f, 1f) < percentageToHitWithDebuff)
				DoDamage();
			else
				OnAnyMissedTarget?.Invoke(this);
		}
		else
			DoDamage();
	}

	//protected virtual void DamageTarget(int damage, HealthType type)
	//{
	//	if (target == null)
	//		throw new NullReferenceException($"{name} does not defines a target.");
	//	if (damage <= 0)
	//		return;

	//	if (hasMissDebuff)
	//	{
	//		if (Random.Range(0f, 1f) < percentageToHitWithDebuff)
	//			target.Health.TakeDamage(damage, type);
	//		else
	//			OnAnyMissedTarget?.Invoke(this);
	//	}
	//	else
	//		target.Health.TakeDamage(damage, type);
	//	//Debug.Log($"{name} damaged {target.name} for {damage} damage");
	//}

	protected void OnMissDebuff() => hasMissDebuff = true;
	protected virtual void OnDmgDebuff() => hasDmgDebuff = true;
	protected virtual void OnTimeDebuff() { }
	#endregion
}
