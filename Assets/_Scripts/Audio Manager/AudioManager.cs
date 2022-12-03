using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    private Coroutine WaitSFXPlayEnd;
    private Coroutine SFXPlay;
    private WaitForSeconds waitForSFXPlayEnd;
    private WaitForSeconds waitForSFXplayEndButCanInterrupted;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;

    public void PlaySFXAndDontRepeat(AudioData audioData)
    {
        if (WaitSFXPlayEnd != null) return;
        waitForSFXPlayEnd = new WaitForSeconds(audioData.audioClip.length);
        WaitSFXPlayEnd = StartCoroutine(WaitSFXPlayEndCorotine(() => PlaySFX(audioData)));
    }

    IEnumerator WaitSFXPlayEndCorotine(Action PlayerSFX)
    {
        PlayerSFX?.Invoke();
        yield return waitForSFXPlayEnd;
        waitForSFXPlayEnd = null;
        WaitSFXPlayEnd = null;
    }

    //Used for UI SFX
    public void PlaySFX(AudioData audioData)
    {
        if (audioData.audioClip != null)
            sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    //Used for repeat_play SFX
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = UnityEngine.Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[UnityEngine.Random.Range(0, audioDatas.Length)]);
    }



}
