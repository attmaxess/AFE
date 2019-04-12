using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCharacterExisted : Singleton<PhotonCharacterExisted>
{
    public bool CharacterExisted()
    {
        bool existed = false;
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        List<IPlayer> playerList = new List<IPlayer>();
        for (int i = 0; i < photonViews.Length; i++)
            if (photonViews[i].GetComponent<IPlayer>() != null)
                playerList.Add(photonViews[i].GetComponent<IPlayer>());

        for (int i = 0; i < playerList.Count; i++)
            if (playerList[i].GameObject().GetComponent<PhotonView>().IsMine)
                return true;

        return existed;
    }
}
