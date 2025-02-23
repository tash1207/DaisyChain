using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXObject;
    [SerializeField] AudioClip clickSFX;

    public static SoundFXManager instance;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, float volume)
    {
        PlaySoundFXClip(audioClip, volume, false);
    }

    public void PlaySoundFXClip(AudioClip audioClip, float volume, bool modulation)
    {
        AudioSource audioSource = Instantiate(soundFXObject, Camera.main.transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        if (modulation)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClipXTimes(AudioClip audioClip, float volume, int times)
    {
        StartCoroutine(PlayAudio(audioClip, volume, times, 0f, false));
    }

    public void PlaySoundFXClipXTimes(AudioClip audioClip, float volume, int times, float delay)
    {
        StartCoroutine(PlayAudio(audioClip, volume, times, delay, false));
    }

    public void PlaySoundFXClipXTimesWithModulation(AudioClip audioClip, float volume, int times, float delay)
    {
        StartCoroutine(PlayAudio(audioClip, volume, times, delay, true));
    }

    IEnumerator PlayAudio(AudioClip audioClip, float volume, int times, float delay, bool modulation)
    {
        for (int i = 0; i < times; i++)
        {
            PlaySoundFXClip(audioClip, volume, modulation);
            yield return new WaitForSeconds(audioClip.length + delay);
        }
    }

    public void ClickButton()
    {
        SoundFXManager.instance.PlaySoundFXClip(clickSFX, 1f);
    }
}
