using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollViewAllScenes : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;
    public bool ClickHere = false;

    [Header("Prefab")]
    public GameObject contentPredfab = null;
    public Transform currentViewport = null;
    public Transform currentContent = null;

    public GameObject buttonPrefab = null;

    [Header("Load")]
    public List<string> nameList = new List<string>();
    public List<string> pathList = new List<string>();

#if UNITY_EDITOR
    [ContextMenu("LoadAllPaths")]
    public void LoadAllPaths()
    {
        nameList = new List<string>();
        pathList = new List<string>();

        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                pathList.Add(S.path);

                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                nameList.Add(name);
            }
        }
    }
#endif

    [ContextMenu("LoadAllScenes")]
    public void LoadAllScenes()
    {
        if (currentContent != null)
            if (Application.isPlaying) Destroy(currentContent.gameObject);
            else DestroyImmediate(currentContent.gameObject);

        currentContent = Instantiate(contentPredfab, currentViewport.position, currentViewport.rotation, currentViewport).transform;
        currentContent.gameObject.SetActive(true);
        ScrollRect scroll = GetComponent<ScrollRect>();
        scroll.content = currentContent.GetComponent<RectTransform>();

        for (int i = 0; i < pathList.Count; i++)
        {
            Button tempButton = Instantiate(buttonPrefab, currentContent).GetComponent<Button>();
            tempButton.gameObject.SetActive(true);
            tempButton.GetComponentInChildren<Text>().text = nameList[i];
            ButtonLoadAScene buttonScript = tempButton.GetComponent<ButtonLoadAScene>();
            buttonScript.currentPath = pathList[i];            
        }
    }    
}
