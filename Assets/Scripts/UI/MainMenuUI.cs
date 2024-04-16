using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private string newGameText = "New Game";
    [SerializeField] private string continueText = "Continue";
	[Header("References")]
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private TMP_Text mainButtonText;
	[Header("Debug")]
    [SerializeField, ReadOnly] private bool hasSave;
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
    public void StartNewGame()
    {
        // resets save before starting
        PlayerPrefs.DeleteAll();
        StartGame();
    }

	// method used in unity event for main menu UI play buttons
	public void ShowMainPanel()
    {
		mainPanel.SetActive(true);
		settingsPanel.SetActive(false);

        UpdateMainPanelText();
	}

	// method used in unity event for main menu UI play buttons
	public void ShowOptionsPanel()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}
    #endregion

    #region Private Methods
    private void UpdateMainPanelText()
    {
		mainButtonText.text = hasSave ? continueText : newGameText;
	}

    [Button(enabledMode:EButtonEnableMode.Playmode)]
    private void ToggleHasSaveToTest()
    {
        hasSave = !hasSave;
        UpdateMainPanelText();
    }
    #endregion
}
