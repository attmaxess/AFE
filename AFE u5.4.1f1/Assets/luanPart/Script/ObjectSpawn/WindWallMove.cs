using UnityEngine;
using UniRx;
using Com.Beetsoft.AFE;
using System;

public class WindWallMove : ObjectMovement
{
    public float timeLive = 5;


    private void OnEnable()
    {
        Observable.Timer(TimeSpan.FromMilliseconds(timeLive * 1000)).TakeUntilDestroy(gameObject).TakeWhile(_ => gameObject.activeSelf).Subscribe(_ =>
        {
            GetComponent<ObjectElementSkillBehaviour>().ReturnPool();
        });
    }

    public override void MoveToDir(Vector3 startPos, Vector3 target)
    {
        transform.position = startPos;
        var _duration = duration;
        ObservableTween.Tween(transform.position, target, _duration
            , ObservableTween.EaseType.Linear).TakeUntilDestroy(gameObject)
            .TakeWhile(_ => gameObject.activeSelf)
            .Subscribe(pos =>
            {
                transform.position = pos;
            });

    }
}