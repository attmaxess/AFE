using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnRefreshListCloudPoints : MonoBehaviour
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

        Vector3[] points = new Vector3[map.Length];
        for (int i = 0; i < map.Length; i++)
        {
            btnCloudPointSnapGround cp = Instantiate(btnCloudPointPrefab, Vector3.zero, Quaternion.identity, currentContent).GetComponent<btnCloudPointSnapGround>();
            cp.gameObject.SetActive(true);
            cp.SetCurrentCordinate(new Vector3(map[i].point.x, map[i].point.y, map[i].point.z));
        }
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

        for (int i = 0; i < int.Parse(Random.Range(1,20).ToString()); i++)
        {
            btnCloudPointSnapGround cp = Instantiate(btnCloudPointPrefab, Vector3.zero, Quaternion.identity, currentContent).GetComponent<btnCloudPointSnapGround>();
            cp.gameObject.SetActive(true);
            cp.SetCurrentCordinate(new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), Random.Range(0, 100f)));
        }
    }
}
