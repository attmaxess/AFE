using UnityEngine;

public class RunFMSAnimator : BaseFMSAnimator
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        character.run = false;
        animator.ResetTrigger("Run");
        Debug.Log("Attack State Enter " + character.attack);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // condition transition
        if (character.idle)
        {
            animator.SetTrigger("Idle");
            return;
        }
        if (character.attack)
        {
            animator.SetTrigger("Attack");
            return;
        }

        if (character.hit)
        {
            animator.SetTrigger("Hit");
            return;
        }
    }
}
