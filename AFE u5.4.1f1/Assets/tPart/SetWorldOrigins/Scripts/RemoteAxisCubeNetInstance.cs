using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAxisCubeNetInstance : Photon.MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
    public string playerPrefabName = "RemoteAxisCube";

    void OnJoinedRoom()
    {
        if (isDebug) Debug.Log("RemoteAxisCubeNetInstance OnJoinedRoom");
        StartGame();
    }

    IEnumerator OnLeftRoom()
    {
        if (isDebug) Debug.Log("RemoteAxisCubeNetInstance OnLeftRoom");
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.room != null || PhotonNetwork.connected == false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void StartGame()
    {
        if (isDebug) Debug.Log("RemoteAxisCubeNetInstance StartGame");

        // Spawn our local player
        PhotonNetwork.Instantiate(this.playerPrefabName, transform.position, Quaternion.identity, 0);
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("RemoteAxisCubeNetInstance OnDisconnectedFromPhoton");
    }
}
