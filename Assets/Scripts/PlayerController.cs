using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region ���N���X�Q��
    /// <summary>
    /// ���͂��ꂽ�l
    /// </summary>
    private PlayerIn _input = default;
    
    /// <summary>
    /// �z��N���X
    /// </summary>
    private Map _map = default;
    #endregion

    #region Vector2
    /// <summary>
    /// �ړ�����h���b�v�̈ʒu
    /// </summary>
    private Vector2 _mouse_Position = default;
    public Vector2 _Mouse_Position
    {
        get { return _mouse_Position; }
    }
    #endregion

    #region bool
    private bool _isCatch = default;
    public bool IsCatch
    {
        get { return _isCatch; }
    }
    #endregion

    #region enum
    /// <summary>
    /// ���݂̏����̏��
    /// </summary>
    public enum _player_State
    {
        Generate,
        Move,
        MoveDelete,
        RandomGenerate,
        Fall,
        FallDelete,
    }

    /// <summary>
    /// ���݂̏����̏��
    /// </summary>
    public _player_State _now_state = default;
    #endregion

    //---------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        _input = new PlayerIn();
        _map = GetComponent<Map>();
    }

    //---------------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (_input.InGame.Select.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            _isCatch = true;
        }
        else
        {
            _isCatch = false;
        }

        _mouse_Position = Camera.main.ScreenToWorldPoint(_input.InGame.Position.ReadValue<Vector2>());

        #region Drop�ʒu�̐���
        if (_mouse_Position.y >= Variables._four)
        {
            _mouse_Position.y = Variables._four;
        }
        else if (_mouse_Position.y < Variables._zero)
        {
            _mouse_Position.y = Variables._zero;
        }

        if (_mouse_Position.x >= Variables._five)
        {
            _mouse_Position.x = Variables._five;
        }
        else if (_mouse_Position.x < Variables._zero)
        {
            _mouse_Position.x = Variables._zero;
        }
        #endregion

        switch (_now_state)
        {
            case _player_State.Generate:
                Generate();
                break;
            case _player_State.Move:
                Move();
                break;
                /*
            case _player_State.MoveDelete:
                MoveDelete();
                break;
            case _player_State.Fall:
                Fall();
                break;
            case _player_State.FallDelete:
                FallDelete();
                break;
            case _player_State.RandomGenerate:
                RandomGenerate();
                break;*/
        }
    }

    //---------------------------------------------------------------------------------------------------------

    private void Generate()
    {
        _map.Generate();
        _now_state = _player_State.Move;
    }

    //---------------------------------------------------------------------------------------------------------

    private void Move()
    {
        _map.Move();
    }

    //---------------------------------------------------------------------------------------------------------

    private void MoveDelete()
    {
        _map.Delete();
        _now_state = _player_State.Fall;
    }

    //---------------------------------------------------------------------------------------------------------

    private void Fall()
    {
        _map.Fall();
        _now_state = _player_State.FallDelete;
    }

    //---------------------------------------------------------------------------------------------------------

    private void FallDelete()
    {
        _map.Delete();
        _now_state = _player_State.RandomGenerate;
    }

    //---------------------------------------------------------------------------------------------------------

    private void RandomGenerate()
    {
        _map.RandomGenerate();
        _now_state = _player_State.Move;
    }

    //---------------------------------------------------------------------------------------------------------

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}