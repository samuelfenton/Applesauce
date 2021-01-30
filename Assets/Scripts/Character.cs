using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    //Animator
    protected Animator m_animator = null;
    protected bool m_currentlyAnimating = false;

    /// <summary>
    /// 
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_animator = GetComponentInChildren<Animator>();

        m_currentlyAnimating = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    protected bool IsAnimationDone()
    {
        if (m_animator == null)
            return false;

        return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.80f;
    }

    protected void PlayAnimation(string p_animationString)
    {
        m_animator.Play(p_animationString);

        m_currentlyAnimating = true;
    }
}
