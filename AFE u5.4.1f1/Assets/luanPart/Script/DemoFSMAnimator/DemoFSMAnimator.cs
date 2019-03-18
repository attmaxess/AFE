using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoFSMAnimator : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("OnStateEnter DemoFSMAnimator");
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.name);
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineEnter DemoFSMAnimator");
        base.OnStateMachineEnter(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineExit DemoFSMAnimator");
        base.OnStateMachineExit(animator, stateMachinePathHash);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("OnStateExit DemoFSMAnimator");
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

}
