﻿using System.Collections;
using System.Collections.Generic;
using DHT.TextureToString64;
using UnityEngine;

public class btnToStringFile : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public TextureToBase64Methods textureMethod = null;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        textureMethod.ToString64File();
    }
}
