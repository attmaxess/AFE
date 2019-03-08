using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateVikingPrefab : MonoBehaviour
{
    public Text text;
    int count = 0;
    GameManagerArVik GameManagerArVik;

    public void CreateVikingObject(Vector3 pos, GameObject hitObject)
    {

        Debug.Log("CreateVikingObject");
        count++;
        text.text = count.ToString();
        if (GameManagerArVik == null)
        {
            GameManagerArVik = GameObject.FindObjectOfType<GameManagerArVik>();
        }

        if (GameManagerArVik != null)
        {
            bool isSpawn = false;
            //GameManagerArVik.DestroyObj();
            var aa = GameObject.FindObjectsOfType<ThirdPersonNetworkARVik>();
            for (int i = 0; i < aa.Length; i++)
            {
                if (aa[i].gameObject.GetPhotonView().isMine)
                {
                    isSpawn = true;
                }
            }

            if (!isSpawn)
                GameManagerArVik.RpcSpawnObject(pos, hitObject);
        }
    }

    public void ClickBtnSpawnObject()
    {
        CreateVikingObject(Vector3.one * (Random.Range(1, 2)), Camera.main.gameObject);
    }

    public void ClickSpawnAhri()
    {
        if (GameManagerArVik == null)
        {
            GameManagerArVik = GameObject.FindObjectOfType<GameManagerArVik>();
        }

        if (GameManagerArVik != null)
        {
            GameManagerArVik.prefabName = "Ahri";
        }
    }

    public void ClickSpawnYasuo()
    {
        if (GameManagerArVik == null)
        {
            GameManagerArVik = GameObject.FindObjectOfType<GameManagerArVik>();
        }

        if (GameManagerArVik != null)
        {
            GameManagerArVik.prefabName = "Yasuo";
        }
    }
}
