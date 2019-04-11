using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMarkerMethod : MonoBehaviour
{    
    BackgroundMarker currentBackgroundMarker = null;

    [Header("FindBackground")]
    public bool doneFindBackground = true;

    [ContextMenu("FindBackground")]
    public void FindBackground()
    {
        StartCoroutine(C_FindBackground());
    }

    IEnumerator C_FindBackground()
    {
        doneFindBackground = false;

        yield return new WaitUntil(() => FindObjectOfType<BackgroundMarker>() != null);
        currentBackgroundMarker = FindObjectOfType<BackgroundMarker>();

        doneFindBackground = true;

        yield break;
    }

    [ContextMenu("Show")]
    public void Show()
    {
        StartCoroutine(C_Show());
    }

    IEnumerator C_Show()
    {
        if (currentBackgroundMarker == null)
        {
            FindBackground();
            yield return new WaitUntil(() => currentBackgroundMarker != null);
        }

        currentBackgroundMarker.Show();
        yield break;
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        StartCoroutine(C_Hide());
    }

    IEnumerator C_Hide()
    {
        if (currentBackgroundMarker == null)
        {
            FindBackground();
            yield return new WaitUntil(() => currentBackgroundMarker != null);
        }

        currentBackgroundMarker.Hide();
        yield break;
    }

    [ContextMenu("ToggleShowHide")]
    public void ToggleShowHide()
    {
        StartCoroutine(C_ToggleShowHide());
    }

    IEnumerator C_ToggleShowHide()
    {
        if (currentBackgroundMarker == null)
        {
            FindBackground();
            yield return new WaitUntil(() => currentBackgroundMarker != null);
        }

        currentBackgroundMarker.ToggleShowHide();
        yield break;
    }    
}
