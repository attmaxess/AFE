using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHT.TextureToString64
{
    [RequireComponent(typeof(TextureToBase64Methods))]
    public class ScreenCaptureManager : MonoBehaviour
    {
        [Header("Debug")]
        public bool isDebug = true;

        [Header("Screen Capture Process")]
        public bool doneTakeScreenCapture = true;

        TextureToBase64Methods _textureMethod = null;
        public TextureToBase64Methods textureMethod
        {
            get
            {
                if (_textureMethod == null) _textureMethod = GetComponent<TextureToBase64Methods>();
                if (_textureMethod == null) _textureMethod = gameObject.AddComponent<TextureToBase64Methods>();
                return _textureMethod;
            }
        }

        [ContextMenu("ScreenCapture")]
        public void TakeScreenCapture()
        {
            StartCoroutine(C_ScreenCapture());
        }

        IEnumerator C_ScreenCapture()
        {
            if (isDebug) Debug.Log("Start C_ScreenCapture");
            doneTakeScreenCapture = false;

            textureMethod.m_texture = ScreenCapture.CaptureScreenshotAsTexture();
            textureMethod.ToString64();

            if (isDebug) Debug.Log("Done C_ScreenCapture");
            doneTakeScreenCapture = true;

            yield break;
        }
    }

    [System.Serializable]
    public class ScreenCaptureData
    {
        public string texName = string.Empty;
        public string string64 = string.Empty;
    }
}