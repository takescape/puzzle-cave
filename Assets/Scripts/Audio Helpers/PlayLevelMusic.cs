using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevelMusic : MonoBehaviour
{
	[System.Serializable]
	public struct MusicPerTrack
	{
		public string MusicName;
		public int Track;
	}

	[SerializeField] private MusicPerTrack[] musicsPerTrack;

	private void Start()
	{
		foreach (var musicPerTrack in musicsPerTrack)
			AudioManager.Instance.PlaySound(musicPerTrack.MusicName, musicPerTrack.Track <= 0 ? 1 : musicPerTrack.Track);
	}
}
