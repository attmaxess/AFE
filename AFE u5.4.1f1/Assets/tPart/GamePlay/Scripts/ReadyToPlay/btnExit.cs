using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.iOS;

public class btnExit : MonoBehaviour
{
    [Header("Input")]
    public CanvasGroup canvasVideo = null;
    public CanvasGroup canvasPlaceNote = null;
    public BeetsoftLogoMethod logoMethod = null;
    public VideoPlayer videoPlayer = null;
    public UnityARVideo uityARVideo = null;
    public UnityARCameraNearFar unityARCameraNearFar = null;    
    public PhotonMenu photonMenu = null;
    public GameStateBeThis stateWhenAwake = null;
    public ShapeManager shapeManager = null;
    public BackgroundMarkerMethod backgroundMethod = null;

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
        stateAfterClick.OnClick();

        photonMenu.LeaveRoom();
        yield return new WaitUntil(() => photonMenu.doneLeaveRoom == true);

        LibPlacenote.Instance.StopSession();
        FeaturesVisualizer.clearPointcloud();        
        shapeManager.ClearShapes();

        backgroundMethod.Hide();
        yield return new WaitUntil(() => backgroundMethod.doneHide == true);

        canvasPlaceNote.alpha = 0;
        canvasPlaceNote.interactable = false;
        canvasPlaceNote.blocksRaycasts = false;        

        logoMethod.GoTo1WaitBack0();
        yield return new WaitUntil(() => BeetsoftLogo.Instance.doneC_ToAlPha == true);        

        videoPlayer.Play();

        uityARVideo.UpdateFrameAtStart = false;
        uityARVideo.OnDestroy();

        unityARCameraNearFar.UpdateFrameAtStart = false;
        unityARCameraNearFar.useUpdate = false;

        yield return new WaitUntil(() => logoMethod.doneGoToWaitBack == true);

        canvasVideo.alpha = 1;
        canvasVideo.interactable = true;
        canvasVideo.blocksRaycasts = true;

        yield break;
    }
}
