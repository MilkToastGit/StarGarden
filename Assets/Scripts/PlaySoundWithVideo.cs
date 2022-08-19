using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using FMODUnity;

public class PlaySoundWithVideo : MonoBehaviour
{
    public VideoPlayer video;
    public StudioEventEmitter sound;

    private void OnEnable()
    {
        video.started += Play;
    }

    private void OnDisable()
    {
        video.started -= Play;
    }

    private void Play(VideoPlayer source)
    {
        sound.Play();
    }
}
