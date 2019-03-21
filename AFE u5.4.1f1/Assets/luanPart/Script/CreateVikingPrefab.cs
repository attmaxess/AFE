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
            //GameManagerArVik.SpawnCharacter(pos, hitObject);
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
            GameManagerArVik.prefabName = "AhriPrefab";
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
            GameManagerArVik.prefabName = "YasuoPrefab";
        }
    }

    public void ClickSpawn(string characterPrefab)
    {
        if (GameManagerArVik == null)
        {
            GameManagerArVik = GameObject.FindObjectOfType<GameManagerArVik>();
        }

        if (GameManagerArVik != null)
        {
            GameManagerArVik.prefabName = characterPrefab;
        }
    }
}
