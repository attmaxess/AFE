using AFE.Enumerables;
using Com.Beetsoft.AFE;
using ControlFreak2;
using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using AnimationState = AFE.Enumerables.AnimationState;

public class TestTriggerAnimation : MonoBehaviourPun
{
    [SerializeField] private Animator animator;
    [SerializeField] private JoystickInputFilter joystickInputFilter;
    [SerializeField] private AnimatorHandler animatorHandler;

    private Animator Animator => animator ? animator : animator = GetComponent<Animator>();

    private JoystickInputFilter JoystickInputFilter => joystickInputFilter;

    private AnimatorHandler AnimatorHandler => animatorHandler;

    private void Awake()
    {
        //AnimatorHandler.Initialize(JoystickInputFilter);
    }

    // Use this for initialization
    private void Start()
    {
        if (!photonView.IsMine) return;

        this.OnKeyDownAsObservable(KeyCode.Q)
            .Subscribe(_ => JoystickInputFilter.Spell1(new SkillMessage()));

        this.OnKeyDownAsObservable(KeyCode.W)
            .Subscribe(_ => JoystickInputFilter.Spell2(new SkillMessage()));

        this.OnKeyDownAsObservable(KeyCode.E)
            .Subscribe(_ => JoystickInputFilter.Spell3(new SkillMessage()));

        this.OnKeyDownAsObservable(KeyCode.R)
            .Subscribe(_ => JoystickInputFilter.Spell4(new SkillMessage()));


//        Observable.EveryUpdate()
//            .Where(_ => Input.GetMouseButtonDown(0))
//            .Subscribe(_ => JoystickInputFilter.BasicAttack(gameObject));

       /* Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Subscribe(_ => JoystickInputFilter.Run(new RunMessage(Vector3.forward)));

        this.OnKeyDownAsObservable(KeyCode.Space)
            .Subscribe(_ => JoystickInputFilter.Idle(new RunMessage(Vector3.zero)));  */
    }
}