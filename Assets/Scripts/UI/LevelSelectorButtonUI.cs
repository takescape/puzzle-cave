using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorButtonUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private TMP_Text buttonText;
	[SerializeField] private TMP_Text completedText;
	[SerializeField] private Button button;
	[Header("Debug")]
	[SerializeField, ReadOnly] private int levelIndex;

	private void Awake()
	{
		completedText.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener(GoToLevel);
	}

	public void SetupButton(int levelIndex)
	{
		this.levelIndex = levelIndex;
		button.onClick.AddListener(GoToLevel);

		buttonText.text = $"Level {levelIndex + 1}";
		completedText.gameObject.SetActive(GameManager.CurrentLevel > levelIndex);
	}

	private void GoToLevel()
	{
		int actualIndex = levelIndex + GameManager.FirstLevelBuildIndex;
		SceneTransition.TransitionToScene(actualIndex);
	}
}
