using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveMainCharacter : MonoBehaviour
{
    [Header("Debug")]
    public bool ClickHere = false;

    [Header("Process")]
    public BackgroundMarker currentGround = null;
    public GameObject currentMainChar = null;

    [Header("Spawn Pos")]
    public List<ColliderList> spawnposList = new List<ColliderList>();

    public enum eSnapBackgroundMarker
    {
        Center, 
        SpawnPosList
    }

    public eSnapBackgroundMarker eSnap = eSnapBackgroundMarker.Center;

    [Header("TrySnap")]
    public bool doneTrySnap = true;

    public void TrySnap(BackgroundMarker ground = null, GameObject character = null)
    {
        StartCoroutine(C_TrySnap(ground, character));
    }

    IEnumerator C_TrySnap(BackgroundMarker ground = null, GameObject character = null)
    {
        doneTrySnap = false;

        if (ground == null)
        {
            yield return new WaitUntil(() => FindObjectOfType<BackgroundMarker>() != null);
            ground = FindObjectOfType<BackgroundMarker>();
            yield return new WaitForSeconds(0.3f);
        }
        currentGround = ground;

        if (character == null)
        {
            ///Try find here
        }
        currentMainChar = character;

        switch (eSnap)
        {
            case eSnapBackgroundMarker.Center:
                SnapCenter();
                break;

            case eSnapBackgroundMarker.SpawnPosList:
                SnapSpawnPos();
                break;
        }

        doneTrySnap = true;
        yield break;
    }

    [ContextMenu("SnapCenter")]
    public void SnapCenter()
    {
        if (currentGround == null || currentMainChar == null) return;
        currentMainChar.transform.position = currentGround.transform.position; 
    }

    [Header("SnapSpawnPos")]
    public bool doneSnapSpawnPos = true;

    [ContextMenu("SnapSpawnPos")]
    public void SnapSpawnPos()
    {
        StartCoroutine(C_SnapSpawnPos());
    }

    IEnumerator C_SnapSpawnPos()
    {
        doneSnapSpawnPos = false;

        if (currentGround == null || currentMainChar == null) yield break;

        for (int i = 0; i < spawnposList.Count; i++)
        {
            spawnposList[i].Shake();
            yield return new WaitUntil(() => spawnposList[i].doneShake == true);
            if (spawnposList[i].colList.Count == 0)
            {
                currentMainChar.transform.position = new Vector3(spawnposList[i].transform.position.x, currentGround.transform.position.y, spawnposList[i].transform.position.z);
                doneSnapSpawnPos = true;
                yield break;
            }
        }

        doneSnapSpawnPos = true;
    }
}
