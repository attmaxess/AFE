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
    public UnityARVideo unityARVideo = null;
    public UnityARCameraNearFar unityARCameraNearFar = null;    
    public PhotonMenu photonMenu = null;
    public GameStateBeThis stateWhenAwake = null;
    public Transform tribalHolder = null;
    public CanvasTribalWorld_Movement canvasTribal = null;

    private void Awake()
    {
        stateWhenAwake.OnClick();
    }

    [Header("OnClick")]
    public GameStateBeThis stateAfterClick = null;

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
        canvasVideo.interactable = false;
        canvasVideo.blocksRaycasts = false;

        videoPlayer.Stop();        

        unityARVideo.UpdateFrameAtStart = true;        
        unityARVideo.Start();

        unityARCameraNearFar.UpdateFrameAtStart = true;
        unityARCameraNearFar.useUpdate = true;
        unityARCameraNearFar.Start();

        yield return new WaitUntil(() => logoMethod.doneGoToWaitBack == true);                

        stateAfterClick.OnClick();        

        tribalHolder.gameObject.SetActive(true);        
        canvasTribal.ZoomCamAndDistance();

        yield return new WaitUntil(() => canvasTribal.doneZoom == true);

        yield break;
    }
}
