using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector]
    public List<Portal> m_portals = new List<Portal>();
    [HideInInspector]
    public bool IsActiveRoom = false;

    private PortalController m_portalController = null;

    public void Init(PortalController p_portalController)
    {
        m_portalController = p_portalController;

        m_portals.AddRange(GetComponentsInChildren<Portal>());

        foreach (Portal portal in m_portals)
        {
            portal.Init(this);
        }
    }

    public virtual void PlayerEnteredRoom(Portal p_entertedFrom)
    {
        IsActiveRoom = true;

        m_portalController.BuildConnections(m_portals, p_entertedFrom);
    }

    public virtual void PlayerLeftRoom()
    {
        IsActiveRoom = false;
    }
}
