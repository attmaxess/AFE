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
            this ObservableStateMachineTrigger observableStateMachineTrigger, string name)
        {
            return observableStateMachineTrigger.OnStateEnterAsObservable().TakeWhile(x => x.StateInfo.IsName(name));
        }

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateExitAsObservable(
            this ObservableStateMachineTrigger observableStateMachineTrigger, string name)
        {
            return observableStateMachineTrigger.OnStateExitAsObservable().TakeWhile(x => x.StateInfo.IsName(name));
        }

        public static IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateIkAsObservable(
            this ObservableStateMachineTrigger observableStateMachineTrigger, string name)
        {
            return observableStateMachineTrigger.OnStateExitAsObservable().TakeWhile(x => x.StateInfo.IsName(name));
        }

        public static List<ObservableStateMachineTrigger> GetObservableBehaviours(this Animator animator)
        {
            return animator.GetBehaviours<ObservableStateMachineTrigger>().ToList();
        }
    }
}