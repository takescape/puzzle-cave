using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : Singleton<SceneTransition>
{
	public static event Action OnSceneChanged;

	[Header("Transition Settings")]
	[SerializeField] private float transitionTime = 1f;
    [Header("References")]
	[SerializeField] private Animator transitionAnimator;

	protected override void Awake()
	{
		base.Awake();
		Time.timeScale = 1f;
	}

	#region Public Methods
	public static void TransitionToNextLevel()
	{
		Instance.Transition();
	}

	public static void TransitionToScene(int sceneIndex)
	{
		Instance.Transition(sceneIndex);
	}

	public void Transition(int sceneIndex = -1)
	{
		transitionAnimator.SetTrigger("Fade");
		StartCoroutine(TransitionCoroutine(sceneIndex));
	}
	#endregion

	#region Private Methods
	private IEnumerator TransitionCoroutine(int sceneIndex = -1)
	{
		yield return new WaitForSecondsRealtime(Instance.transitionTime);

		Time.timeScale = 1f;
		AsyncOperation asyncOp;

		// if no scene is specified, tries to transition to the
		// next scene in build settings
		if (sceneIndex == -1)
		{
			if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
				asyncOp = SceneManager.LoadSceneAsync(0);
			else
				asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else
			asyncOp = SceneManager.LoadSceneAsync(sceneIndex);

		asyncOp.completed += RaiseOnSceneChangedEvent;
	}

	private void RaiseOnSceneChangedEvent(AsyncOperation asyncOp)
	{
		asyncOp.completed -= RaiseOnSceneChangedEvent;
		OnSceneChanged?.Invoke();
	}

	[Button]
	private void TestTransition()
	{
		TransitionToNextLevel();
	}
	#endregion
}
