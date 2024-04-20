using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissVfx : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool isPlayer;
	[Header("References")]
	[SerializeField] private Animator animator;

	private void Awake()
	{
		Character.OnAnyMissedTarget += ShowVfx;
	}

	private void OnDestroy()
	{
		Character.OnAnyMissedTarget -= ShowVfx;
	}

	private void ShowVfx(Character character)
	{
		if ((character.IsPlayer && !isPlayer) || (!character.IsPlayer && isPlayer))
			animator.SetTrigger("Popup");
	}
}
