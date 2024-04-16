using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject levelButtonsParent;
	[Header("Debug")]
	[SerializeField, ReadOnly] private LevelSelectorButtonUI[] levelButtons;

	private void Awake()
	{
		levelButtons = levelButtonsParent.GetComponentsInChildren<LevelSelectorButtonUI>();
	}

	private void Start()
	{
		for (int i = 0; i < levelButtons.Length; i++)
			levelButtons[i].SetupButton(i);
	}

	// method used in unity event for main menu UI play buttons
	public void GoToMainMenu()
	{
		GameManager.GoToMainMenu();
	}
}
