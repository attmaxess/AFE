using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Com.Beetsoft.AFE;

public class PlaceNoteLoadCharacter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("LoadCharacter")]
    public bool doneLoadCharacter = true;
    public CreateCharacter createCharacter = null;
    public CanvasJoystickManagerMethods joystickMethods = null;
    public BackgroundMarker currentGroundMarker = null;
    public PlaneJoystick currentJoystick = null;

    List<TestYasuo> testYasuos = new List<TestYasuo>();
    private void Start()
    {
        MessageBroker.Default.Receive<MessageChangedCharacterYasuo>().Subscribe(mess =>
        {
            if (mess.addOrRemove)
            {
                testYasuos.Add(mess.yasuo);
            }
            else
            {
                testYasuos.Remove(mess.yasuo);
            }
            Debug.Log("count yasuo in screen" + testYasuos.Count);
            if (testYasuos.Count == 1)
            {
            }
            if (testYasuos.Count == 2)
            {
                int count = 3;
                Observable.Interval(System.TimeSpan.FromSeconds(1)).TakeWhile(_ => count >= 1 && testYasuos.Count == 2).Subscribe(_ =>
                {
                    currentGroundMarker.retrieveMainChar.eSnap = RetrieveMainCharacter.eSnapBackgroundMarker.SpawnPosList;
                    currentGroundMarker.retrieveMainChar.TrySnap(currentGroundMarker, currentJoystick, createCharacter.currentCharacter);
                    count--;
                });
            }
            if (testYasuos.Count == 0 || testYasuos.Count >= 3)
            {
            }
        });
    }

    [ContextMenu("LoadCharacter")]
    public void LoadCharacter()
    {
        StartCoroutine(C_LoadCharacter());
    }

    IEnumerator C_LoadCharacter()
    {
        if (isDebug) Debug.Log("Start C_LoadCharacter");
        doneLoadCharacter = false;

        float momentBackground = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<BackgroundMarker>() != null || Time.time - momentBackground > 2f);

        currentGroundMarker = FindObjectOfType<BackgroundMarker>();
        if (currentGroundMarker == null && Time.time - momentBackground > 2f)
        {
            doneLoadCharacter = true;
            yield break;
        }
        currentGroundMarker.Show();

        createCharacter.ClickSpawn();
        joystickMethods.SetCanvasOn();
        yield return new WaitUntil(() => createCharacter.doneClickSpawn == true);

        float momentPlaneJoystick = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<PlaneJoystick>() != null || Time.time - momentPlaneJoystick > 2f);

        currentJoystick = FindObjectOfType<PlaneJoystick>();
        if (currentJoystick == null && Time.time - momentPlaneJoystick > 2f)
        {
            doneLoadCharacter = true;
            yield break;
        }

        currentGroundMarker.retrieveMainChar.eSnap = RetrieveMainCharacter.eSnapBackgroundMarker.SpawnPosList;
        currentGroundMarker.retrieveMainChar.TrySnap(currentGroundMarker, currentJoystick, createCharacter.currentCharacter);
        yield return new WaitUntil(() => currentGroundMarker.retrieveMainChar.doneSnapSpawnPos == true);

        if (isDebug) Debug.Log("Done C_LoadCharacter");
        doneLoadCharacter = true;

        yield break;
    }
}
