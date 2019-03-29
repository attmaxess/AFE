using UnityEngine;
using UniRx;

public class BlowUpObject : MonoBehaviour
{

    public float timeUp = 1;
    public float timeDown = 1;
    public void BlowUp()
    {
        ObservableTween.Tween(transform.position, transform.position + Vector3.up, timeUp, ObservableTween.EaseType.Linear)
            .DoOnCompleted(() =>
            {
                ObservableTween.Tween(transform.position + Vector3.up, transform.position, timeDown, ObservableTween.EaseType.Linear)
                .Subscribe(_ =>
                {
                    transform.position = _;
                });
            })
            .Subscribe(_ =>
            {
                transform.position = _;
            });

    }
}