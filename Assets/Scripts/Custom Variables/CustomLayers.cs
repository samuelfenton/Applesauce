using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLayers : MonoBehaviour
{
    public static int m_enviromentMask = 0;
    public static int m_enviromentLayer = 0;
    public static int m_characterMask = 0;
    public static int m_characterLayer = 0;
    public static int m_portalMask = 0;
    public static int m_portalLayer = 0;

    //-------------------
    //Get masks for future use
    //-------------------
    static CustomLayers()
    {
        //Masks
        m_enviromentMask = LayerMask.GetMask("Enviroment");
        m_characterMask = LayerMask.GetMask("Character");
        m_portalMask = LayerMask.GetMask("Layer");
        //Layer
        m_enviromentLayer = (int)Mathf.Log(m_enviromentMask, 2);
        m_characterLayer = (int)Mathf.Log(m_characterMask, 2);
        m_portalLayer = (int)Mathf.Log(m_portalMask, 2);
    }
}
