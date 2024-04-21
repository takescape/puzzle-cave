using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private ParticleSystem damageVfx;
	[Header("Audio")]
	[SerializeField] private string missSound = "swoosh";
	[SerializeField] private string[] damageSounds;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Character parentCharacter;

	private void Awake()
	{
		parentCharacter = GetComponentInParent<Character>();
	}

	private void Start()
	{
		Character.OnAnyMissedTarget += OnMissed;
		parentCharacter.Health.OnAnyDamage += TakeDamage;
	}

	private void OnDestroy()
	{
		Character.OnAnyMissedTarget -= OnMissed;
		parentCharacter.Health.OnAnyDamage -= TakeDamage;
	}

	private void TakeDamage()
	{
		damageVfx.Play();
		AudioManager.Instance.PlaySoundOneShot(damageSounds[Random.Range(0, damageSounds.Length - 1)], 4);
	}

	private void OnMissed(Character character)
	{
		AudioManager.Instance.PlaySoundOneShot(missSound, 4);
	}
}
