using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ARKitHitTesting;
using Photon.Pun;
using UnityEngine.UI;

namespace AFE.BaseGround
{

    // Classes to hold model information

    // Main Class for Managing Models

    public class AFEBaseMarkerManager : MonoBehaviour
    {
        [Header("Debug")]
        public bool isDebug = true;
        [SerializeField]
        bool _useUpdate = false;
        public bool useUpdate { get { return _useUpdate; } set { _useUpdate = value; OnUseUpdateChanged(); } }

        [Header("Background Marker")]
        public Animator animBtnPlaceBgMarker = null;

        [ContextMenu("OnUseUpdateChanged")]
        public void OnUseUpdateChanged()
        {
            animBtnPlaceBgMarker.SetBool("isPlacing", useUpdate);
        }

        [Header("Inputs")]

        public GameObject backgroundMarkerPrefab = null;

        BackgroundMarker _currentBackgroundMarker = null;
        BackgroundMarker currentBackgroundMarker
        {
            get
            {
                if (_currentBackgroundMarker == null) _currentBackgroundMarker = FindObjectOfType<BackgroundMarker>();
                if (_currentBackgroundMarker == null) _currentBackgroundMarker = Instantiate(backgroundMarkerPrefab, Vector3.zero, Quaternion.identity).GetComponent<BackgroundMarker>();
                return _currentBackgroundMarker;
            }
        }

        // Update function checks for hittest
        void Update()
        {
            if (!useUpdate) return;
            // for hit testing on the device

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        Debug.Log("Not touching a UI button. Moving on.");

                        // add new shape
                        var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                        ARPoint point = new ARPoint
                        {
                            x = screenPosition.x,
                            y = screenPosition.y
                        };

                        // prioritize reults types
                        ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane,
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
                    }
                }
            }
        }

        // The HitTest to Add a Marker

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
                    ModelInfo modelInfo = new ModelInfo();
                    modelInfo.px = hitPosition.x;
                    modelInfo.py = hitPosition.y;
                    modelInfo.pz = hitPosition.z;

                    PlacePhotonGround(modelInfo);

                    return true;
                }
            }
            return false;
        }

        public void PlacePhotonGround(ModelInfo modelInfo)
        {
            if (currentBackgroundMarker == null) return;

            if (LibPlacenote.Instance.GetStatus() != LibPlacenote.MappingStatus.RUNNING)
            {
                return;
            }

            LibPlacenote.PNFeaturePointUnity[] map = LibPlacenote.Instance.GetMap();

            if (map == null || map.Length == 0)
            {
                return;
            }

            if (isDebug) Debug.Log("Find point to snap to!!");

            Vector3 modelPos = new Vector3(modelInfo.px, modelInfo.py, modelInfo.pz);

            float minDistance = Mathf.Infinity;
            Vector3 posAtMinDistance = -Vector3.one;

            for (int i = 0; i < map.Length; i++)
            {
                Vector3 pointPos = new Vector3(map[i].point.x, map[i].point.y, -map[i].point.z);
                float distance = (pointPos - modelPos).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    posAtMinDistance = pointPos;
                }
            }

            currentBackgroundMarker.transform.position = posAtMinDistance;
            useUpdate = false;
        }


        // Helper Functions to convert models to and from JSON

        public JObject BackgroundMarker2JSON()
        {
            if (currentBackgroundMarker == null) return null;

            ModelInfo newInfo = new ModelInfo();
            Vector3 groundPos = currentBackgroundMarker.transform.position;
            newInfo.px = groundPos.x; newInfo.py = groundPos.y; newInfo.pz = groundPos.z;

            ModelList modelList = new ModelList();
            modelList.models = new ModelInfo[1];
            modelList.models[0] = newInfo;

            return JObject.FromObject(modelList);
        }

        public void LoadBackgroundMarkerFromJSON(JToken mapMetadata)
        {
            if (currentBackgroundMarker == null) return;

            if (mapMetadata is JObject && mapMetadata["backgroundMarker"] is JObject)
            {
                ModelList modelList = mapMetadata["backgroundMarker"].ToObject<ModelList>();
                if (modelList.models == null || modelList.models.Length == 0)
                {
                    Debug.Log("no models added");
                    return;
                }

                Vector3 groundPos = new Vector3(modelList.models[0].px, modelList.models[0].py, modelList.models[0].pz);
                currentBackgroundMarker.transform.position = groundPos;
            }
        }

        #region local method

        [ContextMenu("Show")]
        public void Show()
        {
            currentBackgroundMarker.Show();
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            currentBackgroundMarker.Hide();
        }

        [ContextMenu("ToggleShowHide")]
        public void ToggleShowHide()
        {
            currentBackgroundMarker.ToggleShowHide();
        }

        #endregion local method
    }
}