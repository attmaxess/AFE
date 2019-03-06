using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ArkitUIManager : Photon.PunBehaviour {
	[System.Serializable]
    public class ArkitFoundEvent : UnityEvent<ScanValues> { }
	public ArkitManager arkitManager;
	public GameObject foundImagePlane;
	//public GameObject headerScan;
	public GameObject textScan;
	bool isFoundFloor = false;
	[SerializeField]
	private ArkitFoundEvent OnFoundFloorEvent;


    void OnJoinedRoom()
    {
        SetActiveSomething(false);
        arkitManager.StartScanPlane(OnFoundFloor);
        if (UseForTest.Instance.IsTest)
        {
            Invoke("test", UseForTest.Instance.TimeEditor);
        }
    }

    // Use this for initialization
    void Start () {
		/*SetActiveSomething (false);
		arkitManager.StartScanPlane (OnFoundFloor);
		if (UseForTest.Instance.IsTest) {
			Invoke ("test", UseForTest.Instance.TimeEditor);
		}     */
	}
	void test () {
		OnFoundFloor (ScanValues.GetData (Vector3.one, Quaternion.identity));
	}

	void OnFoundFloor (ScanValues value) {
		if (!isFoundFloor) {
			isFoundFloor = true;
			SetActiveSomething (true);
			GameObject.FindObjectOfType<CreatePlane100m> ().CreateFloor (value.position, value.rotation, "Floor");
			GameObject.FindObjectOfType<ArkitFocusSquare> ().StartScan ();
			// Call main game in here
			if(OnFoundFloorEvent != null){
				OnFoundFloorEvent.Invoke(value);
			}
		}
	}
	void SetActiveSomething (bool isActive) {
		foundImagePlane.SetActive (isActive);
		//headerScan.SetActive(!isActive);
		textScan.SetActive (!isActive);
	}
}