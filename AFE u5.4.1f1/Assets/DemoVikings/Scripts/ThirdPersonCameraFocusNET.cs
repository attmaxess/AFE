using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraFocusNET : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;
    public bool useUpdate = true;

    public Collider target;
    // The object we're looking at
    new public Camera camera;
    // The camera to control

    void Reset()
    // Run setup on component attach, so it is visually more clear which references are used
    {
        Setup();
    }

    // If target and/or camera is not set, try using fallbacks
    void Setup()
    {
        if (target == null)
        {
            target = GetComponent<Collider>();
            lastTarPos = target != null ? target.transform.position : Vector3.zero;
        }

        if (camera == null)
        {
            if (Camera.main != null)
            {
                camera = Camera.main;
            }
        }
    }

    // Verify setup, initialise bookkeeping
    void Start()
    {
        // Retry setup if references were cleared post-add
        Setup();

        if (target == null)
        {
            Debug.LogError("No target assigned. Please correct and restart.");
            enabled = false;
            return;
        }

        if (camera == null)
        {
            Debug.LogError("No camera assigned. Please correct and restart.");
            enabled = false;
            return;
        }
    }

    [Header("Focus")]
    public float hToChar = 10f;
    public float dToChar = 10f;
    public float aToWorld = 10f;
    public float distanceUpdateSpeed = 0.1f;

    private Vector3 lastTarPos = Vector3.zero;
    public float deltaPosDiffer = 0.1f;

    private void FixedUpdate()
    {
        if (!useUpdate) return;

        Vector3 pos = target.transform.position + new Vector3(dToChar * Mathf.Sin(aToWorld), hToChar, dToChar * Mathf.Cos(aToWorld));

        if ((pos - camera.transform.position).magnitude > deltaPosDiffer)
        // Only update follow camera if we moved sufficiently
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos, Time.deltaTime * distanceUpdateSpeed);
            camera.transform.forward = target.transform.position - pos;
        }

    }
}
