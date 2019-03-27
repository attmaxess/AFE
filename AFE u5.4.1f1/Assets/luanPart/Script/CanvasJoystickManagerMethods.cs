using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasJoystickManagerMethods : MonoBehaviour
{
    [ContextMenu("SetCanvasOn")]
    public void SetCanvasOn()
    {
        CanvasJoystickManager.Singleton.canvas.gameObject.SetActive(true);
    }

    [ContextMenu("SetCanvasOff")]
    public void SetCanvasOff()
    {
        CanvasJoystickManager.Singleton.canvas.gameObject.SetActive(false);
    }

    [ContextMenu("ToggleCanvas")]
    public void ToggleCanvas()
    {
        CanvasJoystickManager.Singleton.canvas.gameObject.SetActive(!CanvasJoystickManager.Singleton.canvas.gameObject.activeSelf);
    }
    
}
