using UnityEngine;

public class HitFMSAnimator : BaseFMSAnimator
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // condition transition
        if (character.idle)
        {
            animator.SetTrigger("Idle");
            return;
        }
        if (character.run)
        {
            animator.SetTrigger("Attack");
            return;
        }
    }
}