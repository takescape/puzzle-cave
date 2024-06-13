using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string tutorialKey;
	[Header("References")]
	[SerializeField] private GameObject bubble;
	[SerializeField] private GameObject hand;

	private string Key => "Tutorial." + tutorialKey;

	private bool HasShowedTutorial
	{
		get => PlayerPrefs.GetInt(Key, 0) == 1;
		set => PlayerPrefs.SetInt(Key, value ? 1 : 0);
	}

	private void Awake()
	{
		if (HasShowedTutorial)
		{
			bubble.SetActive(false);
			hand.SetActive(true);
		}

		MoviePieces.OnInput += HideTutorial;
	}

	private void OnDestroy()
	{
		MoviePieces.OnInput -= HideTutorial;
	}

	private void HideTutorial()
	{
		HasShowedTutorial = true;
		gameObject.SetActive(false);
	}
}
