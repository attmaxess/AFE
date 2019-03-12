using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    GameManagerArVik GameManagerArVik = null;

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
