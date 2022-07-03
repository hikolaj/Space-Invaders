using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    private Dictionary<string, ObjectPool<Enemy>> _enemyPools;
    private EnemyBlueprints _enemyBlueprints;
    private Transform _parent;

    public void Initialize(EnemyBlueprints enemyBlueprints, Transform parent)
    {
        _parent = parent;
        _enemyBlueprints = enemyBlueprints;
        _enemyPools = new Dictionary<string, ObjectPool<Enemy>>();
        foreach (EnemyBlueprint bp in _enemyBlueprints.Enemies)
        {
            string name = bp.Prefab.name;
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(() => Create(name));
            _enemyPools.Add(name, pool);
        }
    }

    public void Return(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _enemyPools[enemy.Name].Release(enemy);
    }

    public Enemy Get(Enemy prefab)
    {
        Enemy enemy = _enemyPools[prefab.Name].Get();
        return enemy;
    }

    public Enemy Get(string name)
    {
        Enemy enemy = _enemyPools[name].Get();
        return enemy;
    }

    private Enemy Create(string name)
    {
        Enemy enemy = Instantiate(_enemyBlueprints.GetPrefabByName(name));
        enemy.transform.parent = _parent;
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    public void Dispose()
    {
        foreach (EnemyBlueprint bp in _enemyBlueprints.Enemies)
        {
            string name = bp.Prefab.name;
            _enemyPools[name].Dispose();
        }
    }
}
