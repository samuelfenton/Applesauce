using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysics : MonoBehaviour
{
    private const float NORMAL_CALC_HEIGHT = 1.5f;

    private const float NORMAL_CALC_DISTANCE = 3.0f;
    private const float NORMAL_CALC_FORWARD_DISTANCE = 0.5f;
    private const float NORMAL_CALC_RIGHT_DISTANCE = 0.5f;

    private Rigidbody m_rb = null;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void UpdatePhysics()
    {
        //SetUpDirection();
        SnapToGround();
    }

    private void SetUpDirection()
    {
        RaycastHit hitInfo;
        Vector3 rightDir = transform.right;

        //Get up dir
        //Raycast downwards 3 times.
        Vector3 forwardCollisionPoint = Vector3.negativeInfinity;
        Vector3 backRightCollisionPoint = Vector3.negativeInfinity;
        Vector3 backLeftCollisionPoint = Vector3.negativeInfinity;

        Vector3 castFromPoint = transform.position + transform.up * NORMAL_CALC_HEIGHT;

        Vector3 forwardDir = castFromPoint - (transform.position + transform.forward * NORMAL_CALC_FORWARD_DISTANCE);
        Vector3 backRightDir = castFromPoint - (transform.position -  transform.forward * NORMAL_CALC_FORWARD_DISTANCE + transform.right * NORMAL_CALC_RIGHT_DISTANCE);
        Vector3 backLeftDir = castFromPoint - (transform.position - transform.forward * NORMAL_CALC_FORWARD_DISTANCE - transform.right * NORMAL_CALC_RIGHT_DISTANCE);

        if (Physics.Raycast(transform.position + transform.up * NORMAL_CALC_HEIGHT, forwardDir, out hitInfo, NORMAL_CALC_DISTANCE, CustomLayers.m_enviromentMask))
        {
            forwardCollisionPoint = hitInfo.point;
        }
        if (Physics.Raycast(transform.position + transform.up * NORMAL_CALC_HEIGHT, backRightDir, out hitInfo, NORMAL_CALC_DISTANCE, CustomLayers.m_enviromentMask))
        {
            backRightCollisionPoint = hitInfo.point;
        }
        if (Physics.Raycast(transform.position + transform.up * NORMAL_CALC_HEIGHT, backLeftDir, out hitInfo, NORMAL_CALC_DISTANCE, CustomLayers.m_enviromentMask))
        {
            backLeftCollisionPoint = hitInfo.point;
        }

        Vector3 dir = Vector3.Cross(backRightCollisionPoint - forwardCollisionPoint, backLeftCollisionPoint - forwardCollisionPoint);
        var upNorm = Vector3.Normalize(dir);

        //transform.up = norm;

        //Get forward dir
        Vector3 newForward = Quaternion.AngleAxis(90.0f, rightDir) * upNorm;

        transform.LookAt(transform.position + newForward, upNorm);
    }

    private void SnapToGround()
    {
        if (Physics.Raycast(transform.position + transform.up * NORMAL_CALC_HEIGHT, -transform.up, out RaycastHit hitInfo, NORMAL_CALC_DISTANCE, CustomLayers.m_enviromentMask))
        {
            transform.position = hitInfo.point;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p_velocity"></param>
    public void Translate(Vector3 p_velocity)
    {
        m_rb.velocity = p_velocity;
    }
}
