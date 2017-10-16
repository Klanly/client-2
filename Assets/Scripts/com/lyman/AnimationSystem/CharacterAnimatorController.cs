using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 角色动作控制器
/// </summary>
public class CharacterAnimatorController:MonoBehaviour
{
    private Animator animator;
    private string currentAnimationName;
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        currentAnimationName = AnimationType.Idle;
    }

    public void SetSpeed(float playSpeed)
    {

        if (animator != null)
            animator.speed = playSpeed;
    }
    
    public void ChangeAnimation(string animationName,float speed = 1f)
    {
        if (animator == null)
        {
            return;
        }
        if (currentAnimationName != animationName)
        {
            animator.speed = speed;
            animator.SetBool(currentAnimationName, false);
            animator.SetBool(animationName, true);
            currentAnimationName = animationName;
        }
    }

    public float GetCurrentActionLength()
    {
        if (animator != null)
        {
            AnimatorClipInfo[] animatorClipInfos = animator.GetCurrentAnimatorClipInfo(0);
            AnimationClip animationClip = animatorClipInfos[0].clip;
            Debug.Log("animationClip.name:" + animationClip.name);
            return animationClip.length;
        }
        return 0f;
    }
}