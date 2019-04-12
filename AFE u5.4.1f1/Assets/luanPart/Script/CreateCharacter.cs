using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Params")] 
    public bool boolCreatePlaneJoystick = false;
    public string characterPrefab = string.Empty;

    [Header("Output")]
    public GameObject currentCharacter = null;

    [Header("Spawn process")]
    public bool doneClickSpawn = true;

    [ContextMenu("ClickSpawn")]
    public void ClickSpawn()
    {
        StartCoroutine(C_ClickSpawn());
    }

    IEnumerator C_ClickSpawn()
    {
        if (isDebug) Debug.Log("Start C_ClickSpawn");
        doneClickSpawn = false;

        if (PhotonCharacterExisted.Instance.CharacterExisted())
        {
            if (isDebug) Debug.Log("Character Exited!");
            doneClickSpawn = true;
            yield break;
        }

        var newChar = PhotonNetwork.Instantiate(characterPrefab, Vector3.zero, Quaternion.identity, 0);
        yield return new WaitUntil(() => newChar.gameObject != null);

        currentCharacter = newChar;

        if (boolCreatePlaneJoystick)
        {
            GameObject _planeJoyStick = Instantiate(Resources.Load("PlaneJoystick", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            yield return new WaitUntil(() => _planeJoyStick.gameObject != null);

            _planeJoyStick?.GetComponent<PlaneJoystick>().SetMainCharacter(newChar);
            MessageBroker.Default.Publish<MassageSpawnNewCharacter>(new MassageSpawnNewCharacter(newChar.transform));
        }

        if (isDebug) Debug.Log("Done C_ClickSpawn");
        doneClickSpawn = true;

        yield break;
    }    
}
