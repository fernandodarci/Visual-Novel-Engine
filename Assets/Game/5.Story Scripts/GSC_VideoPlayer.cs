using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GSC_VideoPlayer : GSC_GraphicLayer
{
    [SerializeField] private AudioSource videoSource;
    private VideoPlayer VideoPlayer;

    public void PrepareVideo(VideoClip clip)
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        VideoPlayer.SetTargetAudioSource(0,videoSource);
        VideoPlayer.clip = clip;
        VideoPlayer.Prepare();
    }

    public IEnumerator PlayVideo(float fadeTime)
    {
        while(VideoPlayer.isPrepared) yield return null;

        float elapsed = 0f;
        VideoPlayer.Play();
        while (elapsed < fadeTime)
        {
            SmoothChangeSpriteStep(elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator StopVideo(Sprite nextSprite, float fadeTime)
    {
        float elapsed = 0f;
        StartTransition(nextSprite);
        while (elapsed < fadeTime)
        {
            SmoothReverseChangeSpriteStep(elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        VideoPlayer.Stop();
    }

    private void SmoothReverseChangeSpriteStep(float elapsed)
    {
        throw new NotImplementedException();
    }
}


