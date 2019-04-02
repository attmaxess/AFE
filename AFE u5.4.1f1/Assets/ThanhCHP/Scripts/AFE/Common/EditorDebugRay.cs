using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorDebugRay : MonoBehaviour
{
    [Header("Input")]

    [SerializeField] private LayerMask layerMaskTarget;
    private LayerMask LayerMaskTarget => layerMaskTarget;

    public Vector3 pos = Vector3.zero;
    public Vector3 dir = Vector3.one;
    public float distance = 10f;

    [ContextMenu("RayIt")]
    public void RayIt()
    {
        RaycastHit hit;
        bool boolHit = Physics.Raycast(transform.position, dir, out hit, distance, layerMaskTarget);
        if (boolHit)
        {
            Debug.Log(hit.point);
        }
        else
        {
            Debug.Log("khong hit");
        }

        Debug.Log(gameObject.GetAllReceiverDamageNearestByRayCastAll(dir, distance, LayerMaskTarget));
    }

    [Header("Input")]
    public bool useUpdate = true;
    private object inputMessage;

    private void Update()
    {
        if (!useUpdate) return;
        Debug.DrawRay(transform.position, dir.normalized * distance, Color.green);
    }
}
