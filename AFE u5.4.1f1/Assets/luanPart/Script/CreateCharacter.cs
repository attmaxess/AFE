﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{   
    [Header("Params")] 
    public bool boolCreatePlaneJoystick = false;
    public string characterPrefab = string.Empty;

    /// <summary>
    /// Local
    /// </summary>
    GameManagerArVik gameManagerArVik = null;

    [ContextMenu("ClickSpawn")]
    public void ClickSpawn()
    {
        if (gameManagerArVik == null)
        {
            gameManagerArVik = GameObject.FindObjectOfType<GameManagerArVik>();
        }

        if (gameManagerArVik != null)
        {
            gameManagerArVik.prefabName = characterPrefab;
        }

        bool isSpawn = false;
        var aa = GameObject.FindObjectsOfType<ThirdPersonNetworkARVik>();
        for (int i = 0; i < aa.Length; i++)
        {
            Debug.Log("a[i].gameObject " + aa[i].gameObject.GetPhotonView().IsMine);
            if (aa[i].gameObject.GetPhotonView().IsMine)
            {
                isSpawn = true;
                return;
            }
        }

        gameManagerArVik.prefabName = characterPrefab;
        gameManagerArVik.SpawnObject(Vector3.zero, null);
    }

    [ContextMenu("SpawnObjectAtZero")]
    public void SpawnObjectAtZero(float offsetY = 0.5f)
    {
        var currentCharacter = GameObject.FindObjectsOfType<ThirdPersonNetworkARVik>();
        for (int i = 0; i < currentCharacter.Length; i++)
        {
            Debug.Log("a[i].gameObject " + currentCharacter[i].gameObject.GetPhotonView().IsMine);
            if (currentCharacter[i].gameObject.GetPhotonView().IsMine)
            {
                return;
            }
        }

        var newChar = PhotonNetwork.Instantiate(characterPrefab, new Vector3(0, offsetY, 0), Quaternion.identity, 0);
        if (boolCreatePlaneJoystick) GameObject.Instantiate(Resources.Load("PlaneJoystick"), Vector3.zero, Quaternion.identity);
    }
}
