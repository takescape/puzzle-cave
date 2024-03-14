using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Fields
    [Header("Random")]
    [SerializeField] private int seed;
    [Header("Debug")]
    [SerializeField, ReadOnly] private bool isGameStarted;
    [SerializeField, ReadOnly] private bool isGameOverWin;
	[SerializeField, ReadOnly] private bool isGameOverDefeat;
    [SerializeField, ReadOnly] private bool isGamePaused;
	#endregion

	#region Properties
	public static bool IsGameOver => Instance.isGameOverWin || Instance.isGameOverDefeat;
    public static bool IsGameOverWin => Instance.isGameOverWin;
	public static bool IsGameOverDefeat => Instance.isGameOverDefeat;
	public static bool IsGamePaused => Instance.isGamePaused;
	public static bool IsGameStarted => Instance.isGameStarted;
	#endregion

	#region Unity Messages

	protected override void Awake()
    {
		base.Awake();
        Random.InitState(seed);

        // unpause game on scene init
        Time.timeScale = 1f;
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			TogglePause();
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
	}

	public static void Defeat()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;
	}

    public static void Retry()
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
	#endregion

	#region Private Methods
	#endregion
}
