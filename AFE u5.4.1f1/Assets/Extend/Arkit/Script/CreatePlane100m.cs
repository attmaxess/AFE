using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane100m : MonoBehaviour {
	public GameObject prefabPlane100m;
	GameObject saveFloor;
	GameObject saveCeil;
	// Use this for initialization
	void Start () {

	}
	//Set Floor same position y with QRCode
	public void SetPlane100m (Vector3 p) {
		p.y = saveFloor.transform.position.y;
		saveFloor.transform.position = p;
	}
	//Scan Floor using Arkit
	public GameObject CreateFloor(Vector3 pos, Quaternion quaternion, string nameObj){
		saveFloor = CreatePlane (prefabPlane100m.gameObject, pos, quaternion);
		saveFloor.transform.localScale = Vector3.one * 100;
		saveFloor.name = nameObj;
		return saveFloor;
	}
	public Vector3 GetPosFloor(){
		return saveFloor.transform.position;
	}
	GameObject CreatePlane(GameObject prefab, Vector3 v, Quaternion quater, Transform parent = null){
		GameObject obj = Instantiate (prefab, v, quater) as GameObject;
		if (parent != null) {
			obj.transform.SetParent (parent);
			obj.transform.localEulerAngles = Vector3.zero;
		}
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = v;
		return obj;
	}
}