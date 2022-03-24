using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region 他クラス参照
    /// <summary>
    /// playerの入力管理
    /// </summary>
    private PlayerController _playerController = default;
    #endregion

    #region GameObject
    /// <summary>
    /// 生成するDrop
    /// </summary>
    [SerializeField] private GameObject _generate_Drop = default;

    /// <summary>
    /// 現在動かしているDrop
    /// </summary>
    private GameObject _moving_Drop = default;

    private GameObject[,] delete_Queue = new GameObject[20,30];
    #endregion

    #region 盤面配列
    /// <summary>
    /// ステージ
    /// </summary>
    private GameObject[,] _stage_Object = new GameObject[Variables._five, Variables._six];

    /// <summary>
    /// ステージ取得用
    /// </summary>
    public GameObject[,] Stage_Object
    {
        get { return _stage_Object; }
    }
    #endregion

    #region int
    /// <summary>
    /// ドロップの種類
    /// enumをintにして入れる
    /// </summary>
    private int _drop_Type = default;

    /// <summary>
    /// 前回の処理のx座標
    /// </summary>
    private int _old_Position_x = default;
    /// <summary>
    /// 前回の処理のy座標
    /// </summary>
    private int _old_Position_y = default;

    /// <summary>
    /// コンボ数
    /// </summary>
    private int _combo = default;
    #endregion

    #region float
    /// <summary>
    /// ドロップの移動時間
    /// </summary>
    private float _drop_Time = Variables._drop_Move_Time;
    #endregion

    #region bool
    /// <summary>
    /// 動かすminoに設定されているか
    /// </summary>
    #endregion

    #region 配列
    /// <summary>
    /// 配列の真ん中
    /// 〇〇◎〇〇
    /// </summary>
    private const int _HALF = 3;
    /// <summary>
    /// 配列のx座標が1と5
    /// </summary>
    private const int _STAGE_LENGTH_X =  1 | 5;
    /// <summary>
    /// 配列のy座標が1と4
    /// </summary>
    private const int _STAGE_LENGTH_Y = 1 | 4;
    /// <summary>
    /// 配列のx座標が2と5
    /// </summary>
    private const int _STAGE_MIDDLE = 2 | 5;
    #endregion

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    #region 未実装
    /// <summary>
    /// ゲーム開始時のDrop生成
    /// Dropを2個以上繋げない
    /// </summary>
    public void Generate()
    {
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                if (_stage_Object[y, x] == null)
                {
                    _drop_Type = Random.Range((int)Variables._drop_Type.Fire, (int)Variables._drop_Type.Null);
                    _stage_Object[y, x] = Instantiate(_generate_Drop, new Vector3(x, y), Quaternion.identity);
                    _stage_Object[y, x].GetComponent<Select_Sprite>().ChangeSprite(_drop_Type);
                    _stage_Object[y, x].tag = Variables._drop_Tag[_drop_Type];
                }
            }
        }
    }
    #endregion

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Dropの移動
    /// </summary>
    public void Move()
    {
        if (_playerController.IsCatch && _drop_Time > Variables._zero)
        {
            // 動かすminoが設定されていない場合、Dropを設定する
            if (_moving_Drop == null)
            {
                // マウスの座標にあるDropを取得
                _moving_Drop = _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
                // 取ったDropの位置をマウスの位置で取っておく
                _old_Position_x = Mathf.RoundToInt(_playerController._Mouse_Position.x);
                _old_Position_y = Mathf.RoundToInt(_playerController._Mouse_Position.y);
            }

            // Dropの位置を移動
            _moving_Drop.transform.position = _playerController._Mouse_Position;

            //配列の位置にあるDropが現在動かしているDropじゃなければ場所を入れ替える
            if (_stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)] != _moving_Drop)
            {
                // 配列上のマウスの座標にあるDropの位置を現在動かしているDropの動かす前の位置に移動させる
                _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)].transform.position = new Vector2(_old_Position_x, _old_Position_y);
                // ↑で移動したDropの配列を更新
                _stage_Object[_old_Position_y, _old_Position_x] = _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
                // 空いたところに現在動かしているDropを格納
                _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)] = _moving_Drop;
                // 位置情報更新
                _old_Position_x = Mathf.RoundToInt(_playerController._Mouse_Position.x);
                _old_Position_y = Mathf.RoundToInt(_playerController._Mouse_Position.y);
            }

            // 操作時間
            _drop_Time -= Time.deltaTime;
        }
        else
        {
            // 動かしているDropがあれば解除する
            if (_moving_Drop != null)
            {
                _moving_Drop.transform.position = new Vector2(Mathf.RoundToInt(_moving_Drop.transform.position.x), Mathf.RoundToInt(_moving_Drop.transform.position.y));
                _moving_Drop = null;
            }
            _playerController._now_state = PlayerController._player_State.MoveDelete;
        }
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Dropの削除処理
    /// </summary>
    public void Delete()
    {
        int x = default;

        SearchX();
        SearchY();
        for (int y = default; y < delete_Queue.GetLength(Variables._zero); y++)
        {
            Destroy(delete_Queue[y, x]);
        }
    }

    /// <summary>
    /// x軸上に3個以上つながっているか調べる
    /// </summary>
    private void SearchX()
    {

    }

    private void SearchY()
    {

    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Drop生成
    /// </summary>
    public void RandomGenerate()
    {
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                if (_stage_Object[y, x] == null)
                {
                    // Drop生成
                    _stage_Object[y, x] = Instantiate(_generate_Drop, new Vector3(x, y), Quaternion.identity);
                    // Dropの種類を設定
                    _drop_Type = Random.Range((int)Variables._drop_Type.Fire, (int)Variables._drop_Type.Null);
                    // テクスチャ変更
                    _stage_Object[y, x].GetComponent<Select_Sprite>().ChangeSprite(_drop_Type);
                    // タグを設定
                    _stage_Object[y, x].tag = Variables._drop_Tag[_drop_Type];
                }
            }
        }

    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Dropの落下
    /// </summary>
    public void Fall()
    {
        for (int y = default; y < _stage_Object.GetLength(Variables._one); y++)
        {
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                if (_stage_Object[y, x] == null)
                {
                    int fetch_y = y;

                    while (_stage_Object[fetch_y, x] == null)
                    {
                        fetch_y++;
                    }
                    _stage_Object[y, x] = _stage_Object[fetch_y, x];
                    _stage_Object[fetch_y, x].transform.position = new Vector2(x, fetch_y);
                    _stage_Object[fetch_y, x] = null;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------
}