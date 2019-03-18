using UnityEngine;

public class BaseFMSAnimator : StateMachineBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake BaseFMSAnimator");
    }


    protected ThirdPersonControllerNET character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null) character = animator.GetComponentInParent<ThirdPersonControllerNET>();
    }

}
