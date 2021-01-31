using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Player : Character
{
    public const string RELOAD_STRING = "Reload";
    public const string ATTACK_STRING = "Attack";

    public const string VELOCITY_VARIABLE = "Velocity";

    private CustomInput m_input = null;
    private Player_Sound m_playerSound = null;

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
        m_playerSound = GetComponentInChildren<Player_Sound>();

        m_currentAmmo = m_maxCurrentAmmo;

        if (m_currentRoom == null)
        {
            Debug.Log("Player requires the first room assigned");
            return;
        }

        m_currentRoom.PlayerEnteredRoom(null);

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
            if (m_input.GetKey(CustomInput.INPUT_KEY.ATTACK) == CustomInput.INPUT_STATE.DOWNED)
            {
                if(m_currentAmmo > 0)
                {
                    m_currentAmmo--;
                    RaytraceBullet();

                    PlayAnimation(ATTACK_STRING);

                    m_playerSound.PlayGunshot();
                }
                else //Dry Fire
                {
                    m_playerSound.PlayGunDryFire();
                }

            }
            else if(m_input.GetKey(CustomInput.INPUT_KEY.RELOAD) == CustomInput.INPUT_STATE.DOWNED)
            {
                m_currentAmmo = m_maxCurrentAmmo;
                PlayAnimation(RELOAD_STRING);

                m_playerSound.PlayGunReload();
            }
        }

        //Movement

        float vertInput = m_input.GetAxis(CustomInput.INPUT_AXIS.VERTICAL);
        float horiInput = m_input.GetAxis(CustomInput.INPUT_AXIS.HORIZONTAL);
        if (Mathf.Abs(vertInput) < 0.5f && Mathf.Abs(horiInput) < 0.5f)
        {
            m_animator.SetFloat(VELOCITY_VARIABLE, 0.0f);
        }
        else
        {
            m_animator.SetFloat(VELOCITY_VARIABLE, 1.0f);
        }

        m_entityPhysics.SetVelocity(transform.forward * vertInput * m_forwardVelocity + transform.right * horiInput * m_strafeVelocity);
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

        m_gameController.PlayerMovedThroughPortal();
    }
}
