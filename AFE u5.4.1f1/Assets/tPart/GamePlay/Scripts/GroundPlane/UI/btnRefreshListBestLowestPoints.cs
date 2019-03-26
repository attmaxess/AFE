using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnRefreshListBestLowestPoints : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public LibPlacenote placenote = null;

    [Header("Content List")]
    public btnCloudPointSnapGround btnCloudPointPrefab = null;
    public Transform currentContent = null;
    public Transform contentPrefab = null;
    public Transform transViewport = null;
    public ScrollRect scrollView = null;

    [ContextMenu("RefreshList")]
    public void RefreshList()
    {
        if (isDebug) Debug.Log("RefreshList");

        if (LibPlacenote.Instance.GetStatus() != LibPlacenote.MappingStatus.RUNNING)
        {
            return;
        }

        LibPlacenote.PNFeaturePointUnity[] map = LibPlacenote.Instance.GetMap();

        if (map == null)
        {
            return;
        }

        if (currentContent != null)
        {
            if (Application.isPlaying) Destroy(currentContent.gameObject); else DestroyImmediate(currentContent.gameObject);
        }

        currentContent = Instantiate(contentPrefab, Vector3.zero, Quaternion.identity, transViewport).transform;
        currentContent.name = "Content";
        currentContent.gameObject.SetActive(true);
        scrollView.content = currentContent.GetComponent<RectTransform>();

        List<LibPlacenote.PNFeaturePointUnity> sortedMap = new List<LibPlacenote.PNFeaturePointUnity>();
        List<LibPlacenote.PNFeaturePointUnity> tempMap = new List<LibPlacenote.PNFeaturePointUnity>();
        for (int i = 0; i < map.Length; i++) { sortedMap.Add(map[i]); tempMap.Add(map[i]); }
        sortedMap.Sort((a, b) => MagnituteOfSumDirection(tempMap, a).CompareTo(MagnituteOfSumDirection(tempMap, b)));

        for (int i = 0; i < sortedMap.Count; i++)
        {
            btnCloudPointSnapGround cp = Instantiate(btnCloudPointPrefab, Vector3.zero, Quaternion.identity, currentContent).GetComponent<btnCloudPointSnapGround>();
            cp.gameObject.SetActive(true);
            cp.SetCurrentCordinate(i, new Vector3(sortedMap[i].point.x, sortedMap[i].point.y, -sortedMap[i].point.z));
        }
    }

    float MagnituteOfSumDirection(List<LibPlacenote.PNFeaturePointUnity> sortedMap, LibPlacenote.PNFeaturePointUnity indexedPoint)
    {
        Vector3 currentSumDirection = Vector3.zero;
        for (int i = 0; i < sortedMap.Count; i++)
        {
            Vector3 toV3 = new Vector3(sortedMap[i].point.x, sortedMap[i].point.y, -sortedMap[i].point.z);
            Vector3 fromV3 = new Vector3(indexedPoint.point.x, indexedPoint.point.y, -indexedPoint.point.z);
            currentSumDirection += toV3 - fromV3;
        }
        return (currentSumDirection / sortedMap.Count).magnitude;
    }

    [ContextMenu("RandomList")]
    public void RandomList()
    {
        if (isDebug) Debug.Log("RandomList");

        if (currentContent != null)
        {
            if (Application.isPlaying) Destroy(currentContent.gameObject); else DestroyImmediate(currentContent.gameObject);
        }
        currentContent = Instantiate(contentPrefab, Vector3.zero, Quaternion.identity, transViewport).transform;
        currentContent.name = "Content";
        currentContent.gameObject.SetActive(true);
        scrollView.content = currentContent.GetComponent<RectTransform>();

        for (int i = 0; i < int.Parse(Random.Range(1, 20).ToString()); i++)
        {
            btnCloudPointSnapGround cp = Instantiate(btnCloudPointPrefab, Vector3.zero, Quaternion.identity, currentContent).GetComponent<btnCloudPointSnapGround>();
            cp.gameObject.SetActive(true);
            cp.SetCurrentCordinate(i, new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), -Random.Range(0, 100f)));
        }
    }
}
