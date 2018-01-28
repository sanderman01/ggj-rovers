using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    [System.Serializable]
    public struct Clip
    {
        public AudioClip audioClip;
        public float volume;
    }

    [SerializeField]
    private bool warnOnNullClip = true;
    public bool WarnOnNullClip { get { return warnOnNullClip; } }

    [SerializeField]
    private Clip[] audioClips;
    public Clip[] AudioClips { get { return audioClips; } }

    public void PlayAtIndex(int index)
    {
        AudioManager.PlayAtIndex(this, index);
    }

    [ContextMenu("PlayRandom")]
    public void PlayRandom()
    {
        AudioManager.PlayRandom(this);
    }

    [ContextMenu("StandardVolume")]
    private void StandardVolume()
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioClips[i].volume = 1f;
        }
    }
}
