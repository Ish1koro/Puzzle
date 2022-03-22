using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlayerController _playerController = default;

    /// <summary>
    /// 生成するDrop
    /// </summary>
    [SerializeField] private GameObject _generate_Drop = default;

    /// <summary>
    /// 現在動かしているDrop
    /// </summary>
    private GameObject _moving_Drop = default;

    #region 盤面配列
    private GameObject[,] Stage_Object = new GameObject[Variables._five, Variables._six];

    public GameObject[,] _get_Stage
    {
        get { return Stage_Object; }
    }

    private GameObject[,] old_Stage = new GameObject[Variables._five, Variables._six];

    #endregion

    #region int
    /// <summary>
    /// ドロップの種類
    /// enumをintにして入れる
    /// </summary>
    private int drop_type = default;

    #endregion

    #region float
    private float _drop_Time = Variables._drop_Move_Time;
    #endregion

    #region bool
    /// <summary>
    /// 
    /// </summary>
    private bool _isSet = false;
    #endregion

    #region const
    private const int _HALF = 3;
    private const int _STAGE_LENGTH_X = 1 << 1 | 1 << 4;
    private const int _STAGE_LENGTH_Y = 1 << 1 | 1 << 3;
    private const int _STAGE_MIDDLE = 1 << 2 | 1 << 3;

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
        for (int y = default; y < Stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < Stage_Object.GetLength(Variables._one); x++)
            {
                if (Stage_Object[y, x] == null)
                {
                    drop_type = Random.Range((int)Variables._drop_Type.Fire, (int)Variables._drop_Type.Null);
                    Stage_Object[y, x] = Instantiate(_generate_Drop, new Vector3(x, y), Quaternion.identity);
                    Stage_Object[y, x].GetComponent<Select_Sprite>().ChangeSprite(drop_type);
                    Stage_Object[y, x].gameObject.tag = Variables._drop_Tag[drop_type];
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Dropの移動
    /// </summary>
    public void Move()
    {
        old_Stage = _get_Stage;

        _moving_Drop.transform.position = _playerController._Mouse_Position;
        
        if (!_isSet)
        {
            _moving_Drop = _get_Stage[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
        }
        
        if (_get_Stage[Mathf.RoundToInt(_moving_Drop.transform.position.y), Mathf.RoundToInt(_moving_Drop.transform.position.x)] != _moving_Drop)
        {
            // Stage_Object[Mathf.RoundToInt(_moving_Drop.transform.position.y), Mathf.RoundToInt(_moving_Drop.transform.position.x)];
        }

    }

    public void Delete()
    {
        GameObject[,] delete_GameObjects = default;

        int combo = Variables._one;
        int count = Variables._one;

        for (int y = default; y < Stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < Stage_Object.GetLength(Variables._one); x++)
            {
                switch (x)
                {
                    case _HALF:
                        if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._one].gameObject.tag && (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._two].gameObject.tag || Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag))
                        {
                            delete_GameObjects[combo, count] = Stage_Object[y, x];
                            Stage_Object[y, x] = null;
                        }
                        else if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag && Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._two].gameObject.tag)
                        {
                            delete_GameObjects[combo, count] = Stage_Object[y, x];
                            Stage_Object[y, x] = null;
                        }
                        break;

                    case _STAGE_MIDDLE:
                        if (x == Variables._two)
                        {
                            if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._one].gameObject.tag && (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._two].gameObject.tag || Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag))
                            {
                                delete_GameObjects[combo, count] = Stage_Object[y, x];
                                Stage_Object[y, x] = null;
                            }
                        }
                        else
                        {
                            if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag && Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._two].gameObject.tag)
                            {
                                delete_GameObjects[combo, count] = Stage_Object[y, x];
                                Stage_Object[y, x] = null;
                            }
                        }
                        break;

                    case _STAGE_LENGTH_X:
                        if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._one].gameObject.tag && Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag)
                        {
                            delete_GameObjects[y, x] = Stage_Object[y, x];
                        }
                        break;
                }

                switch (y)
                {
                    case _STAGE_LENGTH_Y:
                        if (Stage_Object[y, x].gameObject.tag == Stage_Object[y + Variables._one, x].gameObject.tag && Stage_Object[y, x].gameObject.tag == Stage_Object[y - Variables._one, x].gameObject.tag)
                        {
                            delete_GameObjects[y, x] = Stage_Object[y, x];
                        }
                        break;


                    default:
                        if (y == Variables._two)
                        {
                            if (Stage_Object[y, x].gameObject.tag == Stage_Object[y + Variables._one, x].gameObject.tag && (Stage_Object[y, x].gameObject.tag == Stage_Object[y + Variables._two, x].gameObject.tag || Stage_Object[y, x].gameObject.tag == Stage_Object[y - Variables._one, x].gameObject.tag))
                            {
                                delete_GameObjects[combo, count] = Stage_Object[y, x];
                                Stage_Object[y, x] = null;
                            }
                        }
                        else
                        {
                            if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._one].gameObject.tag && Stage_Object[y, x].gameObject.tag == Stage_Object[y, x - Variables._two].gameObject.tag)
                            {
                                delete_GameObjects[combo, count] = Stage_Object[y, x];
                                Stage_Object[y, x] = null;
                            }
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Drop生成
    /// </summary>
    public void RandomGenerate()
    {
        for (int y = default; y < Stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < Stage_Object.GetLength(Variables._one); x++)
            {
                if (Stage_Object[y, x] == null)
                {
                    Stage_Object[y, x] = Instantiate(_generate_Drop, new Vector3(x, y), Quaternion.identity);
                    drop_type = Random.Range((int)Variables._drop_Type.Fire, (int)Variables._drop_Type.Null);
                    Stage_Object[y, x].GetComponent<Select_Sprite>().ChangeSprite(drop_type);
                    Stage_Object[y, x].gameObject.tag = Variables._drop_Tag[drop_type];
                }
            }
        }
    }

    /// <summary>
    /// Dropの落下
    /// </summary>
    public void Fall()
    {
        for (int y = default; y < Stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < Stage_Object.GetLength(Variables._zero); x++)
            {
                if (Stage_Object[y, x] = null)
                {

                }
            }
        }
    }
}
