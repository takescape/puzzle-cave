using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayClickSound : MonoBehaviour
{
	[SerializeField] private string clickSoundName = "ui_click";
	[SerializeField, Min(1)] private int trackNumber = 3;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(PlaySound);
	}

	private void OnDestroy()
	{
		button.onClick.RemoveListener(PlaySound);
	}

	[Button(enabledMode:EButtonEnableMode.Playmode)]
	public void PlaySound()
	{
		AudioManager.Instance.PlaySoundOneShot(clickSoundName, trackNumber);
	}
}
