using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChangeIpf : MonoBehaviour
{
    public InputField ipf = null;
    public Slider slider = null;

    [ContextMenu("OnChangeValue")]
    public void OnChangeValue()
    {
        ipf.text = slider.value.ToString();
    }
}
