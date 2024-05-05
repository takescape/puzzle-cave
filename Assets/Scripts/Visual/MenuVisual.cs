using Cinemachine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVisual : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float changeEverySeconds = 6;
	[Header("References")]
	[SerializeField] private CinemachineVirtualCamera[] vcams;
	[Header("Debug")]
	[SerializeField, ReadOnly] private float timer;
	[SerializeField, ReadOnly] private int currentCamera;

	private void Awake()
	{
		currentCamera = 0;
		UpdateCameras();

		timer = changeEverySeconds - 1;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > changeEverySeconds)
		{
			currentCamera++;
			if (currentCamera > vcams.Length - 1)
				currentCamera = 0;

			UpdateCameras();
			timer = 0f;
		}
	}

	private void UpdateCameras()
	{
		for (int i = 0; i < vcams.Length; i++)
			vcams[i].gameObject.SetActive(i == currentCamera);
	}
}
