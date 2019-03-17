using AFE.Enumerables;
using Com.Beetsoft.AFE;
using ControlFreak2;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using AnimationState = AFE.Enumerables.AnimationState;
using MonoBehaviour = Photon.MonoBehaviour;

public class TestTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private JoystickInputFilter joystickInputFilter;
    [SerializeField] private AnimatorHandler animatorHandler;

    private Animator Animator => animator ? animator : animator = GetComponent<Animator>();

    private JoystickInputFilter JoystickInputFilter => joystickInputFilter;

    private AnimatorHandler AnimatorHandler => animatorHandler;

    private void Awake()
    {
        AnimatorHandler.SetInputFilterObserver(JoystickInputFilter);
    }

    // Use this for initialization
    private void Start()
    {
        // if (!photonView.isMine) return;

        this.OnKeyDownAsObservable(KeyCode.Q)
            .Subscribe(_ => JoystickInputFilter.Spell1(Vector3.zero));

        this.OnKeyDownAsObservable(KeyCode.W)
            .Subscribe(_ => JoystickInputFilter.Spell2(Vector3.zero));

        this.OnKeyDownAsObservable(KeyCode.E)
            .Subscribe(_ => JoystickInputFilter.Spell3(Vector3.zero));

        this.OnKeyDownAsObservable(KeyCode.R)
            .Subscribe(_ => JoystickInputFilter.Spell4(Vector3.zero));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => JoystickInputFilter.BasicAttack(gameObject));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Subscribe(_ => JoystickInputFilter.Run(Vector3.zero));

        this.OnKeyDownAsObservable(KeyCode.Space)
            .Subscribe(_ => JoystickInputFilter.Idle(Vector3.zero));
    }
}