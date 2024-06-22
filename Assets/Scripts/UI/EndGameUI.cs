using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float showOnStartAfter = 3f;
	[SerializeField] private float animationTime = 5f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Animator animator;
	[SerializeField, ReadOnly] private CanvasGroup canvasGroup;
	[SerializeField, ReadOnly] private bool isShowing;
	[SerializeField, ReadOnly] private float showingTimer;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		canvasGroup = GetComponent<CanvasGroup>();
		showingTimer = 0f;

		GameManager.DoAfterSeconds(showOnStartAfter, ShowUI);
	}

	private void Update()
	{
		if (showingTimer > 0)
		{
			showingTimer -= Time.deltaTime;
			if (showingTimer <= 0)
				isShowing = false;
		}

		if (Input.anyKeyDown && !isShowing)
			ShowUI();
	}

	private void ShowUI()
	{
		animator.SetTrigger("Show");
		isShowing = true;
		showingTimer = animationTime;
	}
}
