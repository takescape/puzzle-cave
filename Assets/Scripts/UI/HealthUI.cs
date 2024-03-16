using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Health health;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Slider slider;
	[SerializeField, ReadOnly] private TMP_Text healthText;

	private void Awake()
	{
		slider = GetComponent<Slider>();
		healthText = GetComponentInChildren<TMP_Text>();
	}

	private void Start()
	{
		slider.maxValue = health.MaxHealth;
		slider.minValue = 0;
		healthText.text = $"0 / {health.MaxHealth}";
	}

	private void LateUpdate()
	{
		slider.value = health.CurrentHealth;
		healthText.text = $"{health.CurrentHealth} / {health.MaxHealth}";
	}
}
