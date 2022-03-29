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
    private GameObject _generate_Drop = default;

    /// <summary>
    /// 現在動かしているDrop
    /// </summary>
    private GameObject _moving_Drop = default;

    /// <summary>
    /// 実際に消す配列
    /// </summary>
    [SerializeField, Header("削除する配列")] private GameObject[] _delete_Queue = new GameObject[Variables._thirty];

    /// <summary>
    /// 横を見ているときに仮に入れる配列
    /// </summary>
    [SerializeField, Header("x配列")] private GameObject[] _delete_Queue_x = new GameObject[Variables._six];

    /// <summary>
    /// 縦を見ているときに仮に入れる配列
    /// </summary>
    [SerializeField, Header("y配列")] private GameObject[] _delete_Queue_y = new GameObject[Variables._five];
    #endregion

    #region 盤面配列
    /// <summary>
    /// ステージ
    /// </summary>
    [SerializeField] private GameObject[,] _stage_Object = new GameObject[Variables._five, Variables._six];

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

    private int _drop_Count = default;
    #endregion

    #region bool
    private bool _cant_Delete = false;
    #endregion

    #region float
    /// <summary>
    /// ドロップの移動時間
    /// </summary>
    private float _drop_Time = Variables._drop_Move_Time;
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

    //---------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        _generate_Drop = (GameObject)Resources.Load(Variables._drop);
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
                    _stage_Object[y, x].name = Variables._drop_Name + _drop_Count;
                    _stage_Object[y, x].tag = Variables._drop_Tag[_drop_Type];
                    _drop_Count++;
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
        else if(_drop_Time != Variables._drop_Move_Time)
        {
            // 動かしているDropがあれば解除する
            if (_moving_Drop != null)
            {
                _moving_Drop.transform.position = new Vector2(Mathf.RoundToInt(_moving_Drop.transform.position.x), Mathf.RoundToInt(_moving_Drop.transform.position.y));
                _moving_Drop = null;
            }
            
            _drop_Time = Variables._drop_Move_Time;


            _playerController._now_state = PlayerController._player_State.MoveDelete;
        }
    }

    //---------------------------------------------------------------------------------------------------------

    public void ArrayCheck()
    {
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            for(int x = default; x < _stage_Object.GetLength(Variables._zero); x++)
            {
                if(_stage_Object[y, x].transform.position != new Vector3(x, y, Variables._zero))
                {

                }
            }
        }
        Delete();
    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Dropの削除処理
    /// </summary>
    public void Delete()
    {
        // 配列のy座標の最後まで
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            // 配列のx座標の最後まで
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                if (Stage_Object[y, x])
                {
                    Debug.Log(new Vector2( x, y));
                    SearchX(x, y);
                }
            }
        }
    }

    /// <summary>
    /// x軸上に3個以上つながっているか調べる
    /// </summary>
    private void SearchX(int x, int y)
    {
        // つながっている数
        int count = default;

        for (int right = x; right < _stage_Object.GetLength(Variables._one); right++)
        {
            // 右隣のtagが同じとき
            if (_stage_Object[y, x].tag == _stage_Object[y, right].tag)
            {
                // 削除する配列に入れる
                _delete_Queue_x[count] = _stage_Object[y, right];
                count++;
            }
            else
            {
                
                break;
            }
        }

        if (count > Variables._two)
        {
            for (int x_count = default; x_count != count; x_count++)
            {
                _delete_Queue[x_count] = _delete_Queue_x[x_count];
            }
        }
        else
        {
            count = Variables._zero;
            _delete_Queue_x = new GameObject[Variables._six];
        }
        SearchY(x, y, count);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">現在調べているx座標</param>
    /// <param name="y">現在調べているy座標</param>
    /// <param name="queue">削除する配列の現在の順番</param>
    private void SearchY(int x, int y, int queue)
    {
        // y座標でつながっている数
        int count = default;

        for (int up = y; up < _stage_Object.GetLength(Variables._zero); up++)
        {
            // 右隣のtagが同じとき
            if (_stage_Object[y, x].tag == _stage_Object[up, x].tag)
            {
                // 削除する配列に入れる
                _delete_Queue_y[count] = _stage_Object[up, x];
                count++;
            }
            // 違ったら抜ける
            else
            {
                break;
            }
        }

        // つながっている数が2より大きいとき
        if (count > Variables._two)
        {
            // つながっている数を超えるまで
            for (int y_count = default; y_count > count; y_count++)
            {
                // 削除する配列に入れる
                _delete_Queue[queue] = _delete_Queue_y[count];
                queue++;
            }
        }
        // 2以下の場合
        else
        {
            // 仮に入れた配列を初期化
            _delete_Queue_y = new GameObject[Variables._five];
        }

        StartCoroutine("Delete_Queue");
    }

    private IEnumerator Delete_Queue()
    {
        for (int count = default; count < _delete_Queue.GetLength(Variables._zero); count++)
        {
            if (!_delete_Queue[count])
            {
                yield break;
            }

            Destroy(_delete_Queue[count]);
        }
        _combo++;
        yield break;
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
                if (!_stage_Object[y, x])
                {
                    int fetch_y = y;

                    while (!_stage_Object[fetch_y, x])
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