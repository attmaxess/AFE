using System;
using UnityEngine;
public interface IArkitManager {
	void StartScanPlane (Action<ScanValues> callbackOnFoundPlane);
	void StopScanPlane ();
}