﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Beetsoft.Luan;

public class Snap2Character : MonoBehaviour
{
    public void Snap(Com.Beetsoft.Luan.CreateCharacter loadChar, GameObject snapPos)
    {
        StartCoroutine(C_Snap(loadChar, snapPos));
    }

    IEnumerator C_Snap(Com.Beetsoft.Luan.CreateCharacter loadChar, GameObject snapPos)
    {
        yield return new WaitUntil(() => loadChar.currentCharacter != null);

        float momentPlaneJoystick = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<PlaneJoystick>() != null || Time.time - momentPlaneJoystick > 2f);

        GameObject joystick = FindObjectOfType<PlaneJoystick>().gameObject;
        if (joystick == null && Time.time - momentPlaneJoystick > 2f)
        {
            yield break;            
        }

        joystick.transform.position = snapPos.transform.position;
        loadChar.currentCharacter.transform.position = snapPos.transform.position;

        yield break;
    }
}
