using System;
using System.Collections;
using UnityEngine;

using UnityEngine.XR.iOS;
/// <summary>
/// Scan plane, if found --> create a plane 100m.
/// </summary>
public class ArkitScanPlane : ArkitManager {
	public GameObject planePrefab;
	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	public override void StartScanPlane (Action<ScanValues> callbackOnFoundPlane) {
		UnityARUtility.InitializePlanePrefab (planePrefab);
		base.StartScanPlane(callbackOnFoundPlane);
		Debug.Log("StartScanPlane ---");
	}
}