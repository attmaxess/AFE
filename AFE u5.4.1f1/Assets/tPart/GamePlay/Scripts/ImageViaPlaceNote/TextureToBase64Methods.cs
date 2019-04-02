using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHT.TextureToString64
{
    public class TextureToBase64Methods : MonoBehaviour
    {
        [Header("Debug")]
        public bool isDebug = true;

        [Header("To Texture")]
        public Texture2D m_texture = null;

        [ContextMenu("ToString64")]
        public void ToString64()
        {
            byte[] imageData = m_texture.EncodeToPNG();
            m_encodedData = Convert.ToBase64String(imageData);
            if (isDebug) Debug.Log(m_encodedData);
        }

        [ContextMenu("ToString64File")]
        public void ToString64File()
        {
            if (m_texture.name == string.Empty)
            {
                Debug.Log("m_texture.name null");
                return;
            }

            byte[] imageData = m_texture.EncodeToPNG();
            File.WriteAllText(Application.persistentDataPath + "/" + m_texture.name + ".txt", Convert.ToBase64String(imageData));
            DebugAppPerPath();
        }

        [Header("From Texture")]
        string _m_encodedData = string.Empty;
        [HideInInspector]
        public string m_encodedData
        {
            get { return _m_encodedData; }
            set
            {
                _m_encodedData = value;
                encodeLength = _m_encodedData.Length;
            }
        }

        public int encodeLength = 0;
        private readonly object file;

        [ContextMenu("GetTexture2D")]
        public void GetTexture2D()
        {
            this.m_texture = null;

            byte[] imageData = Convert.FromBase64String(m_encodedData);

            int width, height;
            GetImageSize(imageData, out width, out height);

            m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            m_texture.hideFlags = HideFlags.HideAndDontSave;
            m_texture.filterMode = FilterMode.Point;

            m_texture.LoadImage(imageData);
        }

        private void GetImageSize(byte[] imageData, out int width, out int height)
        {
            width = ReadInt(imageData, 3 + 15);
            height = ReadInt(imageData, 3 + 15 + 2 + 2);
        }

        private int ReadInt(byte[] imageData, int offset)
        {
            return (imageData[offset] << 8) | imageData[offset + 1];
        }

        [Header("Resize")]
        public Vector2 resizeTo = Vector2.one;

        [ContextMenu("Resize")]
        public void Resize()
        {

        }

        [Header("Texture From File")]
        public string TextTextureFileName = string.Empty;

        [ContextMenu("GetTexture2DFromFile")]
        public void GetTexture2DFromFile()
        {
            this.m_texture = null;

            m_encodedData = File.ReadAllText(Application.persistentDataPath + "/" + TextTextureFileName + ".txt");
            byte[] imageData = Convert.FromBase64String(m_encodedData);

            int width, height;
            GetImageSize(imageData, out width, out height);

            m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            m_texture.hideFlags = HideFlags.HideAndDontSave;
            m_texture.filterMode = FilterMode.Point;

            m_texture.LoadImage(imageData);
        }

        [ContextMenu("DebugAppPerPath")]
        public void DebugAppPerPath()
        {
            Debug.Log(Application.persistentDataPath);
        }
    }
}