using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(EnemyWave))]
[RequireComponent(typeof(EnemyPool))]
[RequireComponent(typeof(EnemyBulletPool))]
public class EnemyManager : MonoBehaviour
{
    public EnemyBullet EnemyBulletPrefab;
    public EnemyBlueprints EnemyBlueprints;
    public EnemyPool EnemyPool;
    public EnemyBulletPool EnemyBulletPool;
    public EnemyWave EnemyWave;
    public UFOWave UFOWave;

    public void Initialize(MapManager mapManager, ShieldManager shieldManager)
    {
        if (EnemyWave == null)
            EnemyWave = GetComponent<EnemyWave>();
        if (EnemyPool == null)
            EnemyPool = GetComponent<EnemyPool>();
        if (EnemyBulletPool == null)
            EnemyBulletPool = GetComponent<EnemyBulletPool>();

        EnemyPool.Initialize(EnemyBlueprints, transform);
        EnemyBulletPool.Initialize(EnemyBulletPrefab, null);
        EnemyWave.Initialize(mapManager, shieldManager, EnemyBlueprints, EnemyPool, EnemyBulletPool);
        UFOWave.Initialize(mapManager, EnemyPool, EnemyBlueprints);
    }

    public void KillEnemy(Enemy enemy, bool addPoints = false)
    {
        if (addPoints)
            GameplayManager.Instance.AddPoints(EnemyBlueprints.GetPointsByName(enemy.Name));

        if(enemy.Type == EnemyType.Normal)
            EnemyWave.ReturnEnemy(enemy);
        else if(enemy.Type == EnemyType.UFO)
            UFOWave.ReturnUFO(enemy);
    }

    public void ReturnBullet(EnemyBullet bullet)
    {
        EnemyBulletPool.Return(bullet);
    }

    private void OnDestroy()
    {
        EnemyPool.Dispose();
        EnemyBulletPool.Dispose();
    }
}
