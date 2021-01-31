using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_EndRoom : Room
{
    public Portal m_entrancePortal = null;

    private void Start()
    {
        m_entrancePortal.BreakPortal();

    }

    public override void PlayerEnteredRoom(Portal p_entertedFrom)
    {
        base.PlayerEnteredRoom(p_entertedFrom);

        FindObjectOfType<GameController>().Win();
    }

    public override void PlayerLeftRoom()
    {
        base.PlayerLeftRoom();
    }

    public void EnableEndPortal()
    {
        m_entrancePortal.UnBreakPortal();
    }
}
