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
                instance = FindObjectOfType<CanvasJoystickManager>();
            }

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

    [Header("Canvas")]
    public bool ActiveAfterAwake = false;
    public Transform canvas = null;

    private void Awake()
    {
        Singleton = this;
        if (!ActiveAfterAwake) canvas.gameObject.SetActive(false);
    }

    private void Destroy()
    {
        Singleton = null;
    }
    #endregion

    private int viewIdCharacter;

    public Transform barPlayer;

    public void SetViewIdCharacter(int id)
    {
        viewIdCharacter = id;
    }

    public void ShowCanvas()
    {
        canvas.gameObject.SetActive(true);
    }

    public void HideCanvas(int id)
    {
        if (viewIdCharacter == id)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
