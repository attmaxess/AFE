using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : MonoBehaviour
{
    public static GoHome instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
}
