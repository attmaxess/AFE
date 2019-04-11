using DHT.TextureToString64;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlaceNoteLoadMapImage : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Inputs")]
    public ScreenCaptureManager screenCapture = null;
    public GameObject capturePanel;
    public Image captureImage;

    [Header("LoadImagePanel")]
    public bool doneLoadImagePanel = true;

    public void LoadImagePanel(LibPlacenote.MapInfo mSelectedMapInfo)
    {
        StartCoroutine(C_LoadImagePanel(mSelectedMapInfo));
    }

    IEnumerator C_LoadImagePanel(LibPlacenote.MapInfo mSelectedMapInfo)
    {
        if (isDebug) Debug.Log("Start C_LoadImagePanel");
        doneLoadImagePanel = false;

        capturePanel.SetActive(true);

        ScreenCaptureData captureData = mSelectedMapInfo.metadata.userdata["capture"].ToObject<ScreenCaptureData>();

        screenCapture.textureMethod.TextTextureFileName = captureData.texName;
        screenCapture.textureMethod.ReConfirmPathType();

        if (File.Exists(Application.persistentDataPath + "/" + screenCapture.textureMethod.TextTextureFileName + ".png"))
        {
            if (isDebug) Debug.Log("Existed " + screenCapture.textureMethod.TextTextureFileName + ".png, just reload from persistPath!!");
        }
        else
        {
            screenCapture.textureMethod.m_encodedData = captureData.string64;

            screenCapture.textureMethod.GetTexture2D();
            yield return new WaitUntil(() => screenCapture.textureMethod.m_texture != null);

            screenCapture.textureMethod.ToString64File();
            yield return new WaitUntil(() => screenCapture.textureMethod.doneToString64File == true);

            screenCapture.textureMethod.ToPNGFile();
            yield return new WaitUntil(() => screenCapture.textureMethod.doneToPNGFile == true);

        }

        screenCapture.textureMethod.GetTexture2DFromPNG();

        Vector2 texSize = new Vector2(screenCapture.textureMethod.m_texture.width, screenCapture.textureMethod.m_texture.height);
        captureImage.sprite = Sprite.Create(screenCapture.textureMethod.m_texture, new Rect(0f, 0f, texSize.x, texSize.y), new Vector2(0.5f, 0.5f), 100f);
        captureImage.sprite.name = captureData.texName;

        if (isDebug) Debug.Log("Start C_LoadImagePanel");
        doneLoadImagePanel = true;

        yield break;
    }
}
