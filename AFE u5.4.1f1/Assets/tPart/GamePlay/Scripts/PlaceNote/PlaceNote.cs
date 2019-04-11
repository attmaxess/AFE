using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DHT.TextureToString64;
using AFE.BaseGround;
using Photon.Pun;

public class PlaceNote : MonoBehaviour, PlacenoteListener
{
    [Header("Debug")]
    public bool isDebug = true;

    [SerializeField]
    GameObject mMapSelectedPanel;
    [SerializeField]
    GameObject mInitButtonPanel;
    [SerializeField]
    GameObject mMappingButtonPanel;
    [SerializeField]
    GameObject mSimulatorAddShapeButton;
    [SerializeField]
    GameObject mMapListPanel;
    [SerializeField]
    GameObject mExitButton;
    [SerializeField]
    GameObject mListElement;
    [SerializeField]
    RectTransform mListContentParent;
    [SerializeField]
    ToggleGroup mToggleGroup;
    [SerializeField]
    Text mLabelText;
    [SerializeField]
    Slider mRadiusSlider;
    [SerializeField]
    float mMaxRadiusSearch;
    [SerializeField]
    Text mRadiusLabel;

    private UnityARSessionNativeInterface mSession;

    private bool mARInit = false;


    private LibPlacenote.MapMetadataSettable mCurrMapDetails;

    [Header("mReportDebug ")]
    public bool mReportDebug = false;

    private LibPlacenote.MapInfo mSelectedMapInfo;
    string mSelectedMapId
    {
        get { return mSelectedMapInfo != null ? mSelectedMapInfo.placeId : null; }
    }

    private string mSaveMapId = null;

    private BoxCollider mBoxColliderDummy;
    private SphereCollider mSphereColliderDummy;
    private CapsuleCollider mCapColliderDummy;


    // Use this for initialization
    void Start()
    {
        Input.location.Start();

        mMapListPanel.SetActive(false);

        mSession = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        StartARKit();
        FeaturesVisualizer.EnablePointcloud();
        LibPlacenote.Instance.RegisterListener(this);
        ResetSlider();

        // for simulator
#if UNITY_EDITOR
        mSimulatorAddShapeButton.SetActive(true);
#endif
    }

    void Update()
    {
        if (!mARInit && LibPlacenote.Instance.Initialized())
        {
            mARInit = true;
            mLabelText.text = "Ready to Start!";
        }
    }

    [Header("Debug Map File Info")]
    public MapToFile mapToFile = null;
    public bool isDebugMapFileInfo = false;

    public void OnListMapClick()
    {
        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            return;
        }

        foreach (Transform t in mListContentParent.transform)
        {
            Destroy(t.gameObject);
        }

