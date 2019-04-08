using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class btnReconnectPhotonAlpha1 : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    //[ContextMenu("ReloadPhoton")]
    //public void ReloadPhoton()
    //{
    //    StartCoroutine(C_ReloadPhoton());
    //}

    //IEnumerator C_ReloadPhoton()
    //{
    //    if (!PhotonNetwork.IsConnected)
    //        PhotonNetwork.ConnectUsingSettings();         
                
    //    PhotonNetwork.NickName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

    //    yield return new WaitUntil(() => PhotonNetwork.IsConnected == true);

    //    PhotonNetwork.JoinRoom("room_Demo_Alpha1");

    //    PhotonNetwork.JoinOrCreateRoom("room_Demo_Alpha1", new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);
    //    yield return new WaitUntil(() => PhotonNetwork.InRoom == true);

    //    Debug.Log("In room " + PhotonNetwork.CurrentRoom.Name);

    //    yield break;
    //}    
}
