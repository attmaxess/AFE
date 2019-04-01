using UnityEngine;
using UniRx;

public class BlowUpObject : MonoBehaviour
{

    public float timeUp = 1;
    public float timeDown = 1;
    bool isBlowUp;
    public void BlowUp()
    {
        if (isBlowUp) return;
        isBlowUp = true;

        var dispose =  ObservableTween.Tween(transform.position, transform.position + Vector3.up, timeUp + timeDown, ObservableTween.EaseType.Linear, ObservableTween.LoopType.PingPong)
            .DoOnCompleted(() =>
            {
                isBlowUp = false;
            })
            .Subscribe(pos =>
            {
                transform.position = pos;
            });

        dispose?.Dispose();

    }
}