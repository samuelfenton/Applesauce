using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_StartRoom : Room
{
    public Portal m_exitPortal = null;


    public override void PlayerEnteredRoom(Portal p_entertedFrom)
    {
        base.PlayerEnteredRoom(p_entertedFrom);
    }

    public override void PlayerLeftRoom()
    {
        base.PlayerLeftRoom();

        m_exitPortal.BreakPortal();
    }
}
