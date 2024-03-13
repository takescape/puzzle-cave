using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Health health;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Slider slider;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Start()
	{
		slider.maxValue = health.MaxHealth;
		slider.minValue = 0;
	}

	private void LateUpdate()
	{
		slider.value = health.CurrentHealth;
	}
}
