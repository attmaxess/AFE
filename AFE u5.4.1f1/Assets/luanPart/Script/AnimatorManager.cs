using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    private ThirdPersonControllerNET controller;


    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        controller = GetComponent<ThirdPersonControllerNET>();

        if (GetComponent<PhotonView>().isMine)
        {
            controller.attack += attackAnim;
            controller.skill1 += SkillAnim1;
            controller.skill2 += SkillAnim2;
            controller.skill3 += SkillAnim3;
            controller.skill4 += SkillAnim4;
            controller.idle += IdleAnim;

        }
    }

    private void IdleAnim()
    {
    }

    private void SkillAnim3()
    {
    }

    private void SkillAnim4()
    {
    }

    private void SkillAnim2()
    {
    }

    private void SkillAnim1()
    {
    }

    private void attackAnim()
    {
    }

}
