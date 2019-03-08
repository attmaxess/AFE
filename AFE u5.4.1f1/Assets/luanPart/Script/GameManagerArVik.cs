using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // ArkitUIManager = GameObject.FindObjectOfType<ArkitUIManager>();
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

    void SpawnObject(Vector3 pos, GameObject hitObject, string prefabName)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = Random.Range(0, 2) == 0;//Axe
        enabledRenderers[1] = Random.Range(0, 2) == 0; ;//Shield

        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;

        var newChar = PhotonNetwork.Instantiate(prefabName, pos, hitObject.transform.rotation, 0, objs);
        listCharacter.Add(newChar.GetPhotonView());

    }

    [PunRPC]
    public void RpcSpawnObject(Vector3 pos, GameObject hitObject)
    {
        SpawnObject(pos, hitObject, prefabName);
    }
    [PunRPC]
    public void DestroyObj()
    {
        var charMine = GetIsMineChar();
        if (charMine != null)
            Destroy(charMine);
    }

    void Destroy(PhotonView photonView)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(photonView);
        }
        listCharacter.Remove(photonView);
    }

}