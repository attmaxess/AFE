using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap2Character : MonoBehaviour
{
    public void Snap(GameObject character, GameObject snapPos)
    {
        StartCoroutine(C_Snap(character, snapPos));
    }

    IEnumerator C_Snap(GameObject character, GameObject snapPos)
    {
        float momentPlaneJoystick = Time.time;
        yield return new WaitUntil(() => FindObjectOfType<PlaneJoystick>() != null || Time.time - momentPlaneJoystick > 2f);

        GameObject joystick = FindObjectOfType<PlaneJoystick>().gameObject;
        if (joystick == null && Time.time - momentPlaneJoystick > 2f)
        {
            yield break;            
        }

        joystick.transform.position = snapPos.transform.position;
        character.transform.position = snapPos.transform.position;

        yield break;
    }
}
