using UnityEngine;
using UnityEngine.Animations;

public class IdleFMSState : BaseFMSAnimator
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Debug.Log("Idle OnStateEnter " + character.AttackRpc + " - " + character.idleRpc);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null) character = animator.GetComponentInParent<ThirdPersonControllerNET>();


        // condition transition
        if (character.AttackRpc)
        {
            animator.SetTrigger("Attack");
            return;
        }
        if (character.runRpc)
        {
            animator.SetTrigger("Run");
            return;
        }

        if (character.hitRpc)
        {
            animator.SetTrigger("Hit");
            return;
        }

        if (character.skill_1Rpc)
        {
            animator.SetTrigger("Skill1");
            return;
        }
        if (character.skill_2Rpc)
        {
            animator.SetTrigger("Skill2");
            return;
        }
        if (character.skill_3Rpc)
        {
            animator.SetTrigger("Skill3");
            return;
        }
        if (character.skill_4Rpc)
        {
            animator.SetTrigger("Skill4");
            return;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        character.AttackRpc = false;
        character.hitRpc = false;
        character.runRpc = false;
        character.skill_1Rpc = false;
        character.skill_2Rpc = false;
        character.skill_3Rpc = false;
        character.skill_4Rpc = false;

        Debug.Log("Idle OnStateExit " + character.AttackRpc + " - " + character.idleRpc);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineEnter IdleFMSState");
        base.OnStateMachineEnter(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineExit IdleFMSState");
        base.OnStateMachineExit(animator, stateMachinePathHash);
    }
}
