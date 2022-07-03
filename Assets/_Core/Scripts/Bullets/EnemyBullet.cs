using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletBase
{
    private void Update()
    {
        Move();
        if (transform.position.y <= GameplayManager.Instance.MapManager.BottomOfMap)
            Destroy();
    }

    public void Destroy()
    {
        GameplayManager.Instance.EnemyManager.ReturnBullet(this);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Shield")
        {
            col.GetComponent<Shield>().Hit();
            Destroy();
        }
        else if (col.tag == "Player")
        {
            col.GetComponent<PlayerShipController>().Hit();
            Destroy();
        }
    }
}
