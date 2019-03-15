using UnityEngine;

public class RunFMSAnimator : BaseFMSAnimator
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        character.runRpc = false;
        animator.ResetTrigger("Run");
        Debug.Log("Attack State Enter " + character.AttackRpc);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // condition transition
        if (character.idleRpc)
        {
            animator.SetTrigger("Idle");
            return;
        }
        if (character.AttackRpc)
        {
            animator.SetTrigger("Attack");
            return;
        }

        if (character.hitRpc)
        {
            animator.SetTrigger("Hit");
            return;
        }
    }
}
