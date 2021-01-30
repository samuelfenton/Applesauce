using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private List<Room> m_rooms = new List<Room>();

    private List<Portal> m_portals = new List<Portal>();

    private void Start()
    {
        m_rooms.AddRange(FindObjectsOfType<Room>());
        m_portals.AddRange(FindObjectsOfType<Portal>());

        foreach (Room room in m_rooms)
        {
            room.Init(this);
        }

        List<Entity> entities = new List<Entity>();
        entities.AddRange(FindObjectsOfType<Entity>());

        foreach (Entity entity in entities)
        {
            entity.Init();
        }
    }

    public void BuildConnections(List<Portal> p_toBeConnectedPortals, Portal p_currentPortal)
    {
        List<Portal> allPortals = m_portals;

        for (int portalIndex = 0; portalIndex < p_toBeConnectedPortals.Count; portalIndex++)
        {
            if(!p_toBeConnectedPortals[portalIndex].m_portalBrokenFlag || p_toBeConnectedPortals[portalIndex] == p_currentPortal)//Dont change if broken or one just moved through
            {
                p_toBeConnectedPortals[portalIndex].m_connectedPortal = null;

                List<Portal> possiblePortals = new List<Portal>();

                for (int allPortalIndex = 0; allPortalIndex < allPortals.Count; allPortalIndex++)
                {
                    if (allPortals[allPortalIndex] != p_toBeConnectedPortals[portalIndex] && !allPortals[allPortalIndex].m_portalBrokenFlag)
                    {
                        possiblePortals.Add(allPortals[allPortalIndex]);
                    }
                }

                p_toBeConnectedPortals[portalIndex].m_connectedPortal = possiblePortals[Random.Range(0, possiblePortals.Count)];
            }
        }
    }
}
