﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public float m_mouseXSensitivity = 30.0f;
    public float m_mouseYSensitivity = 30.0f;

    public bool m_inverted = false;

    public enum INPUT_KEY {RELOAD, ATTACK, INTERACT, MENU, KEY_COUNT };
    
    public enum INPUT_STATE { UP, DOWN, DOWNED };

    public enum INPUT_AXIS { HORIZONTAL, VERTICAL, MOUSE_X, MOUSE_Y, SCROLL, AXIS_COUNT };

    //[HideInInspector]
    private INPUT_STATE[] m_keyVal = new INPUT_STATE[(int)INPUT_KEY.KEY_COUNT];
    //[HideInInspector]
    private float[] m_axisVal = new float[(int)INPUT_AXIS.AXIS_COUNT];

    private InputAction_Gameplay m_intput = null;

    private void Awake()
    {
        for (int i = 0; i < (int)INPUT_KEY.KEY_COUNT; i++)
        {
            m_keyVal[i] = INPUT_STATE.UP;
        }

        for (int i = 0; i < (int)INPUT_AXIS.AXIS_COUNT; i++)
        {
            m_axisVal[i] = 0.0f;
        }

        m_intput = new InputAction_Gameplay();
    }

    private void OnEnable()
    {
        m_intput.Player.Move.Enable();
        m_intput.Player.Mouse.Enable();

        m_intput.Player.Reload.Enable();
        m_intput.Player.Attack.Enable();
        m_intput.Player.Interact.Enable();
        m_intput.Player.Menu.Enable();
    }

    private void OnDisable()
    {
        m_intput.Player.Move.Disable();
        m_intput.Player.Mouse.Disable();

        m_intput.Player.Reload.Disable();
        m_intput.Player.Attack.Disable();
        m_intput.Player.Interact.Disable();
        m_intput.Player.Menu.Disable();
    }


    /// <summary>
    /// Update the inputs
    /// </summary>
    public void UpdateInput()
    {
        m_axisVal[(int)INPUT_AXIS.HORIZONTAL] = m_intput.Player.Move.ReadValue<Vector2>().x;
        m_axisVal[(int)INPUT_AXIS.VERTICAL] = m_intput.Player.Move.ReadValue<Vector2>().y;
        m_axisVal[(int)INPUT_AXIS.MOUSE_X] = m_intput.Player.Mouse.ReadValue<Vector2>().x;
        m_axisVal[(int)INPUT_AXIS.MOUSE_Y] = m_intput.Player.Mouse.ReadValue<Vector2>().y;

        
        m_keyVal[(int)INPUT_KEY.RELOAD] = DetermineInputState(m_intput.Player.Reload.triggered, m_intput.Player.Reload.ReadValue<float>());
        m_keyVal[(int)INPUT_KEY.ATTACK] = DetermineInputState(m_intput.Player.Attack.triggered, m_intput.Player.Attack.ReadValue<float>());
        m_keyVal[(int)INPUT_KEY.INTERACT] = DetermineInputState(m_intput.Player.Interact.triggered, m_intput.Player.Interact.ReadValue<float>());
        m_keyVal[(int)INPUT_KEY.MENU] = DetermineInputState(m_intput.Player.Menu.triggered, m_intput.Player.Menu.ReadValue<float>());
    }

    //Reset each input to Keystate up, and set axis values to 0.0f
    public void ResetInput()
    {
        for (int i = 0; i < (int)INPUT_KEY.KEY_COUNT; i++)
        {
            m_keyVal[i] = INPUT_STATE.UP;
        }

        for (int i = 0; i < (int)INPUT_AXIS.AXIS_COUNT; i++)
        {
            m_axisVal[i] = 0.0f;
        }
    }

    /// <summary>
    /// Determine the next state for a given key
    /// If trigger frame, always DOWNED
    /// If postive value and not trigger, then DOWN
    /// Default UP
    /// </summary>
    /// <param name="p_isTriggerFrame">All inputs will have trigger when input is in the same frame</param>
    /// <param name="p_currentVal">Current value for an input</param>
    /// <returns>Current input state based off above rules</returns>
    private INPUT_STATE DetermineInputState(bool p_isTriggerFrame, float p_currentVal)
    {
        if (p_isTriggerFrame)
            return INPUT_STATE.DOWNED;
        if (p_currentVal > 0.0f)
            return INPUT_STATE.DOWN;
        return INPUT_STATE.UP;
    }

    /// <summary>
    /// Simplify key state to a boolean
    /// </summary>
    /// <param name="p_input">Key to test against</param>
    /// <returns>true when down or downed</returns>
    public bool GetKeyBool(INPUT_KEY p_input)
    {
        if (p_input < INPUT_KEY.KEY_COUNT)
            return m_keyVal[(int)p_input] == INPUT_STATE.DOWN || m_keyVal[(int)p_input] == INPUT_STATE.DOWNED;
        return false;
    }

    /// <summary>
    /// Get Key state
    /// </summary>
    /// <param name="p_input">Axis to test against</param>
    /// <returns>Current state of key</returns>
    public INPUT_STATE GetKey(INPUT_KEY p_input)
    {
        if (p_input < INPUT_KEY.KEY_COUNT)
            return m_keyVal[(int)p_input];
        return INPUT_STATE.UP;
    }

    /// <summary>
    /// Simplyfy axis value to a boolean
    /// </summary>
    /// <param name="p_input">Axis to test against</param>
    /// <returns>true when axis value isnt 0.0f</returns>
    public bool GetAxisBool(INPUT_AXIS p_input)
    {
        if (p_input < INPUT_AXIS.AXIS_COUNT)
            return m_axisVal[(int)p_input] != 0.0f;
        return false;
    }

    /// <summary>
    /// Get Axis value
    /// </summary>
    /// <param name="p_input">Axis to test against</param>
    /// <returns>Value of Axis</returns>
    public float GetAxis(INPUT_AXIS p_input)
    {
        if(p_input == INPUT_AXIS.MOUSE_Y)
        {
            if (m_inverted)
                return m_axisVal[(int)p_input] * -1;
            return m_axisVal[(int)p_input];
        }
        if (p_input < INPUT_AXIS.AXIS_COUNT)
            return m_axisVal[(int)p_input];
        return 0.0f;
    }

    /// <summary>
    /// Checks for any input that might be used
    /// All excluding camera flip and menu
    /// </summary>
    /// <returns>If any value is positive</returns>
    public bool AnyInput()
    {
        return GetAxisBool(INPUT_AXIS.HORIZONTAL) || GetAxisBool(INPUT_AXIS.VERTICAL) || GetKeyBool(INPUT_KEY.RELOAD) || 
            GetKeyBool(INPUT_KEY.ATTACK) || GetKeyBool(INPUT_KEY.INTERACT);
    }
}
