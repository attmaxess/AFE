using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStatsInspector : MonoBehaviour
{
    [Header("Debug")]
    public bool useUpdate = false;
    public bool continueUpdateAtStart = false;

    [Header("Stats")]
    public bool photonConnected = false;
    public string currentLobby = string.Empty;
    public string currentRoom = string.Empty;
    public bool isMasterClient = false;

    private void Update()
    {
        if (!useUpdate) return;        
    }

    private void Start()
    {
        if (continueUpdateAtStart) ContinueDoUpdate();
    }

    public float waitToContinue = 1f;

    [ContextMenu("ContinueDoUpdate")]
    public void ContinueDoUpdate()
    {
        StartCoroutine(C_ContinueDoUpdate());
    }

    IEnumerator C_ContinueDoUpdate()
    {
        while (true)
        {
            DoUpdate();
            yield return new WaitForSeconds(waitToContinue);
        }
    }

    [ContextMenu("DoUpdate")]
    public void DoUpdate()
    {
        photonConnected = PhotonNetwork.IsConnected;
        currentLobby = PhotonNetwork.CurrentLobby == null ? "null" : PhotonNetwork.CurrentLobby.Name;
        currentRoom = PhotonNetwork.CurrentRoom == null ? "null" : PhotonNetwork.CurrentRoom.Name;
        isMasterClient = PhotonNetwork.IsMasterClient;
    }
    
    [ContextMenu("StopAllC")]
    public void StopAllC()
    {
        StopAllCoroutines();
    }    
}
