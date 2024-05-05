using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerUI : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float cutTimeSeconds = 1f;
	[Header("References")]
	[SerializeField] private MainMenuUI uiMenu;
	[SerializeField] private LevelSelectorUI uiLevelSelector;
	[SerializeField] private CinemachineVirtualCamera vcamBeforeCut;
	[SerializeField] private CinemachineVirtualCamera vcamCut;
	[SerializeField] private GameObject menuNormal;
	[SerializeField] private GameObject menuLevel;

	public void ShowMenu()
	{
		vcamBeforeCut.m_Priority = 9;
		vcamCut.m_Priority = 9;

		uiMenu.gameObject.SetActive(true);
		uiLevelSelector.gameObject.SetActive(false);
	}

	public void ShowLevelSelector()
	{
		vcamBeforeCut.m_Priority = 11;
		StartCoroutine(WaitCutToLevelSelector());
	}
	
	private IEnumerator WaitCutToLevelSelector()
	{
		yield return new WaitForSeconds(cutTimeSeconds);

		vcamCut.m_Priority = 12;

		uiMenu.gameObject.SetActive(false);
		menuNormal.SetActive(false);

		uiLevelSelector.gameObject.SetActive(true);
		menuLevel.SetActive(true);
	}
}
