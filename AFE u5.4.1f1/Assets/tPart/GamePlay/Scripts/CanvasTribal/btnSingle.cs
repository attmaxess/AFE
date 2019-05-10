using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSingle : MonoBehaviour
{
    [Header("Input")]
    public GameObject placeNoteHolder = null;
    public CanvasGroup canvasPlaceNote = null;
    public PhotonMenu photonMenu = null;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        StartCoroutine(C_OnClick());
    }

    IEnumerator C_OnClick()
    {
        placeNoteHolder.gameObject.SetActive(true);

        canvasPlaceNote.alpha = 1;
        canvasPlaceNote.interactable = true;
        canvasPlaceNote.blocksRaycasts = true;

        photonMenu.HandleCurrentMapName();

        yield break;
    }
}