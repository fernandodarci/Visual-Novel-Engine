using UnityEngine.Audio;
using UnityEngine;
using System;


public class GSC_AudioSource : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup Mixer;
    [SerializeField] private AudioSource Source;
    [Range(0f, 1f)] public float Volume;
    
    public void InitializeAudioSource()
    {
        if (Source != null && Mixer != null)
        {
            Source.enabled = true;
            Source.volume = Volume;
            Source.outputAudioMixerGroup = Mixer;
        }
    }

    public void Play(AudioClip clip, float volume, float pitch, bool loop)
    {
        Source.clip = clip;
        Source.loop = loop;
        Source.pitch = pitch;
        Source.volume = Volume * volume;
        Source.Play();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        Source.PlayOneShot(clip, volume * Volume);
    }

    public void Pause()
    {
        if (Source.isPlaying) Source.Pause();
    }

    public void Resume()
    {
        if (!Source.isPlaying && Source.clip != null)
            Source.UnPause();
    }

    public void Stop()
    {
        if (Source.isPlaying) Source.Stop();
    }

    public void SetVolume(float volume)
    {
        Source.volume = Mathf.Clamp01(volume);
    }

    public void SetPitch(float pitch)
    {
        Source.pitch = Mathf.Clamp01(pitch);
    }

    public void Mute()
    {
        Source.mute = true;
    }

    public void Unmute()
    {
        Source.mute = false;
    }

    public void UpdateAudioSource(float MasterVolume)
    {
        Source.volume = Mathf.Clamp01(Volume * MasterVolume);
    }
}
