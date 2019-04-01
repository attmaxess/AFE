using UnityEngine;
using UniRx;

public class BlowUpObject : MonoBehaviour
{

    public float timeUp = 1;
    public float timeDown = 1;
    bool isBlowUp;
    System.IDisposable dispose;
    public void BlowUp()
    {
        if (isBlowUp) return;
        isBlowUp = true;

        dispose = ObservableTween.Tween(transform.position, transform.position + Vector3.up, timeUp + timeDown, ObservableTween.EaseType.Linear, ObservableTween.LoopType.PingPong, () =>
        {
            dispose?.Dispose();
            isBlowUp = false;
        })
            .DoOnCompleted(() =>
            {
            })
            .Subscribe(pos =>
            {
                transform.position = pos;
            });


    }
}