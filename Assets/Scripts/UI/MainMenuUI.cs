using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject continuePanel;
    [Header("Debug")]
    [SerializeField, ReadOnly] private bool hasSave;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        UpdatePanelsVisual();
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
    #endregion

    #region Private Methods
    private void UpdatePanelsVisual()
    {
        newGamePanel.SetActive(!hasSave);
        continuePanel.SetActive(hasSave);
    }

    [Button(enabledMode:EButtonEnableMode.Playmode)]
    private void ToggleHasSaveToTest()
    {
        hasSave = !hasSave;
        UpdatePanelsVisual();
    }
    #endregion
}
