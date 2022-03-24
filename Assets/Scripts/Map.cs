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
    [SerializeField] private GameObject _generate_Drop = default;

    /// <summary>
    /// ���ݓ������Ă���Drop
    /// </summary>
    private GameObject _moving_Drop = default;

    private GameObject[,] delete_Queue = new GameObject[20,30];
    #endregion

    #region �Ֆʔz��
    /// <summary>
    /// �X�e�[�W
    /// </summary>
    private GameObject[,] _stage_Object = new GameObject[Variables._five, Variables._six];

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
    #endregion

    #region float
    /// <summary>
    /// �h���b�v�̈ړ�����
    /// </summary>
    private float _drop_Time = Variables._drop_Move_Time;
    #endregion

    #region bool
    /// <summary>
    /// ������mino�ɐݒ肳��Ă��邩
    /// </summary>
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

    private void Awake()
    {
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
                    _stage_Object[y, x].tag = Variables._drop_Tag[_drop_Type];
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
        else
        {
            // �������Ă���Drop������Ή�������
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
    /// Drop�̍폜����
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
    /// x�����3�ȏ�Ȃ����Ă��邩���ׂ�
    /// </summary>
    private void SearchX()
    {

    }

    private void SearchY()
    {

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