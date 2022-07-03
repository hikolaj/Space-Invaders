using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BulletBase
{
    public bool Active = false;

    private void Update()
    {
        Move();

        if (transform.position.y >= GameplayManager.Instance.MapManager.TopOfMap)
            DestroyMe();
    }

    public void DestroyMe()
    {
        Active = false;
        GameplayManager.Instance.PlayerShip.ReturnBullet(this);
    }

    void OnEnable()
    {
        Active = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active)
            return;
        
        if (col.tag == "Shield")
        {
            DestroyMe();
        }
        else if (col.tag == "Enemy")
        {
            col.GetComponent<Enemy>().Kill(true);
            DestroyMe();
        }
    }
}
