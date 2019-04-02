using System;
using AFE.Extensions;
using UniRx;
using UnityEngine;
using AnimationState = Com.Beetsoft.AFE.Enumerables.AnimationState;

namespace Com.Beetsoft.AFE
{
    public abstract class AttackBasicHandler : SkillHandler
    {
        [SerializeField] private AnimationState.BasicAttack maxHit = AnimationState.BasicAttack.Hit4;
        private AnimationState.BasicAttack BasicAttackIndex { get; set; } = AnimationState.BasicAttack.Hit1;

        protected AnimationState.BasicAttack MaxHit => maxHit;
        
        protected override void Start()
        {
            if (!photonView.IsMine) return;

            var applySkillTimer =
                Observable.Timer(TimeSpan.FromMilliseconds(Constant.Yasuo.TimeDelayApplyDamageAttackBasicMilliseconds));

            JoystickInputFilterObserver.OnBasicAttackAsObservable()
                .Do(_ => WillEnterStateAttack())
                .SelectMany(message =>
                    applySkillTimer.Select(_ => message))
                .Do(_ => WillExitStateAttack())
                .Subscribe(ApplyDamage);
        }

        protected abstract void ApplyDamage(IInputMessage message);

        protected virtual void WillEnterStateAttack()
        {
            Animator.SetTriggerWithBool(Constant.AnimationPram.Attack);
            Animator.SetInteger(Constant.AnimationPram.AttackInt, (int) BasicAttackIndex);
        }

        protected virtual void WillExitStateAttack()
        {
            BasicAttackIndex++;
            if (BasicAttackIndex > MaxHit)
                BasicAttackIndex = AnimationState.BasicAttack.Hit1;
        }
    }
}