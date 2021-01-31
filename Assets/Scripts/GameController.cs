using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Character_Enemy m_characterEnemy = null;

    private int m_currentPortalTransportCount = 0;
    private int m_spawnEnemyCount = 5;   

    private void Start()
    {
        m_characterEnemy = FindObjectOfType<Character_Enemy>();

        m_characterEnemy.gameObject.SetActive(false);
    }

    public void PlayerMovedThroughPortal()
    {
        Debug.Log("transfererd through portal");
        m_currentPortalTransportCount++;
        if(m_currentPortalTransportCount == m_spawnEnemyCount)
        {
            m_characterEnemy.gameObject.SetActive(true);
        }
    }
}
