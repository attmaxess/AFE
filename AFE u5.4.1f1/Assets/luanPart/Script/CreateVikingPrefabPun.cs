using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateVikingPrefabPun : Photon.MonoBehaviour
{

    public string prefabName = "VikingPrefab";

    public Text text;
    int count = 0;

    public void CreateVikingObject(Vector3 pos, GameObject hitObject)
    {

        Debug.Log("CreateVikingObject");

        count++;
        text.text = count.ToString();
        bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = Random.Range(0, 2) == 0;//Axe
        enabledRenderers[1] = Random.Range(0, 2) == 0; ;//Shield

        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;

        PhotonNetwork.Instantiate(this.prefabName, pos, hitObject.transform.rotation, 0, objs);

    }


}
