using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CameraShake : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float intensity = 2;
	[SerializeField] private float time = .3f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private CinemachineVirtualCamera vCam;
	[SerializeField, ReadOnly] private float shakeTimer;

	private void Awake()
	{
		Health.OnAnyHealthDamage += ShakeCamera;

		vCam = GetComponent<CinemachineVirtualCamera>();
		var noiseCam = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		noiseCam.m_AmplitudeGain = 0;
	}

	private void OnDestroy()
	{
		Health.OnAnyHealthDamage -= ShakeCamera;
	}

	private void Update()
	{
		if (shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
			if (shakeTimer <= 0 )
			{
				var noiseCam = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
				noiseCam.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, 1- (shakeTimer / time));
			}
		}
	}

	private void ShakeCamera()
	{
		var noiseCam = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		noiseCam.m_AmplitudeGain = intensity;
		shakeTimer = time;
	}
}
