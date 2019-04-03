using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereUnit : MonoBehaviour
{
    public float currentDense = 0;

    public void CalculateDensity(List<Transform> map)
    {
        float dense = 0;
        for (int i = 0; i < map.Count; i++)
        {
            float magnitute = (transform.position - map[i].position).magnitude;
            if (magnitute < 0.01f)
            {
                Debug.Log(map[i].name + " and " + transform.name + " too close!!");
            }
            else
            {
                dense += 1 / (magnitute);
            }
        }
        currentDense = dense;
    }

    [ContextMenu("LocalSizeToDense")]
    public void LocalSizeToDense(float mul)
    {
        float dimension = currentDense * mul;
        transform.localScale = new Vector3(dimension, dimension, dimension);
    }
}
