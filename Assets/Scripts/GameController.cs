using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController _playerController = default;
    private Map _map = default;

    #region float
    private float _drop_Time = Variables._drop_Move_Time;
    #endregion


    private enum _player_State
    {
        Generate,
        Move,
        MoveDelete,
        RandomGenerate,
        Fall,
        FallDelete,
    }

    private _player_State _now_state = default;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _map = GetComponent<Map>();
        _now_state = _player_State.Generate;
    }

    private void Update()
    {
        switch (_now_state)
        {
            case _player_State.Generate:
                _map.RandomGenerate();
                break;
            case _player_State.Move:
                Move();
                break;
            case _player_State.MoveDelete:
                //_map.Delete();
                break;
            case _player_State.RandomGenerate:
                _map.RandomGenerate();
                break;
            case _player_State.Fall:
                break;
            case _player_State.FallDelete:
                //_map.Delete();
                break;
        }
    }

    private void Move()
    {

        _map._get_Stage[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)].transform.position = _playerController._Mouse_Position;

        _drop_Time -= Time.deltaTime;
        if (_drop_Time < Variables._zero)
        {
            _now_state = _player_State.MoveDelete;
            _drop_Time = Variables._drop_Move_Time;
        }
    }
}
