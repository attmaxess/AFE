using UnityEngine;

public class HitFMSAnimator : BaseFMSAnimator
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // condition transition
        if (character.idleRpc)
        {
            animator.SetTrigger("Idle");
            return;
        }
        if (character.runRpc)
        {
            animator.SetTrigger("Attack");
            return;
        }
    }
}