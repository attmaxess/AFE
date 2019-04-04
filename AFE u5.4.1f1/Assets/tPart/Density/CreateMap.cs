using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [Header("Holder")]
    public GameObject spherePrefab = null;
    public GameObject holderPrefab = null;
    public Transform currentHolder = null;

    [Header("Params")]
    public int countSphere = 300;
    public Vector2 border = new Vector2(-100, 100);

    [Header("Process")]
    public List<Transform> sphereList = new List<Transform>();

    [ContextMenu("RefreshSphereList")]
    public void RefreshSphereList()
    {
        List<SphereUnit> s_sphereList = currentHolder.GetComponentsInChildren<SphereUnit>().ToList();
        sphereList = new List<Transform>();
        for (int i = 0; i < s_sphereList.Count; i++) sphereList.Add(s_sphereList[i].transform);
    }

    [ContextMenu("CreateRandom")]
    public void CreateRandom()
    {
        if (currentHolder != null)
            if (Application.isPlaying) Destroy(currentHolder.gameObject); DestroyImmediate(currentHolder.gameObject);

        currentHolder = Instantiate(holderPrefab.gameObject, Vector3.zero, Quaternion.identity, null).transform;
        currentHolder.name = "SphereHolder";
        currentHolder.gameObject.SetActive(true);

        sphereList = new List<Transform>();
        for (int i = 0; i < countSphere; i++)
        {
            Transform currentSphere = Instantiate(spherePrefab.gameObject, Vector3.zero, Quaternion.identity, currentHolder).transform;
            currentSphere.name = "Sphere_" + i.ToString();
            currentSphere.gameObject.SetActive(true);
            currentSphere.position = new Vector3(Random.Range(border.x, border.y), Random.Range(border.x, border.y), Random.Range(border.x, border.y));

            sphereList.Add(currentSphere);
        }
    }

    [Header("DenseAll")]
    public float mulDense = 1f;

    [ContextMenu("DenseAll")]
    public void DenseAll()
    {
        for (int i = 0; i < sphereList.Count; i++)
        {
            sphereList[i].GetComponent<SphereUnit>().CalculateDensity(this.sphereList);
            sphereList[i].GetComponent<SphereUnit>().LocalSizeToDense(mulDense);
        }
    }

    [ContextMenu("DebugMaxDense")]
    public void DebugMaxDense()
    {
        float maxDense = -Mathf.Infinity;
        string sphereDebug = string.Empty;
        for (int i = 0; i < sphereList.Count; i++)
        {
            float currentDense = sphereList[i].GetComponent<SphereUnit>().currentDense;
            if (maxDense < currentDense)
            {
                maxDense = currentDense;
                sphereDebug = sphereList[i].name;
            };
        }

        Debug.Log("MaxDense " + maxDense + " at " + sphereDebug);
    }
}
