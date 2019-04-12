using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStatsInspector : MonoBehaviour
{
    [Header("Debug")]
    public bool useUpdate = false;

    [Header("Stats")]
    public bool photonConnected = false;
    public string currentLobby = string.Empty;
    public string currentRoom = string.Empty;
    public bool isMasterClient = false;

    private void Update()
    {
        if (!useUpdate) return;

        photonConnected = PhotonNetwork.IsConnected;
        currentLobby = PhotonNetwork.CurrentLobby == null ? "null" : PhotonNetwork.CurrentLobby.Name;
        currentRoom = PhotonNetwork.CurrentRoom == null ? "null" : PhotonNetwork.CurrentRoom.Name;
        isMasterClient = PhotonNetwork.IsMasterClient;
    }
}
