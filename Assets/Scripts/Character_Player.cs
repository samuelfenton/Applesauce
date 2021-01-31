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

    /// <summary>
    /// 
    /// </summary>
    public override void Init()
    {
        base.Init();

        m_input = gameObject.AddComponent<CustomInput>();

        m_currentAmmo = m_maxCurrentAmmo;

        if (m_currentRoom == null)
        {
            Debug.Log("Player requires the first room assigned");
            return;
        }

        SetCurrentRoom(null, m_currentRoom);
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

        m_entityPhysics.SetVelocity(transform.forward * m_input.GetAxis(CustomInput.INPUT_AXIS.VERTICAL) * m_forwardVelocity + transform.right * m_input.GetAxis(CustomInput.INPUT_AXIS.HORIZONTAL) * m_strafeVelocity);
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
        if (Physics.Raycast(transform.position + transform.up * 1.5f + transform.forward * 0.5f, transform.forward, out RaycastHit hit, 100.0f, ~CustomLayers.m_enviromentMask))//Raycast ignore the enviroment layer
        {
            Portal portalHit = hit.collider.GetComponent<Portal>();

            if(portalHit != null)
            {
                portalHit.BreakPortal();

                if(portalHit.m_connectedPortal != null)
                {
                    portalHit.m_connectedPortal.BreakPortal();
                }
            }
        }
    }

    public override void SetCurrentRoom(Portal p_entertedIntoPortal, Room p_currentRoom)
    {
        m_currentRoom.PlayerLeftRoom();
        m_currentRoom = p_currentRoom;
        m_currentRoom.PlayerEnteredRoom(p_entertedIntoPortal);
    }
}
