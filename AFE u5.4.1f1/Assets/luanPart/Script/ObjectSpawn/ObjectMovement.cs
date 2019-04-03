using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Com.Beetsoft.AFE;

public interface IMovable
{
    void MoveToDir(Vector3 startPos, Vector3 target);
}

public interface ITriggerObject
{
    void SetIdIgnore(int id);
}

public class ObjectMovement : MonoBehaviour, IMovable
{
    public float duration = 2;


    public virtual void MoveToDir(Vector3 startPos, Vector3 target)
    {
        transform.position = startPos;
        var _duration = duration;
        ObservableTween.Tween(transform.position, target, _duration
            , ObservableTween.EaseType.Linear, ObservableTween.LoopType.None
            , () =>
         {
             var twist = GetComponent<Twist>();
             if (twist != null)
                 twist.ReturnPool();
         })
            .TakeWhile(_ => gameObject.activeSelf)
            .Subscribe(pos =>
            {
                transform.position = pos;
            });
    }
}
