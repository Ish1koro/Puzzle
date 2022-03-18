using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlayerController _playerController = default;

    [SerializeField] private GameObject _generate_Drop = default;

    private GameObject _moving_Drop = default;

    #region ”Õ–Ê”z—ñ
    private GameObject[,] Stage_Object = new GameObject[5, 6];

    private int[,] stage_drop_Type = new int[5, 6];

    public GameObject[,] _get_Stage
    {
        get { return Stage_Object; }
    }

    private GameObject[,] old_Stage = new GameObject[5, 6];
    #endregion

    #region int
    private int drop_type = default;
    #endregion

    #region bool
    private bool _isSet = false;
    #endregion

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    /*

    public void Generate()
    {

    }

    public void Move()
    {
        old_Stage = _get_Stage;
        if (!_isSet)
        {
            _moving_Drop = _get_Stage[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
        }
        _moving_Drop.transform.position = _playerController._Mouse_Position;
        if (_get_Stage[Mathf.RoundToInt(_moving_Drop.transform.position.y), Mathf.RoundToInt(_moving_Drop.transform.position.x)] != _moving_Drop)
        {
            Stage_Object[Mathf.RoundToInt(_moving_Drop.transform.position.y), Mathf.RoundToInt(_moving_Drop.transform.position.x)]
        }

    }

    public void Delete()
    {
        for (int y = default; y < Stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < Stage_Object.GetLength(Variables._one); x++)
            {
                if (Stage_Object[y, x].gameObject.tag == Stage_Object[y, x + Variables._one].gameObject.tag)
                {

                }
            }
        }
    }
    */
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
}
