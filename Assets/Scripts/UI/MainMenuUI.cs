using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    #region Fields
	[Header("References")]
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject settingsPanel;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        ShowMainPanel();
    }
    #endregion

    #region Public Methods
    // method used in unity event for main menu UI play buttons
    public void StartGame()
    {
        GameManager.StartGame();
    }

	// method used in unity event for main menu UI play buttons
	public void ShowMainPanel()
    {
		mainPanel.SetActive(true);
		settingsPanel.SetActive(false);
	}

	// method used in unity event for main menu UI play buttons
	public void ShowOptionsPanel()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}
    #endregion

    #region Private Methods
    #endregion
}
