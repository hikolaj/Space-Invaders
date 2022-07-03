using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Shield : MonoBehaviour
{
    public int HP = 3;

    private int _hp;
    
    private Material _material;
    private BoxCollider2D _collider;


    void Start()
    {
        _hp = HP;

        
        _material = GetComponent<SpriteRenderer>().material;
        _material.SetFloat("_MaxHP", HP);
        UpdateMaterial();
        
        _collider = GetComponent<BoxCollider2D>();
        UpdateCollider();

    }

    void Update()
    {
        ///////////////
    }

    public void Hit()
    {
        if(_hp > 0)
        {
            _hp -= 1;
            UpdateMaterial();
            UpdateCollider();

            if (_hp == 0)
                gameObject.SetActive(false);
        }
    }

    public void Heal()
    {
        _hp = HP;
        UpdateMaterial();
        UpdateCollider();
    }

    private void UpdateMaterial()
    {
        _material.SetFloat("_HP", _hp);
    }

    private void UpdateCollider()
    {
        if(HP <= 0)
        {
            _collider.enabled = false;
            return;
        }

        float healt = (float)_hp / (float)HP;

        _collider.size = new Vector2(1, healt);
        _collider.offset = new Vector2(0, (1 - healt)/-2);
    }
}
