using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_InCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnBecameVisible()
    {
        Debug.Log("OnBecame Visible");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("OnBecame Invisible");
    }

    private void OnAnimatorMove()
    {
        Debug.Log("OnAnimatorMove");
    }
}
