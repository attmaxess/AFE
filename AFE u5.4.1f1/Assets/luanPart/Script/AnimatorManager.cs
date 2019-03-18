using Photon.Pun;
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

        if (GetComponent<PhotonView>().IsMine)
        {
            controller.OnAttack += attackAnim;
            controller.OnSkill1 += SkillAnim1;
            controller.OnSkill2 += SkillAnim2;
            controller.OnSkill3 += SkillAnim3;
            controller.OnSkill4 += SkillAnim4;
            controller.OnIdle += IdleAnim;

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
