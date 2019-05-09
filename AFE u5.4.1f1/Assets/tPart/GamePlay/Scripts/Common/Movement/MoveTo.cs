using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [Header("Progress")]
    public float speed = 1f;
    public float deltaDis = 0.1f;

    public bool doneMoveTo = true;

    [ContextMenu("MoveTo0")]
    public void MoveTo0()
    {
        MoveToPos(Vector3.zero);
    }

    public void MoveToPos(Vector3 pos)
    {
        StopAllC();
        StartCoroutine(C_MoveToPos(pos));
    }

    IEnumerator C_MoveToPos(Vector3 pos)
    {
        doneMoveTo = false;

        while ((transform.position - pos).magnitude > deltaDis)
        {
            transform.position = Vector3.Lerp(transform.position, pos, speed);
            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
        doneMoveTo = true;
        yield break;
    }

    [ContextMenu("StopAllC")]
    public void StopAllC()
    {
        StopAllCoroutines();
    }
}
