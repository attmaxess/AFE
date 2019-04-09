using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.iOS;

public class btnAction : MonoBehaviour
{
    [Header("Input")]
    public CanvasGroup canvasVideo = null;
    public BeetsoftLogoMethod logoMethod = null;
    public VideoPlayer videoPlayer = null;
    public UnityARVideo uityARVideo = null;
    public UnityARCameraNearFar unityARCameraNearFar = null;
    public GameObject panelPlaceNote = null;
    public PhotonMenu photonMenu = null;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        StartCoroutine(C_OnClick());
    }

    IEnumerator C_OnClick()
    {
        canvasVideo.interactable = false;
        photonMenu.ReConnectPhotonAlpha1();
        yield return new WaitUntil(() => photonMenu.doneReConnectPhotonAlpha1 == true);

        logoMethod.GoTo1WaitBack0();
        yield return new WaitUntil(() => BeetsoftLogo.Instance.doneC_ToAlPha == true);
        canvasVideo.alpha = 0;        

        videoPlayer.Stop();
        videoPlayer.clip = null;

        uityARVideo.UpdateFrameAtStart = true;        
        uityARVideo.Start();

        unityARCameraNearFar.UpdateFrameAtStart = true;
        unityARCameraNearFar.useUpdate = true;
        unityARCameraNearFar.Start();

        yield return new WaitUntil(() => logoMethod.doneGoToWaitBack == true);

        panelPlaceNote.gameObject.SetActive(true);

        yield break;
    }
}
