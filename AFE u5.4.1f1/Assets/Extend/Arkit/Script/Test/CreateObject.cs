using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {
	[SerializeField] GameObject prefabCube;
	// Use this for initialization
	void Start () {
		
	}
	public void ShowCube(Vector3 pos, GameObject hitObject){
		GameObject obj = CreateModel3D(prefabCube, pos, hitObject.transform.rotation);
		obj.transform.localScale = Vector3.one * .3f;
	}
	GameObject CreateModel3D(GameObject prefab, Vector3 v, Quaternion quater, Transform parent = null){
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
