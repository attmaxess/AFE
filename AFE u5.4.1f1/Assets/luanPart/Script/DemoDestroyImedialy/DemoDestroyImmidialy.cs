using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDestroyImmidialy : MonoBehaviour {

    private void Awake()
    {
        DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
