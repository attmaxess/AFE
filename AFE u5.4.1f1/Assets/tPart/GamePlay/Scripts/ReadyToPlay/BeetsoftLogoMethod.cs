using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetsoftLogoMethod : MonoBehaviour
{
    [Header("0 goto/goback 1")]
    public float waitTime = 1f;
    public float waitSpeed = 9f;

    public void GoTo0WaitBack1()
    {
        if (!Application.isPlaying) return;
        if (!BeetsoftLogo.Instance.gameObject.activeSelf) return;
        BeetsoftLogo.Instance.StopAllC_();
        BeetsoftLogo.Instance.ResetDefaut();
        StartCoroutine(C_GoToWaitBack(0, 1));
    }

    [Header("GoToWaitBack")]
    public bool doneGoToWaitBack = true;

    public void GoTo1WaitBack0()
    {
        if (!Application.isPlaying) return;
        if (!BeetsoftLogo.Instance.gameObject.activeSelf) return;
        BeetsoftLogo.Instance.StopAllC_();
        BeetsoftLogo.Instance.ResetDefaut();
        StartCoroutine(C_GoToWaitBack(1, 0));
    }

    IEnumerator C_GoToWaitBack(float to, float back)
    {
        doneGoToWaitBack = false;

        float temWaitSpeed = BeetsoftLogo.Instance.alphaSpeed;
        BeetsoftLogo.Instance.alphaSpeed = waitSpeed;

        BeetsoftLogo.Instance.StartCoroutine(BeetsoftLogo.Instance.C_ToAlPha(to));
        yield return new WaitUntil(() => BeetsoftLogo.Instance.doneC_ToAlPha == true);

        yield return new WaitForSeconds(waitTime);

        BeetsoftLogo.Instance.StartCoroutine(BeetsoftLogo.Instance.C_ToAlPha(back));
        yield return new WaitUntil(() => BeetsoftLogo.Instance.doneC_ToAlPha == true);

        BeetsoftLogo.Instance.alphaSpeed = temWaitSpeed;

        doneGoToWaitBack = true;

        yield break;
    }

    [ContextMenu("Alpha0")]
    public void Alpha0()
    {
        BeetsoftLogo.Instance.Alpha0();
    }

    [ContextMenu("InstantAlpha0")]
    public void InstantAlpha0()
    {
        BeetsoftLogo.Instance.InstantAlpha0();
    }

    [ContextMenu("Alpha1")]
    public void Alpha1()
    {
        BeetsoftLogo.Instance.Alpha1();
    }

    [ContextMenu("InstantAlpha1")]
    public void InstantAlpha1()
    {
        BeetsoftLogo.Instance.InstantAlpha1();
    }    
}
