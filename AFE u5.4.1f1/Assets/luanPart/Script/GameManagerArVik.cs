using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;

public class GameManagerArVik : Photon.PunBehaviour
{
    public string prefabName = "VikingPrefab";
    public ArkitUIManager ArkitUIManager;
    bool isJoinedRoom = false;
    public List<PhotonView> listCharacter = new List<PhotonView>();

    public PhotonView GetIsMineChar()
    {
        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (listCharacter[i].isMine)
            {
                return listCharacter[i];
            }
        }
        return null;
    }

    private void Awake()
    {

    }

    IEnumerator OnLeftRoom()
    {

        Debug.Log("OnLeftRoom");

        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.room != null || PhotonNetwork.connected == false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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

    public void SpawnObject(Vector3 pos, GameObject hitObject)
    {
        bool isSpawn = false;
        var aa = GameObject.FindObjectsOfType<ThirdPersonNetworkARVik>();
        for (int i = 0; i < aa.Length; i++)
        {
            Debug.Log("a[i].gameObject " + aa[i].gameObject.GetPhotonView().isMine);
            if (aa[i].gameObject.GetPhotonView().isMine)
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