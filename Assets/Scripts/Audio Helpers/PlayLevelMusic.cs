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
		{
			int track = musicPerTrack.Track <= 0 ? 1 : musicPerTrack.Track;
			if (musicPerTrack.MusicName == string.Empty)
				AudioManager.Instance.StopTrack(track);
			else
			{
				if (AudioManager.Instance.IsTrackPlayingSound(musicPerTrack.MusicName, track) == false)
					AudioManager.Instance.ChangeSoundWithFade(musicPerTrack.MusicName, track);
			}
		}
	}
}
