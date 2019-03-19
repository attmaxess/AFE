using System;
using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using Photon.Pun;
using UniRx;
using UnityEngine;
using AnimationState = AFE.Enumerables.AnimationState;
using Random = UnityEngine.Random;

namespace Com.Beetsoft.AFE
{
    public class AnimatorHandler : MonoBehaviourPun
    {
        [SerializeField] private Animator animator;

        private Animator Animator => animator ? animator : animator = GetComponent<Animator>();

        private IJoystickInputFilterObserver InputFilterObserver { get; set; }

        private IReactiveProperty<bool> IsInStateSpell1 { get; } = new ReactiveProperty<bool>();
        private IReactiveProperty<bool> IsInStateSpell2 { get; } = new ReactiveProperty<bool>();
        private IReactiveProperty<bool> IsInStateSpell3 { get; } = new ReactiveProperty<bool>();
        private IReactiveProperty<bool> IsInStateSpell4 { get; } = new ReactiveProperty<bool>();

        private bool IsNotExitState => IsInStateSpell1.Value
                                       || IsInStateSpell2.Value
                                       || IsInStateSpell3.Value
                                       || IsInStateSpell4.Value;

        public void SetInputFilterObserver(IJoystickInputFilterObserver joystickInputFilterObserver)
        {
            InputFilterObserver = joystickInputFilterObserver;
        }

        private void Start()
        {
            if (!photonView.IsMine) return;

            InputFilterObserver.OnRunAsObservable()
                .Where(_ => !IsNotExitState)
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.Run));

