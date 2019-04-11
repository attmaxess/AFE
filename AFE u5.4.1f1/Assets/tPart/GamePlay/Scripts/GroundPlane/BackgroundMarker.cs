using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundMarker : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Setting Transform")]
    public MeshCollider mc = null;
    public List<MeshRenderer> mrList = null;    

    [ContextMenu("FindMRList")]
    public void FindMRList()
    {
        mrList = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
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
        foreach (MeshRenderer mr in mrList) mr.enabled = true;
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        if (isDebug) Debug.Log("Hide Background Marker");
        foreach (MeshRenderer mr in mrList) mr.enabled = false;
    }

    [ContextMenu("ToggleShowHide")]
    public void ToggleShowHide()
    {
        if (isDebug) Debug.Log("Toggle Background Marker");
        foreach (MeshRenderer mr in mrList) mr.enabled = !mr.enabled;
    }

    #endregion local method

    [Header("Retrieve Main Char")]
    public RetrieveMainCharacter retrieveMainChar = null;    
}
