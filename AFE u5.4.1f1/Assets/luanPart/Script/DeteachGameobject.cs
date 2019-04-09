using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteachGameobject : MonoBehaviour
{

    public bool noParent;
    public bool upParent;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (upParent)
        {
            transform.parent = transform.parent.parent;
            transform.SetSiblingIndex(1);
            yield break;
        }
        if (noParent)
        {
            transform.parent = null;
            yield break;
        }
    }
}
