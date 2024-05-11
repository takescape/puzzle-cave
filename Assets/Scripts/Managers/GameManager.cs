using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
	public static event Action OnGameDefeat;
	public static event Action OnGameWin;
	public static event Action<bool> OnGameOver; // bool win

	#region Fields
	[Header("Random")]
    [SerializeField] private int seed;
	[Header("Level Scenes")]
	[SerializeField, Tooltip("Build index for first level scene, used for automatic loadings")] private int firstLevelBuildIndex = 2;
	[Header("Audio")]
	[SerializeField] private string winLevelSound = "level_win";
	[SerializeField] private string defeatLevelSound = "level_lose";
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

		ResetGameState();
		SceneTransition.OnSceneChanged += ResetGameState;
    }

	private void OnDestroy()
	{
		SceneTransition.OnSceneChanged -= ResetGameState;
	}
    #endregion

    #region Public Methods
	public static void StartGame()
	{
		Instance.isGameStarted = true;
		LoadNextScene();
	}

	public static void Win()
	{
		AudioManager.Instance.StopTrack(1); // stop music
		AudioManager.Instance.StopTrack(6); // stop turn sfx
		AudioManager.Instance.PlaySoundOneShot(Instance.winLevelSound, 7);

		Instance.isGameOverWin = true;
		Instance.isGameOverDefeat = false;

		CurrentLevel = SceneManager.GetActiveScene().buildIndex - FirstLevelBuildIndex + 1;

		OnGameWin?.Invoke();
		OnGameOver?.Invoke(true);
	}

	public static void Defeat()
	{
		AudioManager.Instance.StopTrack(1); // stop music
		AudioManager.Instance.StopTrack(6); // stop turn sfx
		AudioManager.Instance.PlaySoundOneShot(Instance.defeatLevelSound, 7);

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;

		OnGameDefeat?.Invoke();
		OnGameOver?.Invoke(false);
	}

	public static void GoToMainMenu()
	{
		SceneTransition.TransitionToScene(0);
	}

	public static void ReloadScene()
	{
		SceneTransition.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void LoadNextScene()
	{
		SceneTransition.TransitionToNextLevel();
	}

	public static void TogglePause()
	{
		Instance.isGamePaused = !Instance.isGamePaused;
		Time.timeScale = Instance.isGamePaused ? 0f : 1f;
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

	public static void DoAfterSeconds(float seconds, Action action)
	{
		Instance.StartCoroutine(Instance.DoAfterSecondsCoroutine(seconds, action));
	}

	public static void StopCoroutines()
	{
		Instance.StopAllCoroutines();
	}
	#endregion

	#region Private Methods
	private void ResetGameState()
	{
		if (isGamePaused)
			TogglePause();

		isGameOverDefeat = false;
		isGameOverWin = false;
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

	private IEnumerator DoAfterSecondsCoroutine(float seconds, Action action)
	{
		yield return new WaitForSeconds(seconds);
		action?.Invoke();
	}
	#endregion
}
