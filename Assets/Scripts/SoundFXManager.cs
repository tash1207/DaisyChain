using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXObject;

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
        AudioSource audioSource = Instantiate(soundFXObject, Camera.main.transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
