using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayRandomVideo : MonoBehaviour
{
    [Header("Input")]
    public VideoPlayer video = null;

    [Header("Params")]
    public List<VideoClip> videoList = new List<VideoClip>();
    public bool PlayAtStart = true;

    private void Start()
    {
        if (PlayAtStart) PlayCurrent();
    }

    [ContextMenu("PlayRandom")]
    public void PlayCurrent()
    {
        if (video.clip != null) video.Play();
    }

    [ContextMenu("PlayRandom")]
    public void PlayRandom()
    {
        if (videoList != null && videoList.Count != 0)
        {
            int ranIndex = Random.Range(0, videoList.Count);
            video.clip = videoList[ranIndex];
        }

        PlayCurrent();
    }

    [ContextMenu("PlayNext")]
    public void PlayNext()
    {
        if (videoList.Count == 0) return;
        int currentIndex = videoList.FindIndex((x) => x == video.clip);
        currentIndex = currentIndex == -1 ? 0 : currentIndex;
        currentIndex = currentIndex == videoList.Count - 1 ? 0 : currentIndex + 1;
        video.clip = videoList[currentIndex];
        PlayCurrent();
    }

    [ContextMenu("PlayPrevious")]
    public void PlayPrevious()
    {
        if (videoList.Count == 0) return;
        int currentIndex = videoList.FindIndex((x) => x == video.clip);
        currentIndex = currentIndex == -1 ? 0 : currentIndex;
        currentIndex = currentIndex == 0 ? videoList.Count - 1 : currentIndex - 1;
        video.clip = videoList[currentIndex];
        PlayCurrent();
    }
}
