using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject winPanel;
	[SerializeField] private GameObject defeatPanel;
	[SerializeField] private Image musicMuteIcon;
	[SerializeField] private Image sfxMuteIcon;

	#region Unity Messages
	private void Awake()
	{
		pausePanel.SetActive(false);
		winPanel.SetActive(false);
		defeatPanel.SetActive(false);

		GameManager.GetMuteSettingsFromSave();
		SetMuteIcons();

		GameManager.OnGameDefeat += ShowDefeatPanel;
		GameManager.OnGameWin += ShowWinPanel;
	}

	private void OnDestroy()
	{
		GameManager.OnGameDefeat -= ShowDefeatPanel;
		GameManager.OnGameWin -= ShowWinPanel;
	}
	#endregion

	#region Public Methods
	// method used in unity event for main menu UI play buttons
	public void StartGame()
	{
		GameManager.StartGame();
	}

	// method used in unity event for main menu UI play buttons
	public void ShowPausePanel()
	{
		pausePanel.SetActive(true);
		GameManager.TogglePause();
	}

	// method used in unity event for main menu UI play buttons
	public void HidePausePanel()
	{
		pausePanel.SetActive(false);
		GameManager.TogglePause();
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
	public void GoToMainMenu()
	{
		GameManager.GoToMainMenu();
	}

	// method used in unity event for main menu UI play buttons
	public void RetryLevel()
	{
		GameManager.ReloadScene();
	}

	// method used in unity event for main menu UI play buttons
	public void NextLevel()
	{
		GameManager.LoadNextScene();
	}
	#endregion

	#region Private Methods
	private void SetMuteIcons()
	{
		musicMuteIcon.gameObject.SetActive(AudioManager.Instance.IsMusicMuted);
		sfxMuteIcon.gameObject.SetActive(AudioManager.Instance.IsSfxMuted);
	}

	private void ShowDefeatPanel()
	{
		pausePanel.SetActive(false);
		defeatPanel.SetActive(true);
		winPanel.SetActive(false);
	}

	private void ShowWinPanel()
	{
		pausePanel.SetActive(false);
		defeatPanel.SetActive(false);
		winPanel.SetActive(true);
	}
	#endregion
}
