using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Character_Enemy m_characterEnemy = null;
    private Room_EndRoom m_roomEnd = null;

    private int m_currentPortalTransportCount = 0;
    private int m_spawnEnemyAtCount = 5;
    private int m_spawnEndRoomAtCount = 15;

    private void Start()
    {
        FindObjectOfType<PortalController>().InitPortalController();

        m_characterEnemy = FindObjectOfType<Character_Enemy>();

        m_characterEnemy.gameObject.SetActive(false);

        m_roomEnd = FindObjectOfType<Room_EndRoom>();
    }

    public void PlayerMovedThroughPortal()
    {
        m_currentPortalTransportCount++;
        if(m_currentPortalTransportCount == m_spawnEnemyAtCount)
        {
            m_characterEnemy.gameObject.SetActive(true);
        }
        if (m_currentPortalTransportCount == m_spawnEndRoomAtCount)
        {
            m_roomEnd.EnableEndPortal();
        }
    }

    public void ShowPauseMenu()
    {

    }

    public void ClosePauseMenu()
    {

    }

    public void ResetGame()
    {

    }

    public void WinGame()
    {

    }
}
