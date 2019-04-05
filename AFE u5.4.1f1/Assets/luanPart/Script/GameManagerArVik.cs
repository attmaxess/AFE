using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;
using Photon.Pun;
using UniRx;

public class GameManagerArVik : MonoBehaviourPunCallbacks
{
    public string prefabName = "VikingPrefab";
    bool isJoinedRoom = false;
    public List<PhotonView> listCharacter = new List<PhotonView>();

    public static GameManagerArVik instance = null;

    public event System.Action attack;
    public event System.Action skill1;
    public event System.Action skill2;
    public event System.Action skill3;
    public event System.Action skill4;

    public static GameManagerArVik Singleton
    {
        get
        {
            return instance;
        }
    }

    public PhotonView GetIsMineChar()
    {
        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (listCharacter[i].IsMine)
            {
                return listCharacter[i];
            }
        }
        return null;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {

    }

    IEnumerator OnLeftRoom()
    {

        Debug.Log("OnLeftRoom");

        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.CurrentRoom != null || PhotonNetwork.IsConnected == false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {

    }

    private void Start()
    {

    }

    public override void OnJoinedRoom()
    {
        isJoinedRoom = true;
        Debug.Log("OnJoinedRoom");
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null) return;
        //Only display this GUI when inside a room        
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }

    public bool isSpawnMainCharacter;

    public void SpawnObject(Vector3 pos, GameObject hitObject)
    {
        StartCoroutine(C_SpawnObject(pos, hitObject));
    }

    public IEnumerator C_SpawnObject(Vector3 pos, GameObject hitObject)
    {
        if (isSpawnMainCharacter) yield break;

        isSpawnMainCharacter = true;

        var newChar = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity, 0);
        yield return new WaitUntil(() => newChar.gameObject != null);

        GameObject _planeJoyStick = Instantiate(Resources.Load("PlaneJoystick", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
        yield return new WaitUntil(() => _planeJoyStick.gameObject != null);

        _planeJoyStick?.GetComponent<PlaneJoystick>().SetMainCharacter(newChar);
        MessageBroker.Default.Publish<MassageSpawnNewCharacter>(new MassageSpawnNewCharacter(newChar.transform));

        yield break;
    }
}

public class MassageSpawnNewCharacter
{
    public ReactiveProperty<Transform> mainCharacter = new ReactiveProperty<Transform>();

    public MassageSpawnNewCharacter(Transform mainCharacter)
    {
        this.mainCharacter.Value = mainCharacter;
    }
}