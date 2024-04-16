using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
	#region Fields
	[Header("Random")]
    [SerializeField] private int seed;
	[Header("Level Scenes")]
	[SerializeField, Tooltip("Build index for first level scene, used for automatic loadings")] private int firstLevelBuildIndex = 2;
	[Header("Debug")]
    [SerializeField, ReadOnly] private bool isGameStarted;
    [SerializeField, ReadOnly] private bool isGameOverWin;
	[SerializeField, ReadOnly] private bool isGameOverDefeat;
    [SerializeField, ReadOnly] private bool isGamePaused;
	[SerializeField, ReadOnly] private int currentLevel;

	public const string CurrentLevelKey = "CurrentLevel";
	#endregion

	#region Properties
	public static bool IsGameOver => Instance.isGameOverWin || Instance.isGameOverDefeat;
    public static bool IsGameOverWin => Instance.isGameOverWin;
	public static bool IsGameOverDefeat => Instance.isGameOverDefeat;
	public static bool IsGamePaused => Instance.isGamePaused;
	public static bool IsGameStarted => Instance.isGameStarted;
	public static int FirstLevelBuildIndex => Instance.firstLevelBuildIndex;
	public static int CurrentLevel
	{
		get => Instance.currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 0);
		set => PlayerPrefs.SetInt(CurrentLevelKey, value);
	}
	#endregion

	#region Unity Messages

	protected override void Awake()
    {
		base.Awake();
        Random.InitState(seed);

		ResetPause();

		SceneTransition.OnSceneChanged += ResetPause;
    }

	private void OnDestroy()
	{
		SceneTransition.OnSceneChanged -= ResetPause;
	}
    #endregion

    #region Public Methods
	public static void StartGame()
	{
		Instance.isGameStarted = true;
		NextLevel();
	}

	public static void Win()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = true;
		Instance.isGameOverDefeat = false;

		CurrentLevel++;
	}

	public static void Defeat()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;
	}

	public static void GoToMainMenu()
	{
		SceneTransition.TransitionToScene(0);
	}

	public static void ReloadScene()
	{
		SceneTransition.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void TogglePause()
	{
		Instance.isGamePaused = !Instance.isGamePaused;
		Time.timeScale = Instance.isGamePaused ? 0f : 1f;
	}

	public static void NextLevel()
	{
		SceneTransition.TransitionToNextLevel();
	}

	public static void GetMuteSettingsFromSave()
	{
		Instance.SetMusicMute(AudioManager.Instance.IsMusicMuted);
		Instance.SetSfxMuted(AudioManager.Instance.IsSfxMuted);
	}

	public static void ToggleMusic()
	{
		AudioManager.Instance.IsMusicMuted = !AudioManager.Instance.IsMusicMuted;
		Instance.SetMusicMute(AudioManager.Instance.IsMusicMuted);
	}

	public static void ToggleSfx()
	{
		AudioManager.Instance.IsSfxMuted = !AudioManager.Instance.IsSfxMuted;
		Instance.SetSfxMuted(AudioManager.Instance.IsSfxMuted);
	}
	#endregion

	#region Private Methods
	private void ResetPause()
	{
		if (isGamePaused)
			TogglePause();
	}

	private void SetMusicMute(bool isMuted)
	{
		if (isMuted)
		{
			AudioManager.Instance.ChangeTrackVolume(1, 0f);
			AudioManager.Instance.ChangeTrackVolume(2, 0f);
		}
		else
		{
			// hard coded volume because this is just for muting,
			// the actual volume is setted up on AudioMixer asset
			AudioManager.Instance.ChangeTrackVolume(1, .5f);
			AudioManager.Instance.ChangeTrackVolume(2, .5f);
		}

		AudioManager.Instance.IsMusicMuted = isMuted;
	}

	private void SetSfxMuted(bool isMuted)
	{
		if (isMuted)
		{
			for (int i = 1; i < AudioManager.Instance.Tracks.Length + 1; i++)
			{
				if (i <= 2) continue;
				AudioManager.Instance.ChangeTrackVolume(i, 0f);
			}
		}
		else
		{
			for (int i = 1; i < AudioManager.Instance.Tracks.Length + 1; i++)
			{
				if (i <= 2) continue;
				AudioManager.Instance.ChangeTrackVolume(i, .5f);
			}
		}

		AudioManager.Instance.IsSfxMuted = isMuted;
	}

	[Button]
	private void IncreaseCurrentLevelTest()
	{
		CurrentLevel++;
	}

	[Button]
	private void DecreaseCurrentLevelTest()
	{
		CurrentLevel--;
	}
	#endregion
}
