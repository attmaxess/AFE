using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [ContextMenu("LoadCharacter")]
    public void LoadCharacter()
    {
        StartCoroutine(C_LoadCharacter());
    }

    IEnumerator C_LoadCharacter()
    {
        if (isDebug) Debug.Log("Start C_LoadCharacter");
        doneLoadCharacter = false;

        if (PhotonCharacterExisted.Instance.CharacterExisted())
        {
            if (isDebug) Debug.Log("Character Exited!");
            doneLoadCharacter = true;
            yield break;
        }

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
