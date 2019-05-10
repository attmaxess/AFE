using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacenoteSampleTerrain : MonoBehaviour
{
    public bool ShowOnEditorAtAwake = true;

    void Awake()
    {
        if (ShowOnEditorAtAwake) DoAwake();
    }

    [ContextMenu("DoAwake")]
    public void DoAwake()
    {
#if UNITY_EDITOR        
            gameObject.transform.GetChild(0).gameObject.SetActive(ShowOnEditorAtAwake);
            gameObject.transform.GetChild(1).gameObject.SetActive(ShowOnEditorAtAwake);        
#else
		    gameObject.transform.GetChild(0).gameObject.SetActive(false);
		    gameObject.transform.GetChild(1).gameObject.SetActive(false);
#endif

    }
}
