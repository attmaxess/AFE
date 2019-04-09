using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Photon.Pun;

public class HealthBarPlayer : MonoBehaviour
{
    public Slider slider;

    IChampionConfig ChampionConfig;

    public Transform character;

    void UpdateHealth(float percent)
    {
        slider.value = Mathf.Lerp(0, 1, percent);
    }

    public void SetInit(Transform character, IChampionConfig init, bool isMine)
    {
        this.character = character;
        ChampionConfig = init;
        SetPosition(isMine);
    }

    void SetPosition(bool isMine)
    {
        var rect = GetComponent<RectTransform>();
        rect.pivot = Vector2.one;
        if (!isMine)
        {
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector2(-20, 0);
        }
        else
        {
            rect.localScale = new Vector3(-1, 1, 1);
            rect.anchoredPosition = new Vector2(20, 0);
        }
    }

    private void Start()
    {
        float maxHealth = 0;
        ChampionConfig.Health
            .Subscribe(health =>
            {
                if (health > maxHealth)
                    maxHealth = health;
                UpdateHealth(health / maxHealth);
            });

        Observable.EveryLateUpdate().RepeatUntilDestroy(gameObject).Where(_ => this.character == null).Subscribe(_ =>
        {
            Destroy(gameObject);
        });
    }
}
