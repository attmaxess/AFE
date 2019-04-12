using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using Com.Beetsoft.AFE;

public class PhotonMenu : MonoBehaviourPunCallbacks
{
    [Header("Debug")]
    public bool isDebug = true;

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
    }

    [Header("ConstantlyCheckConnection")]
    public float deltaCheck = 2f;

    public void ConstantlyCheckConnection()
    {
        StartCoroutine(C_ConstantlyCheckConnection());
    }

    IEnumerator C_ConstantlyCheckConnection()
    {
        while (true)
        {
            CheckConnecting();
            yield return new WaitForSeconds(deltaCheck);
        }
    }

    //[Header("OnGUI")]    
    //public bool DrawOnGUI = false;  \

    [Header("Show On Lost Connection")]
    public bool isDebugConnecting = false;
    public Canvas canvasPhoton = null;
    public GameStateEqual stateArKitPlaceNote = null;

    void CheckConnecting()
    {
#if UNITY_EDITOR
        if (isDebugConnecting) Debug.Log("CheckConnecting");
#endif

        stateArKitPlaceNote.OnClick();
        if (stateArKitPlaceNote.resultEqual == true)
        {
            if (!PhotonNetwork.IsConnected
            || PhotonNetwork.CurrentLobby == null || PhotonNetwork.CurrentLobby.Name != lobbyName
            || PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Name != roomName)
            {
                if (!canvasPhoton.gameObject.activeSelf)
                    canvasPhoton.gameObject.SetActive(true);
            }
            else
            {
                if (canvasPhoton.gameObject.activeSelf)
                    canvasPhoton.gameObject.SetActive(false);
            }
        }
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

    [Header("ConnectUsingSettings")]
    public bool doneConnectUsingSettings = true;

    [ContextMenu("ConnectUsingSettings")]
    public void ConnectUsingSettings()
    {
        StartCoroutine(C_ConnectUsingSettings());
    }

    IEnumerator C_ConnectUsingSettings()
    {
        doneConnectUsingSettings = false;

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings(); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        nickName = PlayerPrefs.GetString("playerName", "Guest" + UnityEngine.Random.Range(1, 9999));
        PhotonNetwork.NickName = nickName;

        yield return new WaitUntil(() => PhotonNetwork.IsConnected == true);
        doneConnectUsingSettings = true;

        yield break;
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

        ConnectUsingSettings();
        yield return new WaitUntil(() => doneConnectUsingSettings == true);
        yield return new WaitForSeconds(1f);

        TypedLobby typedLobby = new TypedLobby();

        if (!PhotonNetwork.InLobby || PhotonNetwork.CurrentLobby.Name != lobbyName)
        {
            typedLobby = new TypedLobby();
            typedLobby.Name = lobbyName;
            typedLobby.Type = LobbyType.Default;
            PhotonNetwork.JoinLobby(typedLobby);

            yield return new WaitUntil(() => PhotonNetwork.CurrentLobby != null && PhotonNetwork.CurrentLobby.Name == lobbyName);
            yield return new WaitForSeconds(1f);
        }

        if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom.Name != roomName)
        {
            bool canCreated = false;
            float momentCreated = Time.time;

            bool canJoin = false;
            float momentJoin = Time.time;

            if (roomList.FindIndex((x) => x.Name == roomName) == -1)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 10;

                try
                {
                    canCreated = PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
                    momentCreated = Time.time;
                }
                catch
                {
                    doneReConnectPhotonAlpha1 = true;
                    yield break;
                }
            }
            else
            {
                try
                {
                    canJoin = PhotonNetwork.JoinRoom(roomName);
                    momentJoin = Time.time;
                }
                catch
                {
                    doneReConnectPhotonAlpha1 = true;
                    yield break;
                }
            }

            //yield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentCreated > 2f);
            //ield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentJoin > 2f);

            if (Time.time - momentJoin > 2f && (canJoin == false || canCreated == false))
            {
                if (isDebug) Debug.Log("Cant join or Create!!");
            }
        }

        yield return new WaitForSeconds(1f);

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

    public override void OnConnectedToMaster()
    {
        // this method gets called by PUN, if "Auto Join Lobby" is off.
        // this demo needs to join the lobby, to show available rooms!

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = lobbyName;
        typedLobby.Type = LobbyType.Default;

        PhotonNetwork.JoinLobby(typedLobby);  // this joins the "default" lobby
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (isDebug) Debug.Log("OnJoinedLobby " + PhotonNetwork.CurrentLobby.Name);
        CheckConnecting();
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        if (isDebug) Debug.Log("OnLeftLobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        if (isDebug) Debug.Log("OnRoomListUpdate : " + roomList.Count);
        this.roomList = roomList;
    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        base.OnLobbyStatisticsUpdate(lobbyStatistics);
        if (isDebug) Debug.Log("OnLobbyStatisticsUpdate : " + lobbyStatistics.Count);
    }

    public override void OnConnected()
    {
        base.OnConnected();
        if (isDebug) Debug.Log("OnConnected ");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (isDebug) Debug.Log("OnDisconnected " + cause.ToString());
        CheckConnecting();
    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        base.OnRegionListReceived(regionHandler);
        if (isDebug) Debug.Log("OnRegionListReceived " + regionHandler.BestRegion.ToString());
    }

    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        base.OnCustomAuthenticationResponse(data);
        if (isDebug) Debug.Log("OnCustomAuthenticationResponse " + data.Count);
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        base.OnCustomAuthenticationFailed(debugMessage);
        if (isDebug) Debug.Log("OnCustomAuthenticationFailed " + debugMessage);
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
        if (isDebug) Debug.Log("OnFriendListUpdate " + friendList.Count);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        if (isDebug) Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        if (isDebug) Debug.Log("OnCreateRoomFailed " + returnCode + ", " + message);
        CheckConnecting();
    }

    void AddOnceRoom(string roomName)
    {
        if (roomList.FindIndex((x) => x.Name == roomName) == -1)
        {

        }
        else
        {

        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (isDebug) Debug.Log("OnJoinedRoom");
        CheckConnecting();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        if (isDebug) Debug.Log("OnJoinRoomFailed" + returnCode + ", " + message);
        CheckConnecting();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        if (isDebug) Debug.Log("OnJoinRandomFailed" + returnCode + ", " + message);
        CheckConnecting();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (isDebug) Debug.Log("OnLeftRoom");
        CheckConnecting();
    }

    #endregion pun callback

    public override void OnEnable()
    {
        base.OnEnable();
        ConstantlyCheckConnection();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public List<RoomInfo> roomList = new List<RoomInfo>();

    [ContextMenu("MasterRPCMap")]
    public void MasterRPCMap()
    {
        if (isDebug) Debug.Log("RPC NameMap All");
        GetComponent<PhotonView>().RPC("RpcNameMap", RpcTarget.All, nameMap);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (isDebug) Debug.Log("OnPlayerEnteredRoom");
        GetComponent<PhotonView>().RPC("RpcNameMap", newPlayer, nameMap);
    }

    [Header("Handle New Map")]
    [SerializeField]
    string _nameMap = "";
    public string nameMap
    {
        get { return _nameMap; }
        set { _nameMap = value; HandleCurrentMapName(); }
    }

    public PlaceNote placeNote = null;

    void HandleCurrentMapName()
    {
        if (string.IsNullOrEmpty(_nameMap)) return;

#if UNITY_EDITOR
        _nameMap = "mapEditor";
#endif

        if (PhotonNetwork.IsMasterClient == false)
            placeNote.OnLoadMapClicked(_nameMap);
    }

    [PunRPC]
    public void RpcNameMap(string name)
    {
        nameMap = name;
    }
}
