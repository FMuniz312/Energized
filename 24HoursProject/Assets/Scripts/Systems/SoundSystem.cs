using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioClipData[] audioClipData;


    static public SoundSystem instance;
    public float volumeMultiplier { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public enum Sound
    {
        ConfirmationSound,
        DeclineSound,
        MenuClick1,
        MenuClick2,
        MenuClick3,
        PlayerDash,
        PlayerHurt,
        ProjectileExplode,
        PlayerAttack,



    }

    public void PlaySound(Sound soundType)
    {
        GameObject gameObject = new GameObject("SoundEffect_", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        AudioClip audioClip = GetAudioClip(soundType);
        audioSource.volume *= volumeMultiplier;
        audioSource.PlayOneShot(audioClip);
        Destroy(gameObject, audioClip.length);

    }

    public void ChangeVolumeMultiplier(float volume)
    {
        volumeMultiplier = volume;
    }
    AudioClip GetAudioClip(Sound sound)
    {
        foreach (AudioClipData ClipData in audioClipData)
        {
            if (ClipData.Sound == sound)
            {
                return ClipData.AudioClip;
            }
        }
        return null;
    }





    [System.Serializable]
    public class AudioClipData
    {
        public AudioClip AudioClip;
        public Sound Sound;
    }

    private void OnDestroy()
    {
       // instance = null;
    }
}


