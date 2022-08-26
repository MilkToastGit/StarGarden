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
        video.prepareCompleted += PrepareCompleted;
        video.started += Started;
        video.errorReceived += ErrorReceived;
        video.Prepare();
    }

    private void PrepareCompleted(VideoPlayer source)
    {
        video.Play();
    }

    private void Started(VideoPlayer source)
    {
        sound.Play();
    }

    private void ErrorReceived(VideoPlayer source, string message)
    {
        print(message);
        Handheld.PlayFullScreenMovie("StreamingAssets/StarGardenGacha.mp4");
    }


    private void OnDisable()
    {
        video.prepareCompleted -= PrepareCompleted;
        video.started -= Started;
        video.errorReceived -= ErrorReceived;
    }
}
