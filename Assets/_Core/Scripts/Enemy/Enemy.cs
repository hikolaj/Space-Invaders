using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public EnemyType Type;
    public string Name;
    public int Id;
    public int IdX;
    public int IdY;

    public void Shoot(EnemyBulletPool pool)
    {
        EnemyBullet bullet = pool.Get();
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
    }

    public void Kill(bool addPoints = false)
    {
        GameplayManager.Instance.EnemyManager.KillEnemy(this, addPoints);
    }
}