            InputFilterObserver.OnIdleAsObservable()
                .Where(_ => !IsNotExitState)
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.Idle));

            InputFilterObserver.OnBasicAttackAsObservable()
                .Where(_ => !IsNotExitState)
                .Subscribe(_ => WillEnterStateAttack());

            InputFilterObserver.OnSpell1AsObservable()
                .Where(_ => !IsNotExitState)
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.Q));

            InputFilterObserver.OnSpell2AsObservable()
                .Where(_ => !(IsInStateSpell3.Value || IsInStateSpell4.Value))
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.W));

            InputFilterObserver.OnSpell3AsObservable()
                .Where(_ => !IsInStateSpell4.Value)
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.E));

            InputFilterObserver.OnSpell4AsObservable()
                .Where(_ => !(IsInStateSpell1.Value || IsInStateSpell3.Value))
                .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.R));

            HandleIdleState();
            HandleSwitchToStateWeaponIn();
            HandleAttackState();
            HandleSpell1State();
            HandleSpell1Dash();
            HandleSpell2State();
            HandleSpell3State();
            HandleSpell4State();
        }

        private void SwitchToStateWeaponOut()
        {
            Animator.SetInteger(Constant.AnimationPram.IdleInInt, (int) AnimationState.IdleIn.None);
            Animator.SetInteger(Constant.AnimationPram.RunOutInt, (int) AnimationState.RunOut.RunOut);
            Animator.SetInteger(Constant.AnimationPram.IdleOutInt, (int) AnimationState.IdleOut.IdleOut);
            Animator.SetInteger(Constant.AnimationPram.RunOutInt, (int) AnimationState.RunOut.RunOut);
        }

        private void SwitchToStateWeaponIn()
        {
            BasicAttackIndex = AnimationState.BasicAttack.Hit1;
            Animator.SetInteger(Constant.AnimationPram.IdleInInt, (int) AnimationState.IdleIn.IdleIn);
            Animator.SetInteger(Constant.AnimationPram.RunOutInt, (int) AnimationState.RunOut.None);
            Animator.SetInteger(Constant.AnimationPram.IdleOutInt, (int) AnimationState.IdleOut.None);
            Animator.SetInteger(Constant.AnimationPram.RunOutInt, (int) AnimationState.RunOut.None);
        }

        #region HandleIdleState

        private void HandleIdleState()
        {
            Observable.Interval(TimeSpan.FromSeconds(15f))
                .Subscribe(_ => Animator.SetInteger(Constant.AnimationPram.IdleInt, Random.Range(1, 5)))
                .AddTo(this);
        }

        #endregion

        #region HandleSwitchToStateWeaponIn

        private void HandleSwitchToStateWeaponIn()
        {
            Animator.OnStateEnterAsObservables(Animator.StringToHash(Constant.SMB.RunIn))
                .ForEach(x => x.Subscribe(_ => SwitchToStateWeaponIn()));

            Animator.OnStateEnterAsObservables(Animator.StringToHash(Constant.SMB.IdleIn))
                .ForEach(x => x.Subscribe(_ => SwitchToStateWeaponIn()));
        }

        #endregion

        #region HandleAttackState

        private AnimationState.BasicAttack BasicAttackIndex { get; set; } = AnimationState.BasicAttack.Hit1;

        private void HandleAttackState()
        {
            var attackSmb = Animator.GetBehaviour<ObservableAttackSmb>();
            attackSmb.OnStateMachineEnterAsObservable()
                .Subscribe(_ => { SwitchToStateWeaponOut(); });
        }

        private void WillEnterStateAttack()
        {
            Animator.SetTriggerWithBool(Constant.AnimationPram.Attack);
            Animator.SetInteger(Constant.AnimationPram.AttackInt, (int) BasicAttackIndex);
            BasicAttackIndex++;
            if (BasicAttackIndex > AnimationState.BasicAttack.Hit4)
                BasicAttackIndex = AnimationState.BasicAttack.Hit1;
        }

        #endregion

        #region HandleSpell1State

        private AnimationState.Spell1 FeatureIndexSpell1State { get; set; } = AnimationState.Spell1.Spell1A;

        private void HandleSpell1State()
        {
            var spell1Smb = Animator.GetBehaviour<ObservableSpell1Smb>();
            spell1Smb.OnStateMachineEnterAsObservable()
                .Subscribe(_ =>
                {
                    SwitchToStateWeaponOut();
                    IsInStateSpell1.Value = true;
                });

            spell1Smb.OnStateMachineExitAsObservable()
                .Subscribe(_ =>
                {
                    FeatureIndexSpell1State++;
                    if ((int) FeatureIndexSpell1State == Constant.Yasuo.QClipAmount)
                        FeatureIndexSpell1State = AnimationState.Spell1.Spell1A;
                    IsInStateSpell1.Value = false;
                    Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State);
                    Animator.SetBool(Constant.AnimationPram.IdleBool, true);
                });
        }

        private void HandleSpell1Dash()
        {
            var spell3Smb = Animator.GetBehaviour<ObservableSpell3Smb>();
            spell3Smb.OnStateMachineEnterAsObservable()
                .SelectMany(_ => InputFilterObserver.OnSpell1AsObservable())
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(Constant.Yasuo.OffsetTimeSpell3AndSpell1)))
                .RepeatUntilDestroy(this)
                .Subscribe(_ =>
                {
                    Animator.SetInteger(Constant.AnimationPram.QInt, (int) AnimationState.Spell1.Spell1_Dash);
                    Animator.SetBool(Constant.AnimationPram.IdleBool, false);
                });
        }

        #endregion

        #region HandleSpell2State

        private void HandleSpell2State()
        {
            var spell2Smb = Animator.GetBehaviour<ObservableSpell2Smb>();
            spell2Smb.OnStateMachineEnterAsObservable()
                .Subscribe(_ =>
                {
                    IsInStateSpell2.Value = true;
                    SwitchToStateWeaponOut();
                });
            spell2Smb.OnStateMachineExitAsObservable()
                .Subscribe(_ => IsInStateSpell2.Value = false);
        }

        #endregion

        #region HandleSpell3State

        private void HandleSpell3State()
        {
            var spell3Smb = Animator.GetBehaviour<ObservableSpell3Smb>();
            spell3Smb.OnStateMachineEnterAsObservable()
                .Subscribe(_ =>
                {
                    IsInStateSpell3.Value = true;
                    SwitchToStateWeaponOut();
                });
            spell3Smb.OnStateMachineExitAsObservable()
                .Subscribe(_ => IsInStateSpell3.Value = false);
        }

        #endregion

        #region HandleSpell4State

        private void HandleSpell4State()
        {
            var spell4Smb = Animator.GetBehaviour<ObservableSpell4Smb>();
            spell4Smb.OnStateMachineEnterAsObservable()
                .Subscribe(_ =>
                {
                    IsInStateSpell4.Value = true;
                    FeatureIndexSpell1State = AnimationState.Spell1.Spell1A;
                    Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State);
                    SwitchToStateWeaponOut();
                });
            spell4Smb.OnStateMachineExitAsObservable()
                .Subscribe(_ => IsInStateSpell4.Value = false);
        }

        #endregion
    }
}