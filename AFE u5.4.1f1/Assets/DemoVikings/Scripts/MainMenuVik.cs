using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class MainMenuVik : MonoBehaviour, ILobbyCallbacks
{

    void Awake()
    {
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings(); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.NickName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

        //Set camera clipping for nicer "main menu" background
        // Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;

    }

    private string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;

    void OnGUI()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }


        if (PhotonNetwork.CurrentRoom != null)
            return; //Only when we're not in a Room


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


    void ShowConnectingGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }

    public void OnConnectedToMaster()
    {
        // this method gets called by PUN, if "Auto Join Lobby" is off.
        // this demo needs to join the lobby, to show available rooms!

        PhotonNetwork.JoinLobby();  // this joins the "default" lobby
    }

    public void OnJoinedLobby()
    {
    }

    public void OnLeftLobby()
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    public List<RoomInfo> roomList = new List<RoomInfo>();
}
