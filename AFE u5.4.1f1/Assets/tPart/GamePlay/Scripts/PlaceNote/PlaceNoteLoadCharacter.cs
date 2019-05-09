﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Com.Beetsoft.AFE;
using Photon.Pun;
using Com.Beetsoft.Luan;

public class PlaceNoteLoadCharacter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;    

    List<TestYasuo> testYasuos = new List<TestYasuo>();
    private void Start()
    {
        MessageBroker.Default.Receive<MessageChangedCharacterYasuo>().TakeUntilDestroy(gameObject).Subscribe(mess =>
        {
            if (mess.addOrRemove)
            {
                testYasuos.Add(mess.yasuo);
            }
            else
            {
                testYasuos.Remove(mess.yasuo);
            }
            if (testYasuos.Count == 1)
            {
            }
            if (testYasuos.Count == 2)
            {
                WaitAndSnap2();
            }
            if (testYasuos.Count == 0 || testYasuos.Count >= 3)
            {
            }
        });
    }

    [ContextMenu("WaitAndSnap2")]
    public void WaitAndSnap2()
    {
        StartCoroutine(C_WaitAndSnap2());
    }

    IEnumerator C_WaitAndSnap2()
    {
        yield return new WaitUntil(() => doneLoadCharacter == true);

        int index = PhotonNetwork.IsMasterClient ? 0 : 1;
        snap2Character.Snap(createCharacterAFE, currentGroundMarker.retrieveMainChar.spawnposList[index].gameObject);

        yield break;
    }

    [Header("LoadCharacter")]
    public bool doneLoadCharacter = true;
    public Com.Beetsoft.Luan.CreateCharacter createCharacterAFE = null;
    public CanvasJoystickManagerMethods joystickMethods = null;
    public BackgroundMarker currentGroundMarker = null;
    public PlaneJoystick currentJoystick = null;
    public Snap2Character snap2Character = null;

    public CreateCharacter createCharacterUFE = null;

    [ContextMenu("LoadCharacter")]
    public void LoadCharacter()
    {
        StartCoroutine(C_LoadCharacterUFE());
    }

    IEnumerator C_LoadCharacterAFE()
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

        createCharacterAFE.ClickSpawn();
        //joystickMethods.SetCanvasOn();
        yield return new WaitUntil(() => createCharacterAFE.doneClickSpawn == true);

        float momentPlaneJoystick = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<PlaneJoystick>() != null || Time.time - momentPlaneJoystick > 2f);

        currentJoystick = FindObjectOfType<PlaneJoystick>();
        if (currentJoystick == null && Time.time - momentPlaneJoystick > 2f)
        {
            doneLoadCharacter = true;
            yield break;
        }

        //currentGroundMarker.retrieveMainChar.eSnap = RetrieveMainCharacter.eSnapBackgroundMarker.SpawnPosList;
        //currentGroundMarker.retrieveMainChar.TrySnap(currentGroundMarker, currentJoystick, createCharacter.currentCharacter);
        //yield return new WaitUntil(() => currentGroundMarker.retrieveMainChar.doneSnapSpawnPos == true);

        if (isDebug) Debug.Log("Done C_LoadCharacter");
        doneLoadCharacter = true;

        yield break;
    }

    IEnumerator C_LoadCharacterUFE()
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

        createCharacterUFE.UFEDoCreate();
        yield return new WaitUntil(() => CreateCharacter.sdoneUFEDoCreate == true);

        GameObject gameEngine = GameObject.Find("Game");
        gameEngine.transform.position = currentGroundMarker.transform.position;

        if (isDebug) Debug.Log("Done C_LoadCharacter");
        doneLoadCharacter = true;

        yield break;
    }
}
