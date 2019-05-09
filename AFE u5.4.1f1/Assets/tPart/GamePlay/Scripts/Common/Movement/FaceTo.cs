using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTo : MonoBehaviour
{
    [Header("Params")]
    public float speed = 1f;
    public float deltaEur = 0.1f;

    [Header("Progess")]
    public bool doneToEur = true;

    [ContextMenu("ToEur0")]
    public void ToEur0()
    {
        ToEur(Vector3.zero);
    }

    public void ToEur(Vector3 eur)
    {
        StopAllC();
        StartCoroutine(C_ToEur(eur));
    }

    IEnumerator C_ToEur(Vector3 eur)
    {
        doneToEur = false;

        while ((transform.eulerAngles - eur).magnitude > deltaEur)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, eur, speed);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = eur;
        doneToEur = true;
        yield break;
    }

    public bool doneToQua = true;

    [ContextMenu("ToQua0")]
    public void ToQua0()
    {
        ToQua(Quaternion.identity);
    }

    public void ToQua(Quaternion qua)
    {
        StopAllC();
        StartCoroutine(C_ToQua(qua));
    }

    IEnumerator C_ToQua(Quaternion qua)
    {
        doneToQua = false;
                
        while (Quaternion.Angle(transform.rotation, qua) > deltaEur)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, qua, speed);
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = qua;
        doneToQua = true;
        yield break;
    }

    public bool doneToEurX = true;

    [ContextMenu("ToEurX0")]
    public void ToEurX0()
    {
        ToEurX(0);
    }

    public void ToEurX(float eurX)
    {
        StopAllC();
        StartCoroutine(C_ToEurX(eurX));
    }

    IEnumerator C_ToEurX(float eurX)
    {
        doneToEurX = false;

        while (transform.rotation.x - eurX > deltaEur)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(eurX, transform.eulerAngles.y, transform.eulerAngles.z), speed);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(eurX, transform.eulerAngles.y, transform.eulerAngles.z);
        doneToEurX = true;
        yield break;
    }

    public bool doneToEurY = true;

    [ContextMenu("ToEurY0")]
    public void ToEurY0()
    {
        ToEurY(0);
    }

    public void ToEurY(float eurY)
    {
        StopAllC();
        StartCoroutine(C_ToEurY(eurY));
    }

    IEnumerator C_ToEurY(float eurY)
    {
        doneToEurY = false;

        while (transform.rotation.y - eurY > deltaEur)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, eurY, transform.eulerAngles.z), speed);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, eurY, transform.eulerAngles.z);
        doneToEurY = true;
        yield break;
    }

    public bool doneToEurZ = true;

    [ContextMenu("ToEurZ0")]
    public void ToEurZ0()
    {
        ToEurZ(0);
    }

    public void ToEurZ(float eurZ)
    {
        StopAllC();
        StartCoroutine(C_ToEurZ(eurZ));
    }

    IEnumerator C_ToEurZ(float eurZ)
    {
        doneToEurZ = false;

        while (transform.rotation.z - eurZ > deltaEur)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, eurZ), speed);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, eurZ);
        doneToEurZ = true;
        yield break;
    }

    [ContextMenu("StopAllC")]
    public void StopAllC()
    {
        StopAllCoroutines();
    }
}
