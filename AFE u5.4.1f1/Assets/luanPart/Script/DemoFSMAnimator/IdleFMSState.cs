using UnityEngine;
using UnityEngine.Animations;

public class IdleFMSState : BaseFMSAnimator
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Debug.Log("Idle OnStateEnter " + character.attack + " - " + character.idle);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null) character = animator.GetComponentInParent<ThirdPersonControllerNET>();


        // condition transition
        if (character.attack)
        {
            animator.SetTrigger("Attack");
            return;
        }
        if (character.run)
        {
            animator.SetTrigger("Run");
            return;
        }

        if (character.hit)
        {
            animator.SetTrigger("Hit");
            return;
        }

        if (character.skill_1)
        {
            animator.SetTrigger("Skill1");
            return;
        }
        if (character.skill_2)
        {
            animator.SetTrigger("Skill2");
            return;
        }
        if (character.skill_3)
        {
            animator.SetTrigger("Skill3");
            return;
        }
        if (character.skill_4)
        {
            animator.SetTrigger("Skill4");
            return;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        character.attack = false;
        character.hit = false;
        character.run = false;
        character.skill_1 = false;
        character.skill_2 = false;
        character.skill_3 = false;
        character.skill_4 = false;

        Debug.Log("Idle OnStateExit " + character.attack + " - " + character.idle);
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
