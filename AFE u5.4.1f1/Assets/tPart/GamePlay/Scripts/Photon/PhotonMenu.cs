using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class PhotonMenu : MonoBehaviour, ILobbyCallbacks, IConnectionCallbacks, IPunObservable
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Pun Inputs")]
    public PhotonView photonView = null;    

    [Header("Pun Params")]
    public string lobbyName = "myLobby";
    public string roomName = "myRoom";
    public string nickName = string.Empty;
    private Vector2 scrollPos = Vector2.zero;

    void Awake()
    {
#if DEMO_ALPHA1
        lobbyName = "lobby_Demo_Alpha1";
        roomName = "room_Demo_Alpha1";
#endif

        _ConnectUsingSettings();
    }    

    [Header("OnGUI")]
    public bool DrawInfo = false;

    void OnGUI()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }

        if (PhotonNetwork.CurrentRoom != null)
            return; //Only when we're not in a Room

        if (DrawInfo)
        {
            GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

            GUILayout.Label("Main Menu");

            //Player name
            GUILayout.BeginHorizontal();
            GUILayout.Label("Player name:", GUILayout.Width(150));
            PhotonNetwork.NickName = GUILayout.TextField(PhotonNetwork.NickName);
            if (GUI.changed)//Save name
                PlayerPrefs.SetString("playerName", PhotonNetwork.NickName);
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            //Join room by title
            GUILayout.BeginHorizontal();
            GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
            roomName = GUILayout.TextField(roomName);
            if (GUILayout.Button("GO"))
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            GUILayout.EndHorizontal();

            //Create a room (fails if exist!)
            GUILayout.BeginHorizontal();
            GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
            roomName = GUILayout.TextField(roomName);
            if (GUILayout.Button("GO"))
            {
                // using null as TypedLobby parameter will also use the default lobby
                PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);
            }
            GUILayout.EndHorizontal();

            //Join random room
            GUILayout.BeginHorizontal();
            GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));
            if (roomList.Count == 0)
            {
                GUILayout.Label("..no games available...");
            }
            else
            {
                if (GUILayout.Button("GO"))
                {
                    PhotonNetwork.JoinRandomRoom();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(30);
            GUILayout.Label("ROOM LISTING:");
            if (roomList.Count == 0)
            {
                GUILayout.Label("..no games available..");
            }
            else
            {
                //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                foreach (RoomInfo game in roomList)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(game.Name + " " + game.PlayerCount + "/" + game.MaxPlayers);
                    if (GUILayout.Button("JOIN"))
                    {
                        PhotonNetwork.JoinRoom(game.Name);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }

            GUILayout.EndArea();
        }
    }

    [Header("Show On Lost Connection")]
    public Canvas canvasPhoton = null;

    void ShowConnectingGUI()
    {
        if (!canvasPhoton.gameObject.activeSelf) canvasPhoton.gameObject.SetActive(true);

        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }

    #region custom method

    [Header("Disconnect")]
    public bool doneDisconnect = true;

    [ContextMenu("Disconnect")]
    public void Disconnect()
    {
        StartCoroutine(C_Disconnect());
    }

    IEnumerator C_Disconnect()
    {
        if (isDebug) Debug.Log("Start C_Disconnect");
        doneDisconnect = false;

        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        yield return new WaitUntil(() => PhotonNetwork.InRoom == false);

        if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
        yield return new WaitUntil(() => PhotonNetwork.InLobby == false);

        PhotonNetwork.Disconnect();

        if (isDebug) Debug.Log("Done C_Disconnect");
        doneDisconnect = true;

        yield break;
    }

    [ContextMenu("_ConnectUsingSettings")]
    public void _ConnectUsingSettings()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings(); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        nickName = PlayerPrefs.GetString("playerName", "Guest" + UnityEngine.Random.Range(1, 9999));
        PhotonNetwork.NickName = nickName;
    }

    [Header("ReConnectPhotonAlpha1")]
    public bool doneReConnectPhotonAlpha1 = true;

    [ContextMenu("ReConnectPhotonAlpha1")]
    public void ReConnectPhotonAlpha1()
    {
        StartCoroutine(C_ReConnectPhotonAlpha1());
    }

    IEnumerator C_ReConnectPhotonAlpha1()
    {
        if (isDebug) Debug.Log("Start C_ReConnectPhotonAlpha1");
        doneReConnectPhotonAlpha1 = false;

        _ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.IsConnected == true);

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = lobbyName;
        typedLobby.Type = LobbyType.Default;

        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby(typedLobby);
        yield return new WaitUntil(() => PhotonNetwork.CurrentLobby != null);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        bool canJoin = PhotonNetwork.JoinRoom(roomName);
        float momentJoin = Time.time;
        yield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentJoin > 2f);

        if (Time.time - momentJoin > 2f && canJoin == false)
        {
            if (isDebug) Debug.Log("Cant join!! Create!!");

            //bool canbeMaster = PhotonNetwork.SetMasterClient(photonView);

            bool canCreated = PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
            float momentCreated = Time.time;
            yield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentCreated > 2f);

            if (Time.time - momentCreated > 2f && canCreated == false)
            {
                if (isDebug) Debug.Log("Cant create!! Try Joint!!");

            }
        }

        if (isDebug) Debug.Log("Done C_ReConnectPhotonAlpha1");
        doneReConnectPhotonAlpha1 = true;

        yield break;
    }

    [ContextMenu("LeaveLobby")]
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    [ContextMenu("JoinCurrentRoom")]
    public void JoinCurrentRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }    

    [ContextMenu("LeaveRoom")]
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [ContextMenu("CreateCurrentRoom")]
    public void CreateCurrentRoom()
    {
        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = lobbyName;
        typedLobby.Type = LobbyType.Default;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        bool canCreated = PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
    }        

    [ContextMenu("DebugPlayerListInCurrentLobby")]
    public void DebugPlayerListInCurrentLobby()
    {
        //Player[] playerList = PhotonNetwork.play;
        //for (int i = 0; i < playerList.Length; i++)
        //{
        //    Debug.Log(playerList[i].NickName);
        //}
    }

    [ContextMenu("DebugCurrentPhoton")]
    public void DebugCurrentPhoton()
    {
        Debug.Log(PhotonNetwork.NickName);
        Debug.Log(PhotonNetwork.CurrentLobby.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

    [ContextMenu("DebugRoomList")]
    public void DebugRoomList()
    {
        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = lobbyName;
        typedLobby.Type = LobbyType.Default;

        Debug.Log(PhotonNetwork.CountOfRooms);
            //(typedLobby, "C0 >= 0 AND C0 < 50");
    }

    #endregion custom method

    #region pun callback

    public void OnConnectedToMaster()
    {
        // this method gets called by PUN, if "Auto Join Lobby" is off.
        // this demo needs to join the lobby, to show available rooms!

        if (isDebug) Debug.Log("OnConnectedToMaster " + PhotonNetwork.MasterClient.NickName);

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = lobbyName;
        typedLobby.Type = LobbyType.Default;

        PhotonNetwork.JoinLobby(typedLobby);  // this joins the "default" lobby
    }

    public void OnJoinedLobby()
    {
        if (isDebug) Debug.Log("OnJoinedLobby " + PhotonNetwork.CurrentLobby.Name);
    }

    public void OnLeftLobby()
    {
        if (isDebug) Debug.Log("OnLeftLobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (isDebug) Debug.Log("OnRoomListUpdate : " + roomList.Count);
        this.roomList = roomList;
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        if (isDebug) Debug.Log("OnLobbyStatisticsUpdate : " + lobbyStatistics.Count);
    }

    public void OnConnected()
    {
        if (isDebug) Debug.Log("OnConnected ");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        if (isDebug) Debug.Log("OnDisconnected " + cause.ToString());
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        if (isDebug) Debug.Log("OnRegionListReceived " + regionHandler.BestRegion.ToString());
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        if (isDebug) Debug.Log("OnCustomAuthenticationResponse " + data.Count);        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        if (isDebug) Debug.Log("OnCustomAuthenticationFailed " + debugMessage);        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ///Do the Serialize here
    }

    #endregion pun callback

    public List<RoomInfo> roomList = new List<RoomInfo>();
}
