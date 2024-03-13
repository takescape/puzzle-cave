using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Health health;
	[SerializeField] private Slider slider;

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
