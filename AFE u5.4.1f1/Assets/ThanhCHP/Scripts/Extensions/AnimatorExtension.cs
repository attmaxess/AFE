using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using ExtraLinq;
using UniRx.Triggers;

namespace AFE.Extensions
{
    public static partial class AnimatorExtension
    {
        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateEnterAsObservable(
            this ObservableStateMachineTrigger observableStateMachineTrigger, int shortNameHash)
        {
            return observableStateMachineTrigger.OnStateEnterAsObservable()
                .TakeWhile(x => x.StateInfo.shortNameHash == shortNameHash);
        }

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateExitAsObservable(
            this ObservableStateMachineTrigger observableStateMachineTrigger, int shortNameHash)
        {
            return observableStateMachineTrigger.OnStateExitAsObservable()
                .TakeWhile(x => x.StateInfo.shortNameHash == shortNameHash);
        }

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateIKAsObservable(
            this ObservableStateMachineTrigger observableStateMachineTrigger, int shortNameHash)
        {
            return observableStateMachineTrigger.OnStateIKAsObservable()
                .TakeWhile(x => x.StateInfo.shortNameHash == shortNameHash);
        }

        public static List<IObservable<ObservableStateMachineTrigger.OnStateInfo>> OnStateEnterAsObservables(
            this Animator animator, int shortNameHash)
        {
            return animator.GetObservableBehaviours().Select(x => x.OnStateEnterAsObservable(shortNameHash)).ToList();
        }

        public static List<IObservable<ObservableStateMachineTrigger.OnStateInfo>> OnStateExitAsObservables(
            this Animator animator, int shortNameHash)
        {
            return animator.GetObservableBehaviours().Select(x => x.OnStateExitAsObservable(shortNameHash)).ToList();
        }

        public static List<IObservable<ObservableStateMachineTrigger.OnStateInfo>> OnStateIKAsObservables(
            this Animator animator, int shortNameHash)
        {
            return animator.GetObservableBehaviours().Select(x => x.OnStateIKAsObservable(shortNameHash)).ToList();
        }

        public static IEnumerable<ObservableStateMachineTrigger> GetObservableBehaviours(this Animator animator)
        {
            return animator.GetBehaviours<ObservableStateMachineTrigger>();
        }

        public static void SetTriggerWithBool(this Animator animator, int id)
        {
            animator.SetBool(id, true);
            Observable.Timer(TimeSpan.FromMilliseconds(250))
                .Subscribe(_ => animator.SetBool(id, false));
        }

        public static void SetTriggerWithBool(this Animator animator, string name)
        {
            animator.SetBool(name, true);
            Observable.Timer(TimeSpan.FromMilliseconds(250))
                .Subscribe(_ => animator.SetBool(name, false));
        }
    }
}