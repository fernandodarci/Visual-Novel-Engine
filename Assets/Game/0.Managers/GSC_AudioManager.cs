using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GSC_AudioManager : GSC_Singleton<GSC_AudioManager>
{
    [SerializeField] private GSC_AudioSource SourcePrefab;
    [SerializeField] private AudioMixerGroup SFX;
    [SerializeField] private AudioMixerGroup Voices;
    [SerializeField] private AudioMixerGroup Music;
    [SerializeField] private AudioMixerGroup Ambient;

    // Valores de volume
    private float AudioVolume = 1f;
    private float SFXVolume = 1f;
    private float VoicesVolume = 1f;
    private float MusicVolume = 1f;
    private float AmbientVolume = 1f;

    // Referências para as fontes atuais de música e ambiente
    private GSC_AudioSource currentMusicSource;
    private GSC_AudioSource currentAmbientSource;

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        GSC_AudioSource audioSource = Instantiate(SourcePrefab, transform);
        audioSource.Mixer = SFX;
        audioSource.InitializeAudioSource();
        audioSource.Play(clip, SFXVolume * AudioVolume, 1f, false);
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;
        GSC_AudioSource audioSource = Instantiate(SourcePrefab, transform);
        audioSource.Mixer = Voices;
        audioSource.InitializeAudioSource();
        audioSource.Play(clip, VoicesVolume * AudioVolume, 1f, false);
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        if (clip == null) return;

        if (currentMusicSource != null)
        {
             StartCoroutine(CrossFadeMusic(currentMusicSource, clip, fadeDuration));
        }
        else
        {
            currentMusicSource = Instantiate(SourcePrefab, transform);
            currentMusicSource.Mixer = Music;
            currentMusicSource.InitializeAudioSource();
            currentMusicSource.Source.volume = 0;
            currentMusicSource.Play(clip, MusicVolume * AudioVolume, 1f, true);
            StartCoroutine(FadeAudio(currentMusicSource, 0, MusicVolume * AudioVolume, fadeDuration));
        }
    }

    public void StopMusic()
    {
        if (currentMusicSource != null) currentMusicSource.Stop();
    }
    
    public void PlayAmbient(AudioClip clip, float fadeDuration = 1f)
    {
        if (clip == null) return;

        if (currentAmbientSource != null)
        {
            StartCoroutine(CrossFadeAmbient(currentAmbientSource, clip, fadeDuration));
        }
        else
        {
            currentAmbientSource = Instantiate(SourcePrefab, transform);
            currentAmbientSource.Mixer = Ambient;
            currentAmbientSource.InitializeAudioSource();
            currentAmbientSource.Source.volume = 0;
            currentAmbientSource.Play(clip, AmbientVolume * AudioVolume, 1f, true);
            StartCoroutine(FadeAudio(currentAmbientSource, 0, AmbientVolume * AudioVolume, fadeDuration));
        }
    }

    public void StopAmbient()
    {
        if (currentAmbientSource != null) currentAmbientSource.Stop(); 
    }

    private IEnumerator FadeAudio(GSC_AudioSource audioSource, float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            audioSource.SetVolume(newVolume);
            yield return null;
        }
        audioSource.SetVolume(targetVolume);
    }

    // Coroutine para realizar o cross fade da música atual para a nova faixa
    private IEnumerator CrossFadeMusic(GSC_AudioSource oldSource, AudioClip newClip, float duration)
    {
        GSC_AudioSource newSource = Instantiate(SourcePrefab, transform);
        newSource.Mixer = Music;
        newSource.InitializeAudioSource();
        newSource.Source.volume = 0;
        newSource.Play(newClip, MusicVolume * AudioVolume, 1f, true);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            oldSource.SetVolume(Mathf.Lerp(MusicVolume * AudioVolume, 0, t));
            newSource.SetVolume(Mathf.Lerp(0, MusicVolume * AudioVolume, t));
            yield return null;
        }
        oldSource.Stop();
        Destroy(oldSource.gameObject);
        currentMusicSource = newSource;
    }

    // Coroutine para realizar o cross fade do áudio ambiente atual para a nova faixa
    private IEnumerator CrossFadeAmbient(GSC_AudioSource oldSource, AudioClip newClip, float duration)
    {
        GSC_AudioSource newSource = Instantiate(SourcePrefab, transform);
        newSource.Mixer = Ambient;
        newSource.InitializeAudioSource();
        newSource.Source.volume = 0;
        newSource.Play(newClip, AmbientVolume * AudioVolume, 1f, true);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            oldSource.SetVolume(Mathf.Lerp(AmbientVolume * AudioVolume, 0, t));
            newSource.SetVolume(Mathf.Lerp(0, AmbientVolume * AudioVolume, t));
            yield return null;
        }
        oldSource.Stop();
        Destroy(oldSource.gameObject);
        currentAmbientSource = newSource;
    }
}
