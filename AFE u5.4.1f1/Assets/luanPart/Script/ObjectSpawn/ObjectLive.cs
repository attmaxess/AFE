using UnityEngine;
using UniRx;
using Com.Beetsoft.AFE;
using System;

public class ObjectLive : MonoBehaviour
{
    public float timeLive = 5;
    private void OnEnable()
    {
        Observable.Timer(TimeSpan.FromMilliseconds(timeLive * 1000)).TakeWhile(_ => gameObject.activeSelf).Subscribe(_ =>
              {
                  GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
              });
    }
}