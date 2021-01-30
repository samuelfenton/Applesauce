using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Player : Character
{
    public const string RELOAD_STRING = "Reload";
    public const string ATTACK_STRING = "Attack";

    private CustomInput m_input = null;

    [Header("Movement")]
    public float m_forwardVelocity = 10.0f;
    public float m_strafeVelocity = 10.0f;

    public float m_rotSpeed = 30.0f;

    //Weapon
    public int m_maxCurrentAmmo = 7;
    [HideInInspector]
    public int m_currentAmmo = 7;

    //Animator
    private Animator m_animator = null;
    private bool m_currentlyAnimating = false;

    public Room m_currentRoom = null;

    /// <summary>
    /// 
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_input = gameObject.AddComponent<CustomInput>();

        m_animator = GetComponentInChildren<Animator>();

        m_currentAmmo = m_maxCurrentAmmo;
        m_currentlyAnimating = false;

        if (m_currentRoom == null)
        {
            Debug.Log("Player requires the first room assigned");
            return;
        }

        m_currentRoom.EnteredRoom(null);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();

        m_input.UpdateInput();

        //States - Attack, reload
        if (m_currentlyAnimating)
        {
            if(IsAnimationDone())
            {
                m_currentlyAnimating = false;
            }
        }
        else
        {
            if (m_currentAmmo > 0 && m_input.GetKey(CustomInput.INPUT_KEY.ATTACK) == CustomInput.INPUT_STATE.DOWNED)
            {
                m_currentAmmo--;
                RaytraceBullet();

                PlayAnimation(ATTACK_STRING);

            }
            else if(m_input.GetKey(CustomInput.INPUT_KEY.RELOAD) == CustomInput.INPUT_STATE.DOWNED)
            {
                m_currentAmmo = m_maxCurrentAmmo;
                PlayAnimation(RELOAD_STRING);
            }
        }

        //Movement

        m_entityPhysics.Translate(transform.forward * m_input.GetAxis(CustomInput.INPUT_AXIS.VERTICAL) * m_forwardVelocity + transform.right * m_input.GetAxis(CustomInput.INPUT_AXIS.HORIZONTAL) * m_strafeVelocity);
        transform.Rotate(transform.up, m_input.GetAxis(CustomInput.INPUT_AXIS.MOUSE_X) * m_rotSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void RaytraceBullet()
    {
        Debug.Log("Fired");
    }

    private bool IsAnimationDone()
    { 
        if (m_animator == null )
            return false;

        return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.80f;
    }

    private void PlayAnimation(string p_animationString)
    {
        m_animator.Play(p_animationString);

        m_currentlyAnimating = true;
    }

    public void SetCurrentRoom(Portal p_entertedIntoPortal)
    {
        m_currentRoom.LeftRoom();
        m_currentRoom = p_entertedIntoPortal.m_parentRoom;
        m_currentRoom.EnteredRoom(p_entertedIntoPortal);
    }
}
