using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private MainMenuUI uiMenu;
	[SerializeField] private LevelSelectorUI uiLevelSelector;
	[SerializeField] private CinemachineVirtualCamera vcamCut;
	[SerializeField] private GameObject menuNormal;
	[SerializeField] private GameObject menuLevel;

	public void ShowMenu()
	{
		SceneTransition.TransitionWithoutSceneCallback(WaitCutToMenu);
	}

	public void ShowLevelSelector()
	{
		SceneTransition.TransitionWithoutSceneCallback(WaitCutToLevelSelector);
	}
	
	private void WaitCutToMenu()
	{
		vcamCut.m_Priority = 9;

		uiMenu.gameObject.SetActive(true);
		menuNormal.SetActive(true);

		uiLevelSelector.gameObject.SetActive(false);
		menuLevel.SetActive(false);
	}

	private void WaitCutToLevelSelector()
	{
		vcamCut.m_Priority = 12;

		uiMenu.gameObject.SetActive(false);
		menuNormal.SetActive(false);

		uiLevelSelector.gameObject.SetActive(true);
		menuLevel.SetActive(true);
	}
}
