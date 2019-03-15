using UnityEngine;

public class AnimatorHandler : Photon.MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Animator Animator => animator ? animator : animator = GetComponent<Animator>();

    private void Start()
    {
        
    }
}