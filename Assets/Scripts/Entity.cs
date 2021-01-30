using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [HideInInspector]
    public EntityPhysics m_entityPhysics = null;

    /// <summary>
    /// 
    /// </summary>
    public virtual void Init()
    {
        m_entityPhysics = gameObject.AddComponent<EntityPhysics>();
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        m_entityPhysics.UpdatePhysics();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p_connectingPortal"></param>
    public void MovedThroughPortal(Portal p_connectingPortal)
    {

    }
}
