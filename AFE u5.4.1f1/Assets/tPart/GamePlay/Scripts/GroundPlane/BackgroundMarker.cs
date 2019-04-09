using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMarker : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Setting Transform")]
    public MeshCollider mc = null;
    public MeshRenderer mr = null;    

    public void SetSize(float size)
    {
        mr.transform.localScale = new Vector3(size, size, size);
    }

    public void MoveAStep(float step)
    {
        transform.position += new Vector3(0, step, 0);
    }

    #region local method

    [ContextMenu("Show")]
    public void Show()
    {
        if (isDebug) Debug.Log("Show Background Marker");
        mr.enabled = true;
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        if (isDebug) Debug.Log("Hide Background Marker");
        mr.enabled = false;
    }

    [ContextMenu("ToggleShowHide")]
    public void ToggleShowHide()
    {
        if (isDebug) Debug.Log("Toggle Background Marker");
        mr.enabled = !mr.enabled;
    }

    #endregion local method
}
