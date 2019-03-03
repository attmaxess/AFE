using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLoadAScene : MonoBehaviour
{
    public string currentPath = string.Empty;    

    public void OnClick()
    {
        if (!string.IsNullOrEmpty(currentPath))
            SceneManager.LoadScene(currentPath);
    }
}
