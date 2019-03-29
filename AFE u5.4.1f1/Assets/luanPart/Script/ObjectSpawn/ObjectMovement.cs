using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public interface IMovable
{
    void MoveToDir(Vector3 startPos, Vector3 dir);
}

public interface ITriggerObject
{
    void SetIdIgnore(int id);
}

public class ObjectMovement : MonoBehaviour, IMovable
{
    public bool moveTime;
    public bool moveRange;
    public float speed = 1;
    public float timeMove = 2;
    public float range = 3;

    void OnEnable()
    {
        //transform.localScale = Vector3.one;
    }

    public void MoveToDir(Vector3 startPos, Vector3 dir)
    {
        transform.localScale = Vector3.one;
        var _timeMove = timeMove;
        transform.position = startPos;
        if (moveTime)
            Observable.EveryUpdate().TakeWhile(_ => _timeMove > 0).Subscribe(_ =>
              {
                  _timeMove -= Time.deltaTime;
                  transform.position += dir * speed * Time.deltaTime;
              });
        else if (moveRange)
        {
            Vector3 target = transform.position + dir * range;
            ObservableTween.Tween(transform.position, target, _timeMove, ObservableTween.EaseType.Linear)
                .TakeWhile(_ => gameObject.activeSelf)
                .Subscribe(pos =>
             {
                 transform.position = pos;
             });
        }
        else
            Observable.EveryUpdate()
                .TakeWhile(_ => gameObject.activeSelf)
                .Subscribe(_ =>
             {
                 transform.position += dir * speed * Time.deltaTime;
             });
    }
}
