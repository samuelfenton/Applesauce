using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Character_Enemy m_characterEnemy = null;
    private Room_EndRoom m_roomEnd = null;

    private int m_currentPortalTransportCount = 0;
    private int m_spawnEnemyAtCount = 5;
    private int m_spawnEndRoomAtCount = 15;

    public GameObject m_UI = null;
    public GameObject m_WIN = null;

    private void Start()
    {
        FindObjectOfType<PortalController>().InitPortalController();

        m_characterEnemy = FindObjectOfType<Character_Enemy>();

        m_characterEnemy.gameObject.SetActive(false);

        m_roomEnd = FindObjectOfType<Room_EndRoom>();

        m_UI.SetActive(false);
        m_WIN.SetActive(false);
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

    public void TogglePauseMenu()
    {
        m_UI.SetActive(!m_UI.activeSelf);
    }

    public void Win()
    {
        m_WIN.SetActive(true);
    }

    public void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
