using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public static void PlayAtIndex(AudioEvent audioEvent, int index)
    {
        if(Instance != null)
        {
            Instance.PlayEventAtIndex(audioEvent, index);
        }
    }

    public static void PlayRandom(AudioEvent audioEvent)
    {
        if (Instance != null)
        {
            Instance.PlayEventRandom(audioEvent);
        }
    }

    public void PlayEventAtIndex(AudioEvent audioEvent, int index)
    {
        if (audioEvent == null) return;

        Assert.IsNotNull(audioEvent.AudioClips);
        float volume = audioEvent.AudioClips[index].volume;
        AudioClip clip = audioEvent.AudioClips[index].audioClip;

        if (clip == null)
        {
            if (audioEvent.WarnOnNullClip) Debug.LogWarning("AudioEvent contains null reference to AudioClip!", this);
        }
        else
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayEventRandom(AudioEvent audioEvent)
    {
        if (audioEvent == null) return;

        // play a random clip from the list.
        Assert.IsNotNull(audioEvent.AudioClips);
        int index = Random.Range(0, audioEvent.AudioClips.Length);
        PlayEventAtIndex(audioEvent, index);
    }
}
