using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Sprite : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprite = default;

    private SpriteRenderer _spriteRenderer = default;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(int drop_type)
    {
        _spriteRenderer.sprite = _sprite[drop_type];
    }
}
