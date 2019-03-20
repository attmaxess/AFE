﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;
using Photon.Pun;

public class GameManagerArVik : MonoBehaviourPunCallbacks
{
    public string prefabName = "VikingPrefab";
    public ArkitUIManager ArkitUIManager;
    bool isJoinedRoom = false;
    public List<PhotonView> listCharacter = new List<PhotonView>();

    public static GameManagerArVik instance = null;

    public event System.Action attack;
    public event System.Action skill1;
    public event System.Action skill2;
    public event System.Action skill3;
    public event System.Action skill4;

    public static GameManagerArVik Singleton
    {
        get
        {
            return instance;
        }
    }

    public PhotonView GetIsMineChar()
    {
        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (listCharacter[i].IsMine)
            {
                return listCharacter[i];
            }
        }
        return null;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
    }

    IEnumerator OnLeftRoom()
    {

        Debug.Log("OnLeftRoom");

        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.CurrentRoom != null || PhotonNetwork.IsConnected == false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {

        Debug.Log(CF2Input.GetButtonDown("BtnTest"));
        Debug.Log(CF2Input.GetButtonUp("BtnTest"));
        Debug.Log(CF2Input.GetButton("BtnTest"));   
        Debug.Log(CF2Input.GetAxis("HozTest"));
        Debug.Log(CF2Input.GetAxis("VerTest"));    
   /*     Debug.Log(CF2Input.GetAxis("Horizontal"));
        Debug.Log(CF2Input.GetAxis("Vertical"));  */
        if (!photonView.IsMine) return;

        if (CF2Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
        }
        if (CF2Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
            if (attack != null) attack();
        }

        if (CF2Input.GetButtonDown("Skill1"))
        {
            if (skill1 != null) skill1();
            Debug.Log("Skill1");
        }
        if (CF2Input.GetButtonDown("Skill2"))
        {
            if (skill2 != null) skill2();
            Debug.Log("Skill2");
        }
        if (CF2Input.GetButtonDown("Skill3"))
        {
            if (skill3 != null) skill3();
            Debug.Log("Skill3");
        }
        if (CF2Input.GetButtonDown("Skill4"))
        {
            if (skill4 != null) skill4();
            Debug.Log("Skill4");
        }
    }

    private void Start()
    {
        ArkitUIManager.gameObject.SetActive(isJoinedRoom);
    }

    public override void OnJoinedRoom()
    {
        isJoinedRoom = true;
        Debug.Log("OnJoinedRoom");
        ArkitUIManager.gameObject.SetActive(isJoinedRoom);
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }

    public void SpawnObject(Vector3 pos, GameObject hitObject)
    {
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

        var newChar = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity, 0);
        GameObject.Instantiate(Resources.Load("PlaneJoystick"), pos, Quaternion.identity);
        //  photonView.RPC("RpcSpawnObject", PhotonTargets.MasterClient, pos, prefabName);
    }

    [PunRPC]
    public void RpcSpawnObject(Vector3 pos, string prefabName)
    {

    }
}