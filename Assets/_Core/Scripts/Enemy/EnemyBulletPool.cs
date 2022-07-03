using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    private ObjectPool<EnemyBullet> _bulletPool;
    private EnemyBullet _enemyBulletPrefab;
    private Transform _parent;

    public void Initialize(EnemyBullet bulletPrefab, Transform parent)
    {
        _parent = parent;
        _enemyBulletPrefab = bulletPrefab;
        _bulletPool = new ObjectPool<EnemyBullet>(Create);
    }

    public void Return(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Release(bullet);
    }

    public EnemyBullet Get()
    {
        EnemyBullet bullet = _bulletPool.Get();
        return bullet;
    }

    private EnemyBullet Create()
    {
        EnemyBullet bullet = Instantiate(_enemyBulletPrefab);
        bullet.transform.parent = _parent;
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    public void Dispose()
    {
        _bulletPool.Dispose();
    }
}
