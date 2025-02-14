using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Fields
	[Header("References")]
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private GameObject creditsPanel;
	[SerializeField] private Image musicMuteIcon;
	[SerializeField] private Image sfxMuteIcon;
	#endregion

	#region Unity Messages
	private void Awake()
    {
        ShowMainPanel();
	}

	private void Start()
	{
		GameManager.GetMuteSettingsFromSave();
		SetMuteIcons();
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
		creditsPanel.SetActive(false);
	}

	// method used in unity event for main menu UI play buttons
	public void ShowOptionsPanel()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(true);
		creditsPanel.SetActive(false);
	}

	// method used in unity event for main menu UI play buttons
	public void ShowCreditsPanel()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(false);
		creditsPanel.SetActive(true);
	}

	// method used in unity event for main menu UI play buttons
	public void ToggleMusic()
	{
		GameManager.ToggleMusic();
		SetMuteIcons();
	}

	// method used in unity event for main menu UI play buttons
	public void ToggleSfx()
	{
		GameManager.ToggleSfx();
		SetMuteIcons();
	}

	// method used in unity event for main menu UI play buttons
	public void ResetProgress()
	{
		PlayerPrefs.DeleteAll();
		GameManager.GoToMainMenu();
	}

	// method used in unity event for main menu UI play buttons
	public void BackToMenu()
	{
		GameManager.GoToMainMenu();
	}
	#endregion

	#region Private Methods
	private void SetMuteIcons()
	{
		musicMuteIcon.gameObject.SetActive(AudioManager.Instance.IsMusicMuted);
		sfxMuteIcon.gameObject.SetActive(AudioManager.Instance.IsSfxMuted);
	}
	#endregion
}
