using UnityEngine;

public class GameManagerArVik : Photon.PunBehaviour
{

    bool isJoinedRoom = false;

    public ArkitUIManager ArkitUIManager;

    private void Awake()
    {
        ArkitUIManager = GameObject.FindObjectOfType<ArkitUIManager>();
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
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (GUILayout.Button("Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
}