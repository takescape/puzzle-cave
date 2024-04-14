using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimeUI : MonoBehaviour
{
	[SerializeField] private Slider slider;
	[SerializeField] private TMP_Text timeText;

	private void Start()
	{
		slider.minValue = 0;
		slider.maxValue = GameManager.MaxTurnTime;
		timeText.text = GameManager.MaxTurnTime.ToString("0.0") + "s";
	}

	private void LateUpdate()
	{
		slider.minValue = 0;
		slider.maxValue = GameManager.MaxTurnTime;
		slider.value = GameManager.TurnTime;
		timeText.text = GameManager.TurnTime.ToString("0.0") + "s";
	}
}
