using UnityEngine;
using UnityEngine.Audio;
using Doozy;

public class AudioManager : MonoBehaviour
{
	[Header("Audio Mixer")]
	[SerializeField] private AudioMixer _audioMixer;

	[Header("Mixer Parameters")]
	[SerializeField] private string _masterVolumeParam = "MasterVolume";
	[SerializeField] private string _musicVolumeParam = "MusicVolume";
	[SerializeField] private string _sfxVolumeParam = "SFXVolume";

	private const float MIN_VOLUME = -80f;
	private const float MAX_VOLUME = 0f;

	private void Start()
	{
		InitializeVolumes();
	}

	private void InitializeVolumes()
	{
		SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
		SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
		SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
	}

	public void SetMasterVolume(float volume)
	{
		SetVolume(_masterVolumeParam, volume);
		PlayerPrefs.SetFloat("MasterVolume", volume);
	}

	public void SetMusicVolume(float volume)
	{
		SetVolume(_musicVolumeParam, volume);
		PlayerPrefs.SetFloat("MusicVolume", volume);
	}

	public void SetSFXVolume(float volume)
	{
		SetVolume(_sfxVolumeParam, volume);
		PlayerPrefs.SetFloat("SFXVolume", volume);
	}
	private void SetVolume(string parameterName, float volume)
	{
		if (_audioMixer == null)
		{
			Debug.LogWarning("AudioMixer не назначен!");
			return;
		}
		float volumeDB = VolumeToDB(volume);
		_audioMixer.SetFloat(parameterName, volumeDB);
	}
	private float VolumeToDB(float volume)
	{
		if (volume <= 0.0001f)
			return MIN_VOLUME;

		return Mathf.Log10(volume) * 20f;
	}

	private float DBToVolume(float dB)
	{
		return Mathf.Pow(10f, dB / 20f);
	}

	public float GetVolume(string parameterName)
	{
		if (_audioMixer == null) return 1f;

		if (_audioMixer.GetFloat(parameterName, out float currentDB))
		{
			return DBToVolume(currentDB);
		}

		return 1f;
	}

	public void ToggleMute(bool mute)
	{
		if (mute)
		{
			SetMasterVolume(0f);
		}
		else
		{
			SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
		}
	}

	public void FadeVolume(string parameterName, float targetVolume, float duration)
	{
		StartCoroutine(FadeVolumeCoroutine(parameterName, targetVolume, duration));
	}

	private System.Collections.IEnumerator FadeVolumeCoroutine(string parameterName, float targetVolume, float duration)
	{
		float startVolume = GetVolume(parameterName);
		float timer = 0f;

		while (timer < duration)
		{
			timer += Time.deltaTime;
			float currentVolume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
			SetVolume(parameterName, currentVolume);
			yield return null;
		}

		SetVolume(parameterName, targetVolume);
	}

	public void ResetAudioSettings()
	{
		SetMasterVolume(1f);
		SetMusicVolume(0.8f);
		SetSFXVolume(0.8f);

		PlayerPrefs.Save();
	}

	public void SaveAudioSettings()
	{
		PlayerPrefs.Save();
	}
}