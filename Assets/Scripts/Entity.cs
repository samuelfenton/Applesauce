using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityPhysics m_entityPhysics = null;

    /// <summary>
    /// 
    /// </summary>
    public virtual void Start()
    {
        m_entityPhysics = gameObject.AddComponent<EntityPhysics>();
    }

    public virtual void Update()
    {
        m_entityPhysics.UpdatePhysics();
    }

    public virtual void FixedUpdate()
    {

    }
}
