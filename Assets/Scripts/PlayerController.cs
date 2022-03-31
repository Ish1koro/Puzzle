using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 他クラス参照
    /// <summary>
    /// 入力された値
    /// </summary>
    private PlayerIn _input = default;
    
    /// <summary>
    /// 配列クラス
    /// </summary>
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
    private bool _isCatch = default;
    public bool IsCatch
    {
        get { return _isCatch; }
    }
    #endregion

    #region enum
    /// <summary>
    /// 現在の処理の状態
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
    /// 現在の処理の状態
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
        Input();

        switch (_now_state)
        {
            case _player_State.Generate:
                Generate();
                break;
            case _player_State.Move:
                Move();
                break;
            case _player_State.MoveDelete:
                MoveDelete();
                break;
            case _player_State.Fall:
                Fall();
                break;
            case _player_State.RandomGenerate:
                RandomGenerate();
                break;
            case _player_State.FallDelete:
                FallDelete();
                break;
                
        }
    }

    //---------------------------------------------------------------------------------------------------------

    private void Input()
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

        #region Drop位置の制限
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
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 生成
    /// </summary>
    private void Generate()
    {
        _map.Generate();
        _now_state = _player_State.Move;
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        _map.Move();
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 削除
    /// </summary>
    private void MoveDelete()
    {
        _map.Delete();
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 落下
    /// </summary>
    private void Fall()
    {
        _map.Fall();
        _now_state = _player_State.FallDelete;
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 落下後の削除
    /// </summary>
    private void FallDelete()
    {
        _map.Delete();
        _now_state = _player_State.RandomGenerate;
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 落下後のランダムな生成
    /// </summary>
    private void RandomGenerate()
    {
        _map.RandomGenerate();
        _now_state = _player_State.Move;
    }

    //---------------------------------------------------------------------------------------------------------
    ///InputSystem
    
    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}