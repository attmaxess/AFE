using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class TransformMethods : MonoBehaviour
{
    Transform _arCam = null;
    Transform arCam { get { if (_arCam == null) _arCam = Camera.main.transform; return _arCam; } }

    public InputField ipf = null;    

    void SnapTempCam()
    {        
        //tempCam.position = Camera.main.transform.position;
        //tempCam.rotation = Camera.main.transform.rotation;
    }

    void SetWorldOrigin()
    {
        UnityARSessionNativeInterface.GetARSessionNativeInterface().SetWorldOrigin(arCam.transform);
    }

    #region Translate
    /// <summary>
    /// Translate
    /// </summary>
    [ContextMenu("TranslateX")]
    public void TranslateX()
    {
        SnapTempCam();
        arCam.Translate(arCam.right.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("TranslateNX")]
    public void TranslateNX()
    {
        SnapTempCam();
        arCam.Translate(-arCam.right.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("TranslateY")]
    public void TranslateY()
    {
        SnapTempCam();
        arCam.Translate(arCam.up.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("TranslateNY")]
    public void TranslateNY()
    {
        SnapTempCam();
        arCam.Translate(-arCam.up.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("TranslateZ")]
    public void TranslateZ()
    {
        SnapTempCam();
        arCam.Translate(arCam.forward.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("TranslateNZ")]
    public void TranslateNZ()
    {
        SnapTempCam();
        arCam.Translate(-arCam.forward.normalized * float.Parse(ipf.text));
        SetWorldOrigin();
    }
    #endregion Translate

    #region Rotate
    /// <summary>
    /// Rotate
    /// </summary>
    [ContextMenu("RotateX")]
    public void RotateX()
    {
        SnapTempCam();
        arCam.Rotate(arCam.right, float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("RotateNX")]
    public void RotateNX()
    {
        SnapTempCam();
        arCam.Rotate(-arCam.right, float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("RotateY")]
    public void RotateY()
    {
        SnapTempCam();
        arCam.Rotate(arCam.up, float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("RotateNY")]
    public void RotateNY()
    {
        SnapTempCam();
        arCam.Rotate(-arCam.up, float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("RotateZ")]
    public void RotateZ()
    {
        SnapTempCam();
        arCam.Rotate(arCam.forward, float.Parse(ipf.text));
        SetWorldOrigin();
    }

    [ContextMenu("RotateNZ")]
    public void RotateNZ()
    {
        SnapTempCam();
        arCam.Rotate(-arCam.forward, float.Parse(ipf.text));
        SetWorldOrigin();
    }
    #endregion Rotate
}
