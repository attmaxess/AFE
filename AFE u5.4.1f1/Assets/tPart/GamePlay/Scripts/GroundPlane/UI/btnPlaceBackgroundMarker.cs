using AFE.BaseGround;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnPlaceBackgroundMarker : MonoBehaviour
{
    [Header("Input")]
    public Animator anim = null;
    public AFEBaseMarkerManager baseMarker = null;

    public void OnClick()
    {
        baseMarker.useUpdate = !baseMarker.useUpdate;
    }
}
