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

    /// <summary>
    /// ��������Drop�̌�
    /// </summary>
    private int _drop_Count = default;

    /// <summary>
    /// �폜����z��̃L���[
    /// </summary>
    private int _queue = default;
    #endregion

    #region bool
    /// <summary>
    /// �z���ɑ��݂��邩
    /// </summary>
    private bool _isExist = default;
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
        Debug.Log(_stage_Object.Length);
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
            
            // ���쎞�Ԃ̌���
            _drop_Time = Variables._drop_Move_Time;

            _playerController._now_state = PlayerController._player_State.MoveDelete;
        }
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
                // �z���ɑ��݂���Α�����
                if (_stage_Object[y, x])
                {
                    SearchX(x, y);

                    Delete_Queue();
                }
            }
        }
    }

    /// <summary>
    /// x�����3�ȏ�Ȃ����Ă��邩���ׂ�
    /// </summary>
    /// <param name="x">���ݒ��ׂĂ���x���W</param>
    /// <param name="y">���ݒ��ׂĂ���y���W</param>
    private void SearchX(int x, int y)
    {
        // �Ȃ����Ă��鐔
        int count = default;

        #region �E�ɂ���Drop�̒T��
        for (int right = x; right < _stage_Object.GetLength(Variables._one); right++)
        {
            // �폜�z���Ɍ��ݒ��ׂĂ���Drop����������true�ɂ���
            for (int queue_count = default; queue_count < _delete_Queue_x.GetLength(Variables._zero); queue_count++)
            {
                if (_delete_Queue_x[queue_count] == _stage_Object[y, right])
                {
                    _isExist = true;
                }
                else
                {
                    _isExist = false;
                }
            }

            // �E�ׂ�tag�������Ŕz���ɑ��݂��Ȃ��Ƃ��Ƃ�
            if (_stage_Object[y, x].tag == _stage_Object[y, right].tag && !_isExist)
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
        #endregion

        #region ���ɂ���Drop�̒T��
        for (int left = x - Variables._one; left >= Variables._zero; left--)
        {
            // �폜�z���Ɍ��ݒ��ׂĂ���Drop����������true�ɂ���
            for (int queue_count = default; _delete_Queue_x[queue_count]; queue_count++)
            {
                if (_delete_Queue_x[queue_count] == _stage_Object[y, left])
                {
                    _isExist = true;
                }
                else
                {
                    _isExist = false;
                }
            }

            // ���ׂ�tag�������Ƃ�
            if (_stage_Object[y, x].tag == _stage_Object[y, left].tag)
            {
                // �폜����z��ɓ����
                _delete_Queue_x[count] = _stage_Object[y, left];
                count++;
            }
            else
            {
                break;
            }
        }
        #endregion

        #region �폜���邩�ǂ����̔��f
        if (count > Variables._two)
        {
            for (int x_count = default; x_count != count; x_count++)
            {
                _delete_Queue[_queue] = _delete_Queue_x[x_count];
                _queue++;
            }

            // ���ɓ��ꂽ�z���������
            _delete_Queue_x = new GameObject[Variables._six];
        }
        else
        {
            // ���ɓ��ꂽ�z���������
            _delete_Queue_x = new GameObject[Variables._six];
        }
        #endregion

        SearchY(x, y);
    }

    /// <summary>
    /// y�����3�ȏ�Ȃ����Ă��邩���ׂ�
    /// </summary>
    /// <param name="x">���ݒ��ׂĂ���x���W</param>
    /// <param name="y">���ݒ��ׂĂ���y���W</param>
    /// <param name="queue">�폜����z��̌��݂̏���</param>
    private void SearchY(int x, int y)
    {
        // y���W�łȂ����Ă��鐔
        int count = default;

        for (int up = y; up < _stage_Object.GetLength(Variables._zero); up++)
        {
            // �����폜�z���Ɍ��ݒ��ׂĂ���Drop����������true�ɂ���
            for (int queue_count = default; queue_count < _delete_Queue_x.GetLength(Variables._zero); queue_count++)
            {
                if (_delete_Queue_x[queue_count] == _stage_Object[up, x])
                {
                    _isExist = true;
                    break;
                }
            }

            // ���tag�������Ƃ�
            if (_stage_Object[y, x].tag == _stage_Object[up, x].tag && !_isExist)
            {
                // �폜����z��ɓ����
                _delete_Queue_y[count] = _stage_Object[up, x];
                count++;
            }
            // �z���ɑ��݂��������Ȃ�
            else if (_isExist)
            {
                return;
            }
            // �������for���𔲂���
            else
            {
                break;
            }
        }

        // �Ȃ����Ă��鐔��2���傫���Ƃ�
        if (count > Variables._two)
        {
            // �Ȃ����Ă��鐔�𒴂���܂�
            for (int y_count = default; y_count < count; y_count++)
            {
                // �폜����z��ɓ����
                _delete_Queue[_queue] = _delete_Queue_y[y_count];
                _queue++;
            }

            // ���ɓ��ꂽ�z���������
            _delete_Queue_y = new GameObject[Variables._five];
        }
        // 2�ȉ��̏ꍇ
        else
        {
            // ���ɓ��ꂽ�z���������
            _delete_Queue_y = new GameObject[Variables._five];
        }
    }

    private void Delete_Queue()
    {
        for (int count = default; count < _delete_Queue.GetLength(Variables._zero); count++)
        {
            if (!_delete_Queue[count])
            {
                break;
            }

            Destroy(_delete_Queue[count]);
            Debug.Log("A");

        }
         _playerController._now_state = PlayerController._player_State.Fall;

        if (_queue == Variables._zero)
        {
            _playerController._now_state = PlayerController._player_State.Move;
        }
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
                // Drop�����݂��Ȃ��Ƃ���ɐ�������
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
        for (int y = default; y < _stage_Object.GetLength(Variables._zero); y++)
        {
            for (int x = default; x < _stage_Object.GetLength(Variables._one); x++)
            {
                // �z���ɑ��݂��Ȃ��Ƃ�
                if (_stage_Object[y, x] == null)
                {
                    Debug.Log(_stage_Object[y, x]);

                    // Drop�����݂���Ƃ���܂�y�������Z
                    for (int fetch_y = y + Variables._one; fetch_y < _stage_Object.GetLength(Variables._zero); fetch_y++)
                    {
                        // ���݂����牺�ɂ��낷
                        if (_stage_Object[fetch_y, x])
                        {
                            _stage_Object[y, x] = _stage_Object[fetch_y, x];
                            _stage_Object[y, x].transform.position = new Vector2(x, y);
                            _stage_Object[fetch_y, x] = null;
                        }
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------
}