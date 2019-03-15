using UnityEngine;
using UnityEngine.Animations;

public class AttackFMSAnimator : BaseFMSAnimator
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        character.AttackRpc = false;
        animator.ResetTrigger("Attack");
        Debug.Log("Attack State Enter " + character.AttackRpc);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null) character = animator.GetComponentInParent<ThirdPersonControllerNET>();

        // condition transition
        if (character.idleRpc)
        {
            animator.SetTrigger("Idle");
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
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Debug.Log("Attack OnStateExit " + character.AttackRpc + " - " + character.idleRpc);
    }


}