        mMapListPanel.SetActive(true);
        mInitButtonPanel.SetActive(false);
        mRadiusSlider.gameObject.SetActive(true);
        LibPlacenote.Instance.ListMaps((mapList) =>
        {
            // render the map list!
            foreach (LibPlacenote.MapInfo mapInfoItem in mapList)
            {
                if (mapInfoItem.metadata.userdata != null)
                {
                    if (isDebug) Debug.Log(mapInfoItem.metadata.userdata.ToString(Formatting.None));
                    if (isDebugMapFileInfo)
                    {
                        mapToFile.TextTextureFileName = mapInfoItem.placeId;
                        mapToFile.mapInfo = mapInfoItem;
                        mapToFile.ToString64File();
                    }
                }
                AddMapToList(mapInfoItem);
            }
        });
    }

    //public void LoadMapID

    public void OnRadiusSelect()
    {
        Debug.Log("Map search:" + mRadiusSlider.value.ToString("F2"));
        mLabelText.text = "Filtering maps by GPS location";

        LocationInfo locationInfo = Input.location.lastData;


        float radiusSearch = mRadiusSlider.value * mMaxRadiusSearch;
        mRadiusLabel.text = "Distance Filter: " + (radiusSearch / 1000.0).ToString("F2") + " km";

        LibPlacenote.Instance.SearchMaps(locationInfo.latitude, locationInfo.longitude, radiusSearch,
            (mapList) =>
            {
                foreach (Transform t in mListContentParent.transform)
                {
                    Destroy(t.gameObject);
                }
                // render the map list!
                foreach (LibPlacenote.MapInfo mapId in mapList)
                {
                    if (mapId.metadata.userdata != null)
                    {
                        Debug.Log(mapId.metadata.userdata.ToString(Formatting.None));
                    }
                    AddMapToList(mapId);
                }

                mLabelText.text = "Map List Complete";
            });
    }

    public void ResetSlider()
    {
        mRadiusSlider.value = 1.0f;
        mRadiusLabel.text = "Distance Filter: Off";
    }

    public void OnCancelClick()
    {
        mMapSelectedPanel.SetActive(false);
        mMapListPanel.SetActive(false);
        mInitButtonPanel.SetActive(true);
        ResetSlider();
    }


    public void OnExitClick()
    {
        mInitButtonPanel.SetActive(true);
        mExitButton.SetActive(false);
        mMappingButtonPanel.SetActive(false);

        LibPlacenote.Instance.StopSession();
        FeaturesVisualizer.clearPointcloud();
        GetComponent<ShapeManager>().ClearShapes();

    }


    void AddMapToList(LibPlacenote.MapInfo mapInfo)
    {
        GameObject newElement = Instantiate(mListElement) as GameObject;
        MapInfoElement listElement = newElement.GetComponent<MapInfoElement>();
        listElement.Initialize(mapInfo, mToggleGroup, mListContentParent, (value) =>
        {
            OnMapSelected(mapInfo);
        });
    }


    void OnMapSelected(LibPlacenote.MapInfo mapInfo)
    {
        mSelectedMapInfo = mapInfo;
        mMapSelectedPanel.SetActive(true);
        mRadiusSlider.gameObject.SetActive(false);
    }

    public void OnLoadMapClicked()
    {
        ConfigureSession();

        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            return;
        }

        ResetSlider();
        mLabelText.text = "Loading Map ID: " + mSelectedMapId;
        LibPlacenote.Instance.LoadMap(mSelectedMapId,
            (completed, faulted, percentage) =>
            {
                if (completed)
                {
                    mMapSelectedPanel.SetActive(false);
                    mMapListPanel.SetActive(false);
                    mInitButtonPanel.SetActive(false);
                    mMappingButtonPanel.SetActive(false);
                    mExitButton.SetActive(true);

                    LibPlacenote.Instance.StartSession();

                    if (mReportDebug)
                    {
                        LibPlacenote.Instance.StartRecordDataset(
                            (datasetCompleted, datasetFaulted, datasetPercentage) =>
                            {

                                if (datasetCompleted)
                                {
                                    mLabelText.text = "Dataset Upload Complete";
                                }
                                else if (datasetFaulted)
                                {
                                    mLabelText.text = "Dataset Upload Faulted";
                                }
                                else
                                {
                                    mLabelText.text = "Dataset Upload: " + datasetPercentage.ToString("F2") + "/1.0";
                                }
                            });
                        Debug.Log("Started Debug Report");
                    }

                    mLabelText.text = "Loaded ID: " + mSelectedMapId;

                    AfterLoadARMap();
                }
                else if (faulted)
                {
                    mLabelText.text = "Failed to load ID: " + mSelectedMapId;
                }
                else
                {
                    mLabelText.text = "Map Download: " + percentage.ToString("F2") + "/1.0";
                }
            }
        );
    }

    [Header("OnLoadMapClicked + nameMap")]
    public bool doneOnLoadMapClickedNameMap = true;

    public void OnLoadMapClicked(string nameMap)
    {
        StartCoroutine(C_OnLoadMapClicked(nameMap));
    }

    IEnumerator C_OnLoadMapClicked(string nameMap)
    {
        if (isDebug) Debug.Log("Start C_OnLoadMapClicked " + nameMap);
        doneOnLoadMapClickedNameMap = false;

        ConfigureSession();

        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            yield break;
        }

        LibPlacenote.Instance.ListMaps((mapList) =>
        {
            // render the map list!
            for (int i = 0; i < mapList.Length; i++)
            {
                if (mapList[i].metadata.userdata != null)
                {
                    if (mapList[i].placeId == nameMap)
                    {
                        mSelectedMapInfo = mapList[i];
                        OnLoadMapClicked();

                        i = mapList.Length;
                        doneOnLoadMapClickedNameMap = true;
                    }                    
                }                
            }
        });

        if (isDebug) Debug.Log("Start C_OnLoadMapClicked " + nameMap);
        doneOnLoadMapClickedNameMap = true;

        yield break;
    }

    [Header("AfterLoadARMap")]
    public PlaceNoteLoadMapImage loadImage = null;
    public PlaceNoteLoadCharacter loadCharacter = null;
    public PhotonMenu photonMenu = null;

    void AfterLoadARMap()
    {
        StartCoroutine(C_AfterLoadARMap());
    }

    IEnumerator C_AfterLoadARMap()
    {
        //LoadImagePanel();
        //yield return new WaitUntil(() => doneLoadImagePanel == true);

        loadCharacter.LoadCharacter();
        yield return new WaitUntil(() => loadCharacter.doneLoadCharacter == true);

        if (PhotonNetwork.IsMasterClient)
        {
            photonMenu.nameMap = mSelectedMapId;
            photonMenu.MasterRPCMap();
        }

        yield break;
    }

    public void OnDeleteMapClicked()
    {
        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            return;
        }

        mLabelText.text = "Deleting Map ID: " + mSelectedMapId;
        LibPlacenote.Instance.DeleteMap(mSelectedMapId, (deleted, errMsg) =>
        {
            if (deleted)
            {
                mMapSelectedPanel.SetActive(false);
                mLabelText.text = "Deleted ID: " + mSelectedMapId;
                OnListMapClick();
            }
            else
            {
                mLabelText.text = "Failed to delete ID: " + mSelectedMapId;
            }
        });
    }

    public void OnNewMapClick()
    {
        ConfigureSession();

        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            return;
        }

        mInitButtonPanel.SetActive(false);
        mMappingButtonPanel.SetActive(true);

        Debug.Log("Started Session");
        LibPlacenote.Instance.StartSession();

        if (mReportDebug)
        {
            LibPlacenote.Instance.StartRecordDataset(
                (completed, faulted, percentage) =>
                {
                    if (completed)
                    {
                        mLabelText.text = "Dataset Upload Complete";
                    }
                    else if (faulted)
                    {
                        mLabelText.text = "Dataset Upload Faulted";
                    }
                    else
                    {
                        mLabelText.text = "Dataset Upload: (" + percentage.ToString("F2") + "/1.0)";
                    }
                });
            Debug.Log("Started Debug Report");
        }
    }

    private void StartARKit()
    {
#if !UNITY_EDITOR
		mLabelText.text = "Initializing ARKit";
		Application.targetFrameRate = 60;
		ConfigureSession ();
#endif
    }


    private void ConfigureSession()
    {
#if !UNITY_EDITOR
		ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration ();
		config.alignment = UnityARAlignment.UnityARAlignmentGravity;
		config.getPointCloudData = true;
		config.enableLightEstimation = true;
        config.planeDetection = UnityARPlaneDetection.Horizontal;
		mSession.RunWithConfig (config);
#endif
    }

    [Header("Screen Capture")]
    public bool doneOnSaveMapClick = true;
    public ScreenCaptureManager screenCapture = null;
    public ScreenshotHelper screenShot = null;
    public BackgroundMarkerMethod backgroundMethod = null;

    public void OnSaveMapClick()
    {
        StartCoroutine(C_OnSaveMapClick());
    }

    IEnumerator C_OnSaveMapClick()
    {
        if (isDebug) Debug.Log("Start C_OnSaveMapClick");
        doneOnSaveMapClick = false;

        if (!LibPlacenote.Instance.Initialized())
        {
            Debug.Log("SDK not yet initialized");
            yield break;
        }

        bool useLocation = Input.location.status == LocationServiceStatus.Running;
        LocationInfo locationInfo = Input.location.lastData;

        mLabelText.text = "Saving...";

        string str64MapImage = string.Empty;

        screenCapture.TakeScreenCapture();
        yield return new WaitUntil(() => screenCapture.doneTakeScreenCapture == true);

        LibPlacenote.Instance.SaveMap(
            (mapId) =>
            {
                LibPlacenote.Instance.StopSession();
                FeaturesVisualizer.clearPointcloud();

                mSaveMapId = mapId;
                mInitButtonPanel.SetActive(true);
                mMappingButtonPanel.SetActive(false);
                mExitButton.SetActive(false);

                LibPlacenote.MapMetadataSettable metadata = new LibPlacenote.MapMetadataSettable();
                metadata.name = RandomName.Get();
                mLabelText.text = "Saved Map Name: " + metadata.name;

                JObject userdata = new JObject();
                metadata.userdata = userdata;

                JObject shapeList = shapeManager.Shapes2JSON();
                userdata["shapeList"] = shapeList;
                GetComponent<ShapeManager>().ClearShapes();

                JObject backgroundMarker = afeBaseMarkerManager.BackgroundMarker2JSON();
                userdata["backgroundMarker"] = backgroundMarker;

                ///Test gởi đi/lấy về gói info gồm hình ảnh với PlaceNote luôn
                ///=============================================================
                ScreenCaptureData captureData = new ScreenCaptureData();
                captureData.texName = mSelectedMapId;
                captureData.string64 = screenCapture.textureMethod.m_encodedData;
                JObject captureObject = JObject.FromObject(captureData);
                userdata["capture"] = captureObject;
                ///=============================================================

                if (useLocation)
                {
                    metadata.location = new LibPlacenote.MapLocation();
                    metadata.location.latitude = locationInfo.latitude;
                    metadata.location.longitude = locationInfo.longitude;
                    metadata.location.altitude = locationInfo.altitude;
                }
                LibPlacenote.Instance.SetMetadata(mapId, metadata, (success) =>
                {
                    if (success)
                    {
                        Debug.Log("Meta data successfully saved");
                    }
                    else
                    {
                        Debug.Log("Meta data failed to save");
                    }
                });
                mCurrMapDetails = metadata;
            },
            (completed, faulted, percentage) =>
            {
                if (completed)
                {
                    mLabelText.text = "Upload Complete:" + mCurrMapDetails.name;
                }
                else if (faulted)
                {
                    mLabelText.text = "Upload of Map Named: " + mCurrMapDetails.name + "faulted";
                }
                else
                {
                    mLabelText.text = "Uploading Map Named: " + mCurrMapDetails.name + "(" + percentage.ToString("F2") + "/1.0)";
                }
            }
        );

        backgroundMethod.Hide();

        if (isDebug) Debug.Log("Done C_OnSaveMapClick");
        doneOnSaveMapClick = true;

        yield break;
    }

    public void OnPose(Matrix4x4 outputPose, Matrix4x4 arkitPose) { }

    [Header("On Status Change")]
    public ShapeManager shapeManager = null;
    public AFEBaseMarkerManager afeBaseMarkerManager = null;

    public void OnStatusChange(LibPlacenote.MappingStatus prevStatus, LibPlacenote.MappingStatus currStatus)
    {
        Debug.Log("prevStatus: " + prevStatus.ToString() + " currStatus: " + currStatus.ToString());
        if (currStatus == LibPlacenote.MappingStatus.RUNNING && prevStatus == LibPlacenote.MappingStatus.LOST)
        {
            mLabelText.text = "Localized";
            shapeManager.LoadShapesJSON(mSelectedMapInfo.metadata.userdata);
            afeBaseMarkerManager.LoadBackgroundMarkerFromJSON(mSelectedMapInfo.metadata.userdata);
        }
        else if (currStatus == LibPlacenote.MappingStatus.RUNNING && prevStatus == LibPlacenote.MappingStatus.WAITING)
        {
            mLabelText.text = "Mapping: Tap to add Shapes";
        }
        else if (currStatus == LibPlacenote.MappingStatus.LOST)
        {
            mLabelText.text = "Searching for position lock";
        }
        else if (currStatus == LibPlacenote.MappingStatus.WAITING)
        {
            if (GetComponent<ShapeManager>().shapeObjList.Count != 0)
            {
                GetComponent<ShapeManager>().ClearShapes();
            }
        }
    }
}
