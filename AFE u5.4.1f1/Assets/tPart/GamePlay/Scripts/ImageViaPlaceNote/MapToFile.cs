using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static LibPlacenote;

public class MapToFile : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("File Info")]
    public string TextTextureFileName = string.Empty;

    [HideInInspector]
    public MapInfo mapInfo = null;

    [ContextMenu("ToString64File")]
    public void ToString64File()
    {
        if (string.IsNullOrEmpty(TextTextureFileName) || mapInfo == null)
        {
            Debug.Log("TextTextureFileName null || mapInfo null");
            return;
        }
                
        File.WriteAllText(Application.persistentDataPath + "/" + TextTextureFileName + ".txt", mapInfo.metadata.userdata.ToString(Formatting.None));
        DebugAppPerPath();
    }

    [ContextMenu("DebugAppPerPath")]
    public void DebugAppPerPath()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
