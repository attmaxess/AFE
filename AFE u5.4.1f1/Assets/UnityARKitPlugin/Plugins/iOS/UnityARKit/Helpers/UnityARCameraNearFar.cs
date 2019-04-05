using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

[RequireComponent(typeof(Camera))]
public class UnityARCameraNearFar : MonoBehaviour
{
    private Camera attachedCamera;
    private float currentNearZ;
    private float currentFarZ;

    [Header("UpdateFrame")]
    public bool UpdateFrameAtStart = false;
    public bool useUpdate = false;

    // Use this for initialization
    [ContextMenu("Start")]
    public void Start()
    {
        if (UpdateFrameAtStart)
        {
            attachedCamera = GetComponent<Camera>();
            UpdateCameraClipPlanes();
            useUpdate = true;
        }
    }

    void UpdateCameraClipPlanes()
    {
        currentNearZ = attachedCamera.nearClipPlane;
        currentFarZ = attachedCamera.farClipPlane;
        UnityARSessionNativeInterface.GetARSessionNativeInterface().SetCameraClipPlanes(currentNearZ, currentFarZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!useUpdate) return;

        if (currentNearZ != attachedCamera.nearClipPlane || currentFarZ != attachedCamera.farClipPlane)
        {
            UpdateCameraClipPlanes();
        }
    }
}
