using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RayCastExtend : MonoBehaviour {
    public static void StartRay (UnityAction<RaycastHit> callBackRay, Vector3 pos) {
        CheckRay (callBackRay, pos);
    }

    static void CheckRay (UnityAction<RaycastHit> callBackRay, Vector3 pos) {
        // if(IsPointerOverUIObject()){
        //     return;
        // }
        //Vector3 pos = new Vector3 (Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay (pos);
        Debug.DrawRay (ray.origin, ray.direction * 100, new Color (1f, 0.922f, 0.016f, 1f));
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 100)) {
            if (callBackRay != null) {
                callBackRay (hit);
            }
        }
    }
    public static bool IsPointerOverUIObject () {
        PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}