using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Enemy : Character
{
    public const string SPIN_STRING = "Spin180";
    public const string RUN_STRING = "Run";
    public const string IDLE_STRING = "Idle";


    private enum ENEMY_STATE {IDLE, AWAKING, CHASING }
    private ENEMY_STATE m_currentState = ENEMY_STATE.IDLE;

    private Character_Player m_playerTarget = null;

    private Room m_pathfindingRoom = null;
    private Portal m_pathfindingPortal = null;

    public float m_forwardSpeed = 10.0f;
    public float m_rotSpeed = 80.0f;

    /// <summary>
    /// 
    /// </summary>
    public override void Init()
    {
        base.Init();

        SetCurrentRoom(null, m_currentRoom);

        m_playerTarget = FindObjectOfType<Character_Player>();

        m_currentState = ENEMY_STATE.IDLE;

        AkSoundEngine.PostEvent("Enemy_Breathing", gameObject);
        AkSoundEngine.PostEvent("Enemy_Growl", gameObject);

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();

        switch (m_currentState)
        {
            case ENEMY_STATE.IDLE:
                UpdateIdleState();
                break;
            case ENEMY_STATE.AWAKING:
                UpdateAwakingState();
                break;
            case ENEMY_STATE.CHASING:
                UpdateChasingState();
                break;
            default:
                    break;
        }

        if(Vector3.Distance(transform.position, m_playerTarget.transform.position) < 1.0f)
        {
            m_gameController.Death();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private bool SameRoomAsPlayer()
    {
        return m_currentRoom == m_playerTarget.m_currentRoom;
    }

    private Portal GetTraversalToPlayerPortal()
    {
        foreach (Portal currentRoomPortal in m_currentRoom.m_portals)
        {
            if(currentRoomPortal.m_connectedPortal.m_parentRoom == m_playerTarget.m_currentRoom)
            {
                return currentRoomPortal;
            }
        }

        return null;
    }

    private void RotateTowards(Vector3 p_point)
    {
        Vector3 toPortal = (p_point - transform.position).normalized;

        float dotRightToPortal = Vector3.Dot(transform.right, toPortal);

        if (dotRightToPortal >= 0.0f) //Rot left
        {
            transform.Rotate(transform.up, m_rotSpeed * Time.deltaTime);
        }
        else //Rot right
        {
            transform.Rotate(transform.up, -m_rotSpeed * Time.deltaTime);
        }
    }

    private void MoveTowards(Vector3 p_point)
    {
        Vector3 toTarget = (p_point - transform.position).normalized;

        float percentForwards = Vector3.Dot(toTarget, transform.forward);

        m_entityPhysics.SetVelocity(m_forwardSpeed * transform.forward * Mathf.Clamp(percentForwards, 0.2f, 1.0f));
    }

    private void UpdateIdleState()
    {
        if (SameRoomAsPlayer())
        {
            m_currentState = ENEMY_STATE.AWAKING;
            PlayAnimation(SPIN_STRING);

            Vector3 up = transform.up;
            transform.LookAt(m_playerTarget.transform.position, up);
        }
    }

    private void UpdateAwakingState()
    {
        if(IsAnimationDone())
        {
            m_currentState = ENEMY_STATE.CHASING;
            PlayAnimation(RUN_STRING);
        }
    }

    private void UpdateChasingState()
    {
        if (SameRoomAsPlayer()) //Same room, move towards
        {
            RotateTowards(m_playerTarget.transform.position);

            MoveTowards(m_playerTarget.transform.position);
        }
        else
        {
            if (m_pathfindingPortal == null)
            {
                m_pathfindingPortal = GetTraversalToPlayerPortal();
                m_pathfindingRoom = m_playerTarget.m_currentRoom;
            }
            else //Has target portal move towards
            {
                if (m_playerTarget.m_currentRoom != m_pathfindingRoom)//Player has moved to new room, find new portal or return to idle
                {
                    m_pathfindingPortal = GetTraversalToPlayerPortal();
                    m_pathfindingRoom = m_playerTarget.m_currentRoom;

                    if(m_pathfindingPortal == null) //Unable to find next room, return to idle
                    {
                        m_currentState = ENEMY_STATE.IDLE;
                        m_pathfindingRoom = null;
                        PlayAnimation(IDLE_STRING);
                    }    
                }
                else
                {
                    RotateTowards(m_pathfindingPortal.transform.position);
                    MoveTowards(m_pathfindingPortal.transform.position);
                }
            }
        }
    }
}
