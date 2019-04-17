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

        float momentFound = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<BackgroundMarker>() != null || Time.time - momentFound > 1f);
        currentBackgroundMarker = FindObjectOfType<BackgroundMarker>();        

        doneFindBackground = true;

        yield break;
    }

    [Header("Show")]
    public bool doneShow = true;

    [ContextMenu("Show")]
    public void Show()
    {
        StartCoroutine(C_Show());
    }

    IEnumerator C_Show()
    {
        doneShow = false;

        if (currentBackgroundMarker == null)
        {
            FindBackground();
            yield return new WaitUntil(() => doneFindBackground == true);
        }

        if (currentBackgroundMarker != null) currentBackgroundMarker.Show();
        doneShow = true;

        yield break;
    }

    [Header("Hide")]
    public bool doneHide = true;

    [ContextMenu("Hide")]
    public void Hide()
    {
        StartCoroutine(C_Hide());
    }

    IEnumerator C_Hide()
    {
        doneHide = false;

        if (currentBackgroundMarker == null)
        {
            FindBackground();
            yield return new WaitUntil(() => doneFindBackground == true);
        }

        if (currentBackgroundMarker != null) currentBackgroundMarker.Hide();
        doneHide = true;

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
            yield return new WaitUntil(() => doneFindBackground == true);
        }

        if (currentBackgroundMarker != null) currentBackgroundMarker.ToggleShowHide();
        yield break;
    }
}
