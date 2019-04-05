using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName = string.Empty;

    [ContextMenu("LoadNormal")]
    public void LoadNormal()
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    [ContextMenu("LoadAsync")]
    public void LoadAsync()
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadSceneAsync(sceneName);
    }
}
