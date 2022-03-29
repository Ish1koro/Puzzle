using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region ���N���X�Q��
    /// <summary>
    /// player�̓��͊Ǘ�
    /// </summary>
    private PlayerController _playerController = default;
    #endregion

    #region GameObject
    /// <summary>
    /// ��������Drop
    /// </summary>
    private GameObject _generate_Drop = default;

    /// <summary>
    /// ���ݓ������Ă���Drop
    /// </summary>
    private GameObject _moving_Drop = default;

    /// <summary>
    /// ���ۂɏ����z��
    /// </summary>
    [SerializeField, Header("�폜����z��")] private GameObject[] _delete_Queue = new GameObject[Variables._thirty];

    /// <summary>
    /// �������Ă���Ƃ��ɉ��ɓ����z��
    /// </summary>
    [SerializeField, Header("x�z��")] private GameObject[] _delete_Queue_x = new GameObject[Variables._six];

    /// <summary>
    /// �c�����Ă���Ƃ��ɉ��ɓ����z��
    /// </summary>
    [SerializeField, Header("y�z��")] private GameObject[] _delete_Queue_y = new GameObject[Variables._five];
    #endregion

    #region �Ֆʔz��
    /// <summary>
    /// �X�e�[�W
    /// </summary>
    [SerializeField] private GameObject[,] _stage_Object = new GameObject[Variables._five, Variables._six];

    /// <summary>
    /// �X�e�[�W�擾�p
    /// </summary>
    public GameObject[,] Stage_Object
    {
        get { return _stage_Object; }
    }
    #endregion

    #region int
    /// <summary>
    /// �h���b�v�̎��
    /// enum��int�ɂ��ē����
    /// </summary>
    private int _drop_Type = default;

    /// <summary>
    /// �O��̏�����x���W
    /// </summary>
    private int _old_Position_x = default;
    /// <summary>
    /// �O��̏�����y���W
    /// </summary>
    private int _old_Position_y = default;

    /// <summary>
    /// �R���{��
    /// </summary>
    private int _combo = default;

    private int _drop_Count = default;
    #endregion

    #region bool
    private bool _cant_Delete = false;
    #endregion

    #region float
    /// <summary>
    /// �h���b�v�̈ړ�����
    /// </summary>
    private float _drop_Time = Variables._drop_Move_Time;
    #endregion

    #region �z��
    /// <summary>
    /// �z��̐^��
    /// �Z�Z���Z�Z
    /// </summary>
    private const int _HALF = 3;
    /// <summary>
    /// �z���x���W��1��5
    /// </summary>
    private const int _STAGE_LENGTH_X =  1 | 5;
    /// <summary>
    /// �z���y���W��1��4
    /// </summary>
    private const int _STAGE_LENGTH_Y = 1 | 4;
    /// <summary>
    /// �z���x���W��2��5
    /// </summary>
    private const int _STAGE_MIDDLE = 2 | 5;
    #endregion

    //---------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        _generate_Drop = (GameObject)Resources.Load(Variables._drop);
        _playerController = GetComponent<PlayerController>();
    }

    #region ������
    /// <summary>
    /// �Q�[���J�n����Drop����
    /// Drop��2�ȏ�q���Ȃ�
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
    /// Drop�̈ړ�
    /// </summary>
    public void Move()
    {
        if (_playerController.IsCatch && _drop_Time > Variables._zero)
        {
            // ������mino���ݒ肳��Ă��Ȃ��ꍇ�ADrop��ݒ肷��
            if (_moving_Drop == null)
            {
                // �}�E�X�̍��W�ɂ���Drop���擾
                _moving_Drop = _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
                // �����Drop�̈ʒu���}�E�X�̈ʒu�Ŏ���Ă���
                _old_Position_x = Mathf.RoundToInt(_playerController._Mouse_Position.x);
                _old_Position_y = Mathf.RoundToInt(_playerController._Mouse_Position.y);
            }

            // Drop�̈ʒu���ړ�
            _moving_Drop.transform.position = _playerController._Mouse_Position;

            //�z��̈ʒu�ɂ���Drop�����ݓ������Ă���Drop����Ȃ���Ώꏊ�����ւ���
            if (_stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)] != _moving_Drop)
            {
                // �z���̃}�E�X�̍��W�ɂ���Drop�̈ʒu�����ݓ������Ă���Drop�̓������O�̈ʒu�Ɉړ�������
                _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)].transform.position = new Vector2(_old_Position_x, _old_Position_y);
                // ���ňړ�����Drop�̔z����X�V
                _stage_Object[_old_Position_y, _old_Position_x] = _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)];
                // �󂢂��Ƃ���Ɍ��ݓ������Ă���Drop���i�[
                _stage_Object[Mathf.RoundToInt(_playerController._Mouse_Position.y), Mathf.RoundToInt(_playerController._Mouse_Position.x)] = _moving_Drop;
                // �ʒu���X�V
                _old_Position_x = Mathf.RoundToInt(_playerController._Mouse_Position.x);
                _old_Position_y = Mathf.RoundToInt(_playerController._Mouse_Position.y);
            }

            // ���쎞��
            _drop_Time -= Time.deltaTime;
        }
        else if(_drop_Time != Variables._drop_Move_Time)
        {
            // �������Ă���Drop������Ή�������
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
    /// Drop�̍폜����
    /// </summary>
    public void Delete()
    {
        // �z���y���W�̍Ō�܂�
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            // �z���x���W�̍Ō�܂�
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
    /// x�����3�ȏ�Ȃ����Ă��邩���ׂ�
    /// </summary>
    private void SearchX(int x, int y)
    {
        // �Ȃ����Ă��鐔
        int count = default;

        for (int right = x; right < _stage_Object.GetLength(Variables._one); right++)
        {
            // �E�ׂ�tag�������Ƃ�
            if (_stage_Object[y, x].tag == _stage_Object[y, right].tag)
            {
                // �폜����z��ɓ����
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
    /// <param name="x">���ݒ��ׂĂ���x���W</param>
    /// <param name="y">���ݒ��ׂĂ���y���W</param>
    /// <param name="queue">�폜����z��̌��݂̏���</param>
    private void SearchY(int x, int y, int queue)
    {
        // y���W�łȂ����Ă��鐔
        int count = default;

        for (int up = y; up < _stage_Object.GetLength(Variables._zero); up++)
        {
            // �E�ׂ�tag�������Ƃ�
            if (_stage_Object[y, x].tag == _stage_Object[up, x].tag)
            {
                // �폜����z��ɓ����
                _delete_Queue_y[count] = _stage_Object[up, x];
                count++;
            }
            // ������甲����
            else
            {
                break;
            }
        }

        // �Ȃ����Ă��鐔��2���傫���Ƃ�
        if (count > Variables._two)
        {
            // �Ȃ����Ă��鐔�𒴂���܂�
            for (int y_count = default; y_count > count; y_count++)
            {
                // �폜����z��ɓ����
                _delete_Queue[queue] = _delete_Queue_y[count];
                queue++;
            }
        }
        // 2�ȉ��̏ꍇ
        else
        {
            // ���ɓ��ꂽ�z���������
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
    /// Drop����
    /// </summary>
    public void RandomGenerate()
    {
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                if (_stage_Object[y, x] == null)
                {
                    // Drop����
                    _stage_Object[y, x] = Instantiate(_generate_Drop, new Vector3(x, y), Quaternion.identity);
                    // Drop�̎�ނ�ݒ�
                    _drop_Type = Random.Range((int)Variables._drop_Type.Fire, (int)Variables._drop_Type.Null);
                    // �e�N�X�`���ύX
                    _stage_Object[y, x].GetComponent<Select_Sprite>().ChangeSprite(_drop_Type);
                    // �^�O��ݒ�
                    _stage_Object[y, x].tag = Variables._drop_Tag[_drop_Type];
                }
            }
        }

    }

    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Drop�̗���
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