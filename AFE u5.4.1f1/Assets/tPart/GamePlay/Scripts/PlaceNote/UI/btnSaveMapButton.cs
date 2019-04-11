using DHT.TextureToString64;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnSaveMapButton : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Iput")]
    public PlaceNote placeNote = null;
    public BackgroundMarkerMethod backgroundMethod = null;

    public void OnClick()
    {
        StartCoroutine(C_OnClick());
    }

    IEnumerator C_OnClick()
    {
        placeNote.OnSaveMapClick();
        yield return new WaitUntil(() => placeNote.doneOnSaveMapClick == true);

        backgroundMethod.Hide();       

        yield break;
    }
}
