using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private HealthType healthType;
	[Header("Debug")]
	[SerializeField, ReadOnly] private HealthUI healthUI;
	[SerializeField, ReadOnly] private Slider slider;
	[SerializeField, ReadOnly] private TMP_Text healthText;

	public HealthInstance Health => healthUI.Health.GetHealth(healthType);

	private void Awake()
	{
		healthUI = GetComponentInParent<HealthUI>();
		slider = GetComponent<Slider>();
		healthText = GetComponentInChildren<TMP_Text>();
	}

	private void Start()
	{
		slider.maxValue = Health.MaxHealth;
		slider.minValue = 0;
		healthText.text = $"0 / {Health.MaxHealth}";
	}

	private void LateUpdate()
	{
		slider.value = Health.CurrentHealth;
		healthText.text = $"{Health.CurrentHealth} / {Health.MaxHealth}";
	}
}
