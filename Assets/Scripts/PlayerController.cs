using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region 他クラス参照
    private PlayerInput _input = default;

    private Map _map = default;
    #endregion

    #region Vector2
    /// <summary>
    /// 移動するドロップの位置
    /// </summary>
    private Vector2 _mouse_Position = default;
    public Vector2 _Mouse_Position
    {
        get { return _mouse_Position; }
    }
    #endregion

    #region bool
    /// <summary>
    /// ドロップを動かせる時間
    /// </summary>
    private bool _canMove = false;
    #endregion

    #region float
    /// <summary>
    /// ドロップを動かせる残り秒数
    /// </summary>
    private float _move_Time = Variables._six;
    #endregion

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _map = GetComponent<Map>();
    }

    private void OnEnable()
    {
        _input.actions[Variables._position].performed += Move;
        _input.actions[Variables._select].performed += Select;
    }

    private void OnDisable()
    {
        _input.actions[Variables._position].performed -= Move;
        _input.actions[Variables._select].performed -= Select;
    }

    private void Move(InputAction.CallbackContext obj)
    {
        _mouse_Position = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
    }

    private void Select(InputAction.CallbackContext obj)
    {
        _canMove = obj.ReadValue<bool>();
    }
}