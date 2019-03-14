using UniRx;
using UniRx.Triggers;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class TestTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Animator Animator => animator;

    // Use this for initialization
    private void Start()
    {
        if (!photonView.isMine) return;

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

public static class Constant
{
    public static class Animation
    {
        public static string Attack { get; } = "Attack";
        public static string AttackToIdle { get; } = "Attack To Idle";

        public static string Q { get; } = "Q";
        public static string W { get; } = "W";
        public static string E { get; } = "E";
        public static string R { get; } = "R";

        public static string Idle { get; } = "Idle";
        public static string QToIdle { get; } = "Q To Idle";

        public static string Run { get; } = "Run";
        public static string EToRun { get; } = "E To Run";
        public static string QToRun { get; } = "Q To Run";

        public static string Death { get; } = "Death";
    }
}