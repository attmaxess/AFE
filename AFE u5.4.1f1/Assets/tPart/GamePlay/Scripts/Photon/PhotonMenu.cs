using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;

public class PhotonMenu : MonoBehaviour, ILobbyCallbacks, IConnectionCallbacks, IPunObservable
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Pun Inputs")]
    public string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;

    void Awake()
    {
#if DEMO_ALPHA1
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

    [ContextMenu("Disconnect")]
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    [ContextMenu("_ConnectUsingSettings")]
    public void _ConnectUsingSettings()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings(); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.NickName = PlayerPrefs.GetString("playerName", "Guest" + UnityEngine.Random.Range(1, 9999));
    }

    [ContextMenu("ReConnectPhotonAlpha1")]
    public void ReConnectPhotonAlpha1()
    {
        StartCoroutine(C_ReConnectPhotonAlpha1());
    }

    IEnumerator C_ReConnectPhotonAlpha1()
    {
        _ConnectUsingSettings();
        yield return new WaitUntil(() => PhotonNetwork.IsConnected == true && PhotonNetwork.CurrentLobby != null);        

        bool canCreated = PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);
        float momentCreated = Time.time;
        yield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentCreated > 2f);

        if (Time.time - momentCreated > 2f && canCreated == false)
        {
            if (isDebug) Debug.Log("Cant create!! Try Joint!!");
            bool canJoin = PhotonNetwork.JoinRoom(roomName);
            float momentJoin = Time.time;
            yield return new WaitUntil(() => (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomName) || Time.time - momentJoin > 2f);

            if (Time.time - momentCreated > 2f && canJoin == false)
            {
                if (isDebug) Debug.Log("Cant join!! Break!!");
            }
        }

        yield break;
    }

    #endregion custom method

    #region pun callback

    public void OnConnectedToMaster()
    {
        // this method gets called by PUN, if "Auto Join Lobby" is off.
        // this demo needs to join the lobby, to show available rooms!

        if (isDebug) Debug.Log("OnConnectedToMaster " + PhotonNetwork.MasterClient.NickName);
        PhotonNetwork.JoinLobby();  // this joins the "default" lobby
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
