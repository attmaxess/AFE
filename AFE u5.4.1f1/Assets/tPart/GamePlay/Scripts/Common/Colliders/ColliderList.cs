using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderList : MonoBehaviour
{
    [Header("Colider List")]
    public List<Collider> colList = new List<Collider>();

    void AddOnce(Collider col)
    {
        if (colList.FindIndex((x) => x == col) == -1) colList.Add(col);
    }

    void Remove(Collider col)
    {
        if (colList.FindIndex((x) => x == col) != -1) colList.Remove(col);
    }

    #region Trigger and Collision

    private void OnTriggerEnter(Collider other)
    {
        AddOnce(other);
    }

    private void OnTriggerStay(Collider other)
    {
        AddOnce(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Remove(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddOnce(collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        AddOnce(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        Remove(collision.collider);
    }

    #endregion Trigger and Collision

    [Header("Shake")]
    public bool doneShake = true;

    [ContextMenu("Shake")]
    public void Shake()
    {
        StartCoroutine(C_Shake());
    }

    IEnumerator C_Shake()
    {
        doneShake = false;

        transform.Translate(Vector3.forward * 0.01f);
        yield return new WaitForSeconds(0.3f);
        transform.Translate(-Vector3.forward * 0.01f);

        doneShake = true;

        yield break;
    }

    //[Header("Sphere Raycast")]

    [ContextMenu("SphereCast")]
    public void SphereCast()
    {
        RaycastHit hit;
        bool isHit = Physics.SphereCast(transform.position, 0.5f, transform.up, out hit, 3f);
        if (isHit) AddOnce(hit.collider);
    }

    [Header("Draw")]
    public bool useUpdate = false;

    void Update()
    {
        if (!useUpdate) return;
        Debug.DrawRay(transform.position, transform.up, Color.green, Time.deltaTime);
    }
}
