using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
public class TapOnScreen : MonoBehaviour {
    [System.Serializable]
    public class ArkitTapEvents : UnityEvent<Vector3, GameObject> { }

    [SerializeField]
    public ArkitTapEvents OnTapToPlane100m = null;
    [SerializeField]
    public ArkitTapEvents OnTapOneModel = null;
    void Update () {
        if (Input.GetButtonDown ("Fire1")) {
            if (IsPointerOverUIObject ()) {
                return;
            }
            Vector3 pos = new Vector3 (Screen.width / 2, Screen.height / 2, 0);
            RayCastExtend.StartRay (OnCheckRay, Input.mousePosition);
        }

    }
    void OnCheckRay (RaycastHit hit) {
        Debug.Log (hit.collider.name + " ------------");
        if (hit.collider.name.IndexOf ("Floor") != -1 && OnTapToPlane100m != null) {
            OnTapToPlane100m.Invoke (hit.point, hit.collider.gameObject);
            Debug.Log ("OnTapToPlane100m  ------------");
            return;
        }
        if (hit.collider.name.IndexOf ("CubeTest") != -1 && OnTapOneModel != null) {
            OnTapOneModel.Invoke (hit.point, hit.collider.gameObject);
            Debug.Log ("OnTapOneModel ------------");
            return;
        }
    }
    private bool IsPointerOverUIObject () {
        PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}