using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasJoystickManager : MonoBehaviour
{
    #region singleton
    static CanvasJoystickManager instance = null;

    public static CanvasJoystickManager Singleton
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Dont Have Instance of Canvas Joystick Manager");
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        Singleton = this;
    }


    private void Destroy()
    {
        Singleton = null;
    }
    #endregion

    public Transform barPlayer;
}
