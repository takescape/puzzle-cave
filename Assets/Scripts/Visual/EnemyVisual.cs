using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float playAttackSecondsBeforeTurnEnd = 1f;
	[Header("References")]
	[SerializeField] private Enemy enemy;
	[SerializeField] private Animator animator;
	[SerializeField, AnimatorParam("animator")] private int damageParam;
	[SerializeField, AnimatorParam("animator")] private int deathParam;
	[SerializeField, AnimatorParam("animator")] private int attackParam;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool isAttacking;

	private void Awake()
	{
		animator.SetBool(deathParam, false);
		isAttacking = false;

		TurnManager.OnEnemyTurnEnded += EnableAttack;
		enemy.Health.OnHealthEnded += PlayDeathAnimation;
		enemy.Health.OnAnyDamage += TakeDamage;
	}

	private void OnDestroy()
	{
		TurnManager.OnEnemyTurnEnded -= EnableAttack;
		enemy.Health.OnHealthEnded -= PlayDeathAnimation;
		enemy.Health.OnAnyDamage -= TakeDamage;
	}

	private void Update()
	{
		if (!TurnManager.IsPlayerTurn
			&& TurnManager.TurnTime <= playAttackSecondsBeforeTurnEnd
			&& TurnManager.CurrentTurnDamages.Any(x => x.Damage > 0))
		{
			PlayAttackAnimation();
			isAttacking = true;
		}
	}

	private void PlayDeathAnimation()
	{
		animator.SetBool(deathParam, true);
	}

	private void PlayAttackAnimation()
	{
		if (isAttacking) return;
		animator.SetTrigger(attackParam);
	}

	private void EnableAttack()
	{
		isAttacking = false;
	}

	private void TakeDamage()
	{
		animator.SetTrigger(damageParam);
	}
}
