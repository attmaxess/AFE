using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Photon.Pun;

public class HealthBarPlayer : MonoBehaviour, IPunObservable,
    IInitialize<IChampionConfig>
{
    public Slider slider;

    IChampionConfig ChampionConfig;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            stream.SendNext(slider.value);
        }
        else
        {
            slider.value = (float)stream.ReceiveNext();
        }
    }

    public void UpdateHealth(float percent)
    {
        slider.value = Mathf.Lerp(0, 1, percent);
    }

    void IInitialize<IChampionConfig>.Initialize(IChampionConfig init)
    {
        ChampionConfig = init;
    }

    private void Start()
    {
        float maxHealth = 0;
        ChampionConfig.Health
            .Subscribe(_ =>
            {
                if (_ > maxHealth)
                    maxHealth = _;
                UpdateHealth(_ / maxHealth);
            });
    }
}
