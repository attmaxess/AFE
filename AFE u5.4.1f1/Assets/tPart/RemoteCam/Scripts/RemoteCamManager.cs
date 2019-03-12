using UnityEngine;
using System.Collections;

public class RemoteCamManager : Photon.MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
    public string playerPrefabName = "RemoteCamPrefab";

    void OnJoinedRoom()
    {
        if (isDebug) Debug.Log("RemoteCamManager OnJoinedRoom");
        StartGame();
    }

    IEnumerator OnLeftRoom()
    {
        if (isDebug) Debug.Log("RemoteCamManager OnLeftRoom");
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.room != null || PhotonNetwork.connected == false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void StartGame()
    {
        if (isDebug) Debug.Log("RemoteCamManager StartGame");

        // Spawn our local player
        PhotonNetwork.Instantiate(this.playerPrefabName, transform.position, Quaternion.identity, 0);
    }

    void OnGUI()
    {
        //Chỗ này thêm code tạo nút bấm để bắt đầu truyền thông tin từ Camera.main dưới thiết bị lên host
        //if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        //if (GUILayout.Button("Leave Room"))
        //{
        //    PhotonNetwork.LeaveRoom();
        //}
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("RemoteCamManager OnDisconnectedFromPhoton");
    }
}
