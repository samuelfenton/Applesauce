using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Player : MonoBehaviour
{
    private CustomInput m_input = null;

    [Header("Movement")]
    public float m_forwardVelocity = 10.0f;
    public float m_strafeVelocity = 10.0f;

    public float m_rotSpeed = 30.0f;


    private void Start()
    {
        m_input = gameObject.AddComponent<CustomInput>();
    }

    private void FixedUpdate()
    {
        m_input.UpdateInput();

        transform.Translate((transform.forward * m_input.GetAxis(CustomInput.INPUT_AXIS.VERTICAL) * m_forwardVelocity + transform.right * m_input.GetAxis(CustomInput.INPUT_AXIS.HORIZONTAL) * m_strafeVelocity) * Time.deltaTime, Space.World);
        transform.Rotate(transform.up, m_input.GetAxis(CustomInput.INPUT_AXIS.MOUSE_X) * m_rotSpeed * Time.deltaTime);
    }
}
