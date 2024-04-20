using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager>
{
	public interface ISound
	{
		public string Name { get; }
		public AudioClip AudioClip { get; }
	}

	[System.Serializable]
	public class Sound : ISound
	{
		public string name;
		public AudioClip audioClip;

		public string Name => name;
		public AudioClip AudioClip => audioClip;
	}

	[Header("Audios")]
	[SerializeField] private Sound[] musics;
	[SerializeField] private Sound[] soundEffects;
	[Header("Settings")]
	[SerializeField, Tooltip("Duration in seconds of the audio fade ins and outs.")] private float fadeTime = 1f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private AudioSource[] tracks;

	public const string MusicVolumeKey = "MusicVolume";
	public const string SfxVolumeKey = "SfxVolume";

	public AudioSource[] Tracks => tracks;
	public bool IsMusicMuted
	{
		get => PlayerPrefs.GetInt(MusicVolumeKey, 0) == 1;
		set => PlayerPrefs.SetInt(MusicVolumeKey, value ? 1 : 0);
	}
	public bool IsSfxMuted
	{
		get => PlayerPrefs.GetInt(SfxVolumeKey, 0) == 1;
		set => PlayerPrefs.SetInt(SfxVolumeKey, value ? 1 : 0);
	}

	#region Unity Messages
	protected override void Awake()
	{
		base.Awake();
		tracks = GetComponents<AudioSource>();
	}
	#endregion

	#region Public Methods
	public void PauseTrack(int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);

		if (audioSource != null)
			audioSource.Pause();
	}

	public void ResumeTrack(int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);

		if (audioSource != null)
			audioSource.UnPause();
	}

	public void StopTrack(int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);

		if (audioSource != null)
			audioSource.Stop();
	}

	public void PauseAllTracks()
	{
		foreach (AudioSource track in FindObjectsOfType<AudioSource>())
			PauseTrack(track);
	}

	public void ResumeAllTracks()
	{
		foreach (AudioSource track in FindObjectsOfType<AudioSource>())
			ResumeTrack(track);
	}

	public void StopAllTracks()
	{
		foreach (AudioSource track in FindObjectsOfType<AudioSource>())
			StopTrack(track);
	}

	public bool IsTrackPlayingSound(string soundName, int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);
		if (audioSource.isPlaying == false)
			return false;

		return audioSource.clip.name == GetAudioClip(soundName).name;
	}

	public void PlaySound(string soundName, int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);
		audioSource.clip = GetAudioClip(soundName);
		audioSource.loop = true;

		if (!audioSource.isPlaying)
			audioSource.Play();
	}

	public void PlaySoundOneShot(string soundName, int trackNumber)
	{
		AudioSource audioSource = GetTrack(trackNumber);
		if (audioSource != null)
			audioSource.PlayOneShot(GetAudioClip(soundName));
	}

	public IEnumerator WaitPlaySoundOneShot(string soundName, int trackNumber, Action action = null)
	{
		AudioSource audioSource = GetTrack(trackNumber);
		if (audioSource != null)
			audioSource.PlayOneShot(GetAudioClip(soundName));

		yield return new WaitWhile(() => audioSource.isPlaying);
		action();
	}

	public float GetTrackVolume(int trackNumber)
	{
		return GetTrack(trackNumber).volume;
	}

	public void ChangeTrackVolume(int trackNumber, float volume)
	{
		AudioSource audioSource = GetTrack(trackNumber);
		if (audioSource != null)
			audioSource.volume = volume;
	}

	public void ChangeSoundWithFade(string soundName, int trackNumber) => StartCoroutine(StartChangeSoundWithFade(soundName, trackNumber, GetTrackVolume(trackNumber)));
	#endregion

	#region Private Methods
	private AudioSource GetTrack(int trackNumber)
	{
		if (trackNumber == 0)
		{
			Debug.LogError("Tracks start at number 1.");
		}

		return tracks[trackNumber - 1];
	}

	private AudioClip GetAudioClip(string soundName)
	{
		foreach (ISound sound in soundEffects)
		{
			if (sound.Name == soundName)
				return sound.AudioClip;
		}

		foreach (ISound music in musics)
		{
			if (music.Name == soundName)
				return music.AudioClip;
		}

		Debug.LogError("Audio " + soundName + " not found.");
		return null;
	}

	private void PauseTrack(AudioSource track) => track.Pause();

	private void ResumeTrack(AudioSource track) => track.UnPause();

	private void StopTrack(AudioSource track) => track.Stop();

	private IEnumerator StartChangeSoundWithFade(string soundName, int trackNumber, float targetVolume)
	{
		AudioSource audioSource = GetTrack(trackNumber);

		yield return Fade(audioSource, 0);

		audioSource.clip = GetAudioClip(soundName);
		audioSource.loop = true;

		if (!audioSource.isPlaying)
			audioSource.Play();

		yield return Fade(audioSource, targetVolume);
	}

	private IEnumerator Fade(AudioSource track, float targetVolume)
	{
		float currentTime = 0;
		float start = track.volume;

		while (currentTime < fadeTime)
		{
			currentTime += Time.deltaTime;
			track.volume = Mathf.Lerp(start, targetVolume, currentTime / fadeTime);
			yield return null;
		}

		yield break;
	}
	#endregion
}