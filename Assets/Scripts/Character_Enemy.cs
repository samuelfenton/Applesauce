using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Enemy : Character
{
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
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();

        if(SameRoomAsPlayer()) //Same room, move towards
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
                if(m_playerTarget.m_currentRoom != m_pathfindingRoom)//Player has moved to new room, find new portal when possible
                {
                    m_pathfindingPortal = GetTraversalToPlayerPortal();
                    m_pathfindingRoom = m_playerTarget.m_currentRoom;
                }
                else
                {
                    RotateTowards(m_pathfindingPortal.transform.position);
                    MoveTowards(m_pathfindingPortal.transform.position);
                }
            }
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
}
