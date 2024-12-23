using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource sfx2AudioSource;
    public AudioSource bgmAudioSource;
    public List<AudioClip> voiceOverClips;
    public List<AudioClip> bgmClips;

    public void PlayBGM(int bgmIndex)
    {
        if (bgmIndex >= 0 && bgmIndex < bgmClips.Count && bgmAudioSource != null)
        {
            // Stop the current BGM if playing
            if (bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Stop();
            }

            // Set the new BGM clip and play it
            bgmAudioSource.clip = bgmClips[bgmIndex];
            bgmAudioSource.Play();
        }
    }

    public void PlaySFX(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < voiceOverClips.Count && sfxAudioSource != null)
        {
            sfxAudioSource.clip = voiceOverClips[clipIndex];
            sfxAudioSource.Play();
        }
    }

    public void PlaySFX2(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < voiceOverClips.Count && sfxAudioSource != null)
        {
            sfx2AudioSource.clip = voiceOverClips[clipIndex];
            sfx2AudioSource.Play();
        }
    }

    public bool IsSFXPlaying()
    {
        return sfxAudioSource != null && sfxAudioSource.isPlaying;
    }

    public void StopSFX()
    {
        if (sfxAudioSource != null && sfxAudioSource.isPlaying)
        {
            sfxAudioSource.Stop(); // Stops the audio playback
        }
    }

    public void StopSFX2()
    {
        if (sfx2AudioSource != null && sfx2AudioSource.isPlaying)
        {
            sfx2AudioSource.Stop(); // Stops the audio playback
        }
    }
}
