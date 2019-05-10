using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CanvasTribalWorld_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float distanceZ = 10;

    [Header("Input")]
    public MoveTo moveTo = null;
    public FaceTo faceTo = null;

    public bool doneZoom = true;

    [ContextMenu("Zoom0")]
    public void Zoom0()
    {
        Zoom(Vector3.zero, Quaternion.identity);
    }

    [ContextMenu("ZoomCamAndDistance")]
    public void ZoomCamAndDistance()
    {

#if UNITY_EDITOR

        RaycastHit hit;
        Vector3 screenPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out hit))
        {

            // create model info object
            ARKitHitTesting.ModelInfo modelInfo = new ARKitHitTesting.ModelInfo();
            modelInfo.px = hit.point.x;
            modelInfo.py = hit.point.y;
            modelInfo.pz = hit.point.z;

            Transform cam = Camera.main.transform;
            Zoom(new Vector3(modelInfo.px, modelInfo.py, modelInfo.pz), cam.eulerAngles.y, cam.eulerAngles.z);

        }

#else

        Vector3 screenPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var screenPosition = Camera.main.ScreenToViewportPoint(screenPos);

        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // prioritize reults types
        ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane,
                        ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane,
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
        };

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            if (HitTestWithResultType(point, resultType))
            {
                Debug.Log("Found a hit test result");
                return;
            }
        }

#endif
    }

    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);

        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                Debug.Log("Got hit!");

                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                Quaternion rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);

                //Transform to placenote frame of reference (planes are detected in ARKit frame of reference)
                Matrix4x4 worldTransform = Matrix4x4.TRS(position, rotation, Vector3.one);
                Matrix4x4? placenoteTransform = LibPlacenote.Instance.ProcessPose(worldTransform);

                Vector3 hitPosition = PNUtility.MatrixOps.GetPosition(placenoteTransform.Value);
                Quaternion hitRotation = PNUtility.MatrixOps.GetRotation(placenoteTransform.Value);

                // create model info object
                ARKitHitTesting.ModelInfo modelInfo = new ARKitHitTesting.ModelInfo();
                modelInfo.px = hitPosition.x;
                modelInfo.py = hitPosition.y;
                modelInfo.pz = hitPosition.z;

                //Vector3 pos = cam.position + cam.forward.normalized * distanceZ;
                Transform cam = Camera.main.transform;
                Zoom(new Vector3(modelInfo.px, modelInfo.py, modelInfo.pz), cam.eulerAngles.y, cam.eulerAngles.z);

                return true;
            }
        }
        return false;
    }    

    public void Zoom(Vector3 pos, Quaternion qua)
    {
        StopAllC();
        StartCoroutine(C_Zoom(pos, qua));
    }

    IEnumerator C_Zoom(Vector3 pos, Quaternion qua)
    {
        doneZoom = false;

        moveTo.MoveToPos(pos);
        faceTo.ToQua(qua);

        yield return new WaitUntil(() => moveTo.doneMoveTo == true && faceTo.doneToQua == true);

        doneZoom = true;
        yield break;
    }

    public void Zoom(Vector3 pos, float toY, float toZ)
    {
        StopAllC();
        StartCoroutine(C_Zoom(pos, toY, toZ));
    }

    IEnumerator C_Zoom(Vector3 pos, float toY, float toZ)
    {
        doneZoom = false;

        moveTo.MoveToPos(pos);
        faceTo.ToEurY(toY);
        faceTo.ToEurZ(toZ);

        yield return new WaitUntil(() => moveTo.doneMoveTo == true && faceTo.doneToEurY == true && faceTo.doneToEurZ == true);

        doneZoom = true;
        yield break;
    }

    [ContextMenu("StopAllC")]
    public void StopAllC()
    {
        StopAllCoroutines();
    }
}
