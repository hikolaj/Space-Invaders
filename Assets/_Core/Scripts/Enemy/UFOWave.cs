using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOWave : MonoBehaviour
{
    public bool UFOIsSpawned { get; private set; }
    public bool Stop = false;

    public float Speed = 1f;
    public float SpawnDelay = 5f;

    private Enemy _spawnedUFO;
    private float _spawnTimer;

    private MapManager _mapManager;
    private EnemyPool _enemyPool;
    private EnemyBlueprints _enemyBlueprints;

    private const float _boundaryOffset = 1f;

    public void Initialize(MapManager mapManager, EnemyPool enemyPool, EnemyBlueprints enemyBlueprints)
    {
        _mapManager = mapManager;
        _enemyPool = enemyPool;
        _enemyBlueprints = enemyBlueprints;
        _spawnTimer = 0;
    }

    void Update()
    {
        if (Stop)
            return;

        if (UFOIsSpawned)
        {
            MoveUFO(_spawnedUFO);
        }
        else
        {
            if (_spawnTimer >= SpawnDelay)
            {
                SpawnUFO();
            }
            else
            {
                _spawnTimer += Time.deltaTime;
            }
        }
    }

    public void SpawnUFO()
    {
        if (_spawnedUFO == null)// get new
        {
            string ufoName = _enemyBlueprints.GetPrefabOfType(EnemyType.UFO, 0).Name;
            _spawnedUFO = _enemyPool.Get(ufoName);
            _spawnedUFO.gameObject.SetActive(true);
        }

        _spawnedUFO.transform.position = new Vector2(_mapManager.LeftBoundary - _boundaryOffset, _mapManager.TopOfMap - _mapManager.CellSize/2);

        _spawnTimer = 0;
        UFOIsSpawned = true;
    }

    private void MoveUFO(Enemy ufo)
    {
        Vector2 pos = ufo.transform.position;
        pos.x += Time.deltaTime * Speed;
        ufo.transform.position = pos;

        if (pos.x >= _mapManager.RightBoundary + _boundaryOffset)
            ReturnUFO(ufo);

    }

    public void ReturnUFO(Enemy enemy)
    {
        UFOIsSpawned = false;
        _spawnedUFO = null;
        _enemyPool.Return(enemy);
    }
}
