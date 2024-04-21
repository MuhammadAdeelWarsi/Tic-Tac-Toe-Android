using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /*FIELDS*/
    private static AudioManager instance;

    [SerializeField] AudioSource[] audioSources;
    [SerializeField] AudioClip[] audioClips;


    /*PROPERTIES*/
    public static AudioManager Instance => instance;


    /*INITIALIZING METHODS*/
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }


    /*OTHER METHODS*/
    public void PlayUserInterfaceSound(int audioClipId)     //Can be accessed from Editor directly. Here, looping is not possible
    {
        AudioSource canvasAudioSource = audioSources[0];
        AudioClip audioClip = audioClips[audioClipId];

        canvasAudioSource.clip = audioClip;
        canvasAudioSource.Play();
    }

    public void StopSoundOrMusic(int audioSourceId)
    {
        AudioSource audioSource = audioSources[audioSourceId];
        audioSource.Stop();
    }


    /*COROUTINES*/
    public IEnumerator PlayBackgroundMusic(int audioSourceId, int audioClipId, float delay, bool mustLooped = false)
    {
        AudioSource gameBoardAudioSource = audioSources[audioSourceId];
        AudioClip audioClip = audioClips[audioClipId];

        gameBoardAudioSource.clip = audioClip;

        if(mustLooped)
        {
            gameBoardAudioSource.loop = true;
        }
        else
        {
            gameBoardAudioSource.loop = false;
        }

        yield return new WaitForSeconds(delay);
        Debug.Log("Playing Background Music!!");
        gameBoardAudioSource.Play();
    }

    public IEnumerator StopSoundOrMusic(int audioSourceId, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Stopping Background Music!!");
        AudioSource audioSource = audioSources[audioSourceId];
        audioSource.Stop();
    }
}
