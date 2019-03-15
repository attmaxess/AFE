using ControlFreak2;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class TestTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Animator Animator => animator;

    private int IndexAttack { get; set; } = 0;

    // Use this for initialization
    private void Start()
    {
        // if (!photonView.isMine) return;

        var attackSmb = Animator.GetBehaviour<ObservableAttackSmb>();
        attackSmb.OnStateMachineEnterAsObservable()
            .Subscribe(_ => Animator.SetInteger(Constant.Animation.AttackInt, ++IndexAttack));

        attackSmb.OnStateEnterAsObservable()
            .Where(_ => IndexAttack == Constant.Yasuo.AttackClipAmount)
            .Subscribe(_ => IndexAttack = 0);

        this.OnKeyDownAsObservable(KeyCode.Q)
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.Q));

        this.OnKeyDownAsObservable(KeyCode.W)
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.W));

        this.OnKeyDownAsObservable(KeyCode.E)
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.E));

        this.OnKeyDownAsObservable(KeyCode.R)
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.R));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.Attack));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Subscribe(_ => Animator.SetTrigger(Constant.Animation.Run));
    }
}