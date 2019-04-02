using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{   
    [Header("Params")] 
    public bool boolCreatePlaneJoystick = false;
    public string characterPrefab = string.Empty;    

    [ContextMenu("ClickSpawn")]
    public void ClickSpawn()
    {
        StartCoroutine(C_ClickSpawn());
    }

    IEnumerator C_ClickSpawn()
    {
        var newChar = PhotonNetwork.Instantiate(characterPrefab, Vector3.zero, Quaternion.identity, 0);
        yield return new WaitUntil(() => newChar.gameObject != null);                       

        if (boolCreatePlaneJoystick)
        {
            GameObject _planeJoyStick = Instantiate(Resources.Load("PlaneJoystick", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            yield return new WaitUntil(() => _planeJoyStick.gameObject != null);

            _planeJoyStick?.GetComponent<PlaneJoystick>().SetMainCharacter(newChar);
            MessageBroker.Default.Publish<MassageSpawnNewCharacter>(new MassageSpawnNewCharacter(newChar.transform));
        }
    }    
}
