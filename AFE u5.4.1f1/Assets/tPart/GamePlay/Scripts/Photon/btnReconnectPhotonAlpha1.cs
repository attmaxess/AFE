using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnReconnectPhotonAlpha1 : MonoBehaviour
{
    [ContextMenu("ReloadPhoton")]
    public void ReloadPhoton()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.PlayerTtl = 999999999;
        roomOptions.EmptyRoomTtl = 999999999;
        roomOptions.CleanupCacheOnLeave = true;

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = "lobby_Demo_Alpha1";

        Debug.Log("Joining room room_Demo_Alpha1 " + PhotonNetwork.JoinOrCreateRoom("room_Demo_Alpha1", roomOptions, typedLobby));
    }
}
