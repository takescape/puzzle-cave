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
		slider.maxValue = TurnManager.MaxTurnTime;
		timeText.text = TurnManager.MaxTurnTime.ToString("0.0") + "s";
	}

	private void LateUpdate()
	{
		slider.minValue = 0;
		slider.maxValue = TurnManager.MaxTurnTime;
		slider.value = TurnManager.TurnTime;
		timeText.text = TurnManager.TurnTime.ToString("0.0") + "s";
	}
}
