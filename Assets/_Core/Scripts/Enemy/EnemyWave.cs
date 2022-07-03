using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public bool Stop = false;
    public bool PlacingNewWave { get; private set; }

    public int CollumnAmount = 11;
    public int RowAmount = 6;
    public int PerEnemyRowCount = 2;

    public float StepSize = 0.5f;
    public float StepDuration = 1;
    public float StepDurationSubtractor = 0.1f;

    public float ShootDelay = 2f;
    public float ShootDelaySubtractor = 0.2f;

    private MapManager _mapManager;
    private ShieldManager _shieldManager;
    private EnemyBlueprints _enemyBlueprints;
    private EnemyPool _enemyPool;
    private EnemyBulletPool _enemyBulletPool;


    private Enemy[,] _enemyGrid;
    private int _enemyCount;
    
    private WaveDirection _direction;
    private float[] _gridXOffsets;
    private float _gridYOffset;
    private float _stepDuration;
    private float _stepTimer;
    private float _rowStepTimer;
    private int _rowStepId;

    private float _shootDelay;
    private float _shootTimer;

    private Coroutine _placingNewWaveCoroutine;

    private const float _placingNewEnemyDelay = 0.05f;
    private const float _minStepDuration = 0.1f;
    private const float _minShootDelay = 0.2f;

    public void Initialize(MapManager mapManager, ShieldManager shieldManager, EnemyBlueprints enemyBlueprints, EnemyPool enemyPool, EnemyBulletPool enemyBulletPool)
    {
        _mapManager = mapManager;
        _shieldManager = shieldManager;
        _enemyBlueprints = enemyBlueprints;
        _enemyPool = enemyPool;
        _enemyBulletPool = enemyBulletPool;
        _enemyGrid = new Enemy[CollumnAmount, RowAmount];
        _gridXOffsets = new float[RowAmount];
    }

    void Update()
    {
        if (Stop)
            return;

        Movement();
        Shooting();
    }

    public void Movement()
    {
        // Step Finish
        if (_stepTimer >= _stepDuration)
        {
            _stepTimer = 0;
            _rowStepTimer = 0;
            _rowStepId = 0;

            // Down movement
            if (ReachedBoundary(_direction))
            {
                _direction = _direction == WaveDirection.left ? WaveDirection.right : WaveDirection.left;
                _gridYOffset += 1;
                _stepDuration = Mathf.Max(_minStepDuration, _stepDuration - StepDurationSubtractor);
                _shootDelay = Mathf.Max(_minShootDelay, _shootDelay - ShootDelaySubtractor);

                UpdateEnemyPositions();

                //Destroy shields
                if (!_shieldManager.ShieldsDestroyed && ReachedShields())
                {
                    _shieldManager.DestroyShields();
                }

                //End of the game
                if (ReachedPlayer())
                {
                    GameplayManager.Instance.EndGame();
                    Stop = true;
                    return;
                }
            }
        }

        // Step per row Movement
        if (_rowStepId < RowAmount && _rowStepTimer >= _stepDuration / 2 / RowAmount)
        {
            float stepValue = _direction == WaveDirection.left ? -StepSize : StepSize;
            int eversedRowStepId = RowAmount - _rowStepId - 1;// reverse row stepping to make it from bottom to top
            _gridXOffsets[eversedRowStepId] += stepValue;

            UpdateEnemyPositionsInRow(eversedRowStepId);

            _rowStepTimer = 0;
            _rowStepId += 1;
        }

        // Update timers
        _stepTimer += Time.deltaTime;
        _rowStepTimer += Time.deltaTime;
    }

    public void Shooting()
    {
        if(_shootTimer >= _shootDelay)
        {
            GetRandomEnemyInGrid().Shoot(_enemyBulletPool);
            _shootTimer = 0;
        }
        _shootTimer += Time.deltaTime;
    }


    public void NewWave()
    {
        Stop = true;
        PlacingNewWave = true;

        ClearGrid();
        
        _shootDelay = ShootDelay;
        _shootTimer = 0;
        _direction = WaveDirection.right;
        _stepDuration = StepDuration;
        _stepTimer = 0;
        _rowStepId = 0;
        _gridYOffset = 0;
        for (int i = 0; i < RowAmount; i++)
            _gridXOffsets[i] = (int)(_mapManager.TotalCollumnAmount / 2 - CollumnAmount / 2) - (CollumnAmount % 2);

        // Place new wave
        if (_placingNewWaveCoroutine != null)
            StopCoroutine(_placingNewWaveCoroutine);
        _placingNewWaveCoroutine = StartCoroutine(PlaceNewWave());
    }

    IEnumerator PlaceNewWave()
    {
        int enemyId = 0;
        int enemyOfTypeCount = _enemyBlueprints.CountEnemiesOfType(EnemyType.Normal);

        for (int y = RowAmount - 1; y >= 0; y--)
        {
            for (int x = 0; x < CollumnAmount; x++)
            {
                string name = _enemyBlueprints.GetPrefabOfType(EnemyType.Normal, enemyId).Name;
                Enemy enemy = _enemyPool.Get(name);
                enemy.IdX = x;
                enemy.IdY = y;
                enemy.transform.position = EnemyPositionAtIndex(x, y);
                enemy.gameObject.SetActive(true);
                _enemyGrid[x, y] = enemy;
                _enemyCount += 1;

                yield return new WaitForSeconds(_placingNewEnemyDelay);
            }
            enemyId += y % PerEnemyRowCount == 0 ? 1 : 0;
            enemyId = Mathf.Min(enemyId, enemyOfTypeCount - 1);
        }

        PlacingNewWave = false;
        Stop = false;
    }

    private void UpdateEnemyPositionsInRow(int IdY)
    {
        for (int x = 0; x < CollumnAmount; x++)
        {
            Enemy enemy = _enemyGrid[x, IdY];
            if (enemy != null)
                enemy.transform.position = EnemyPositionAtIndex(x, IdY);
        }
    }

    private void UpdateEnemyPositions()
    {
        for (int x = 0; x < CollumnAmount; x++)
        {
            for (int y = 0; y < RowAmount; y++)
            {
                Enemy enemy = _enemyGrid[x, y];
                if (enemy != null)
                    enemy.transform.position = EnemyPositionAtIndex(x, y);
            }
        }
    }

    private Vector2 EnemyPositionAtIndex(int x, int y)
    {
        return _mapManager.PositionAtIndex(x + _gridXOffsets[y], y + _gridYOffset);
    }

    public void ReturnEnemy(Enemy enemy)
    {
        _enemyGrid[enemy.IdX, enemy.IdY] = null;
        _enemyPool.Return(enemy);
        _enemyCount -= 1;

        if(_enemyCount <= 0)
        {
            Stop = true;
            GameplayManager.Instance.NewWave();
        }
    }

    public void ClearGrid()
    {
        for (int x = 0; x < CollumnAmount; x++)
        {
            for (int y = 0; y < RowAmount; y++)
            {
                Enemy enemy = _enemyGrid[x, y];
                if (enemy != null)
                    ReturnEnemy(enemy);
            }
        }
        _enemyCount = 0;
    }

    private bool ReachedBoundary(WaveDirection waveDir)
    {
        if (waveDir == WaveDirection.right)
        {
            int mostRightId = 0;

            for (int y = 0; y < RowAmount; y++)
            {
                for (int x = 0; x < CollumnAmount; x++)
                {
                    if (_enemyGrid[x, y] != null && x > mostRightId)
                        mostRightId = x;
                }
            }

            if (mostRightId + _gridXOffsets[0] >= _mapManager.TotalCollumnAmount - 1)
                return true;
            else
                return false;
        }
        else
        {
            int mostLeftId = CollumnAmount;

            for (int y = 0; y < RowAmount; y++)
            {
                for (int x = 0; x < CollumnAmount; x++)
                {
                    if (_enemyGrid[x, y] != null && x < mostLeftId)
                        mostLeftId = x;
                }
            }

            if (mostLeftId + _gridXOffsets[0] <= 0)
                return true;
            else
                return false;
        }
    }

    private bool ReachedShields()
    {
        int mostBottomId = 0;

        for (int y = 0; y < RowAmount; y++)
        {
            for (int x = 0; x < CollumnAmount; x++)
            {
                if (_enemyGrid[x, y] != null && y > mostBottomId)
                    mostBottomId = y;
            }
        }

        if (_gridYOffset + mostBottomId >= _mapManager.TotalRowAmount + _shieldManager.YPositionIndexOffset)
            return true;
        else
            return false;
    }

    private bool ReachedPlayer()
    {
        int mostBottomId = 0;

        for (int y = 0; y < RowAmount; y++)
        {
            for (int x = 0; x < CollumnAmount; x++)
            {
                if (_enemyGrid[x, y] != null && y > mostBottomId)
                    mostBottomId = y;
            }
        }

        if (_gridYOffset + mostBottomId >= _mapManager.TotalRowAmount)
            return true;
        else
            return false;
    }

    private Enemy GetRandomEnemyInGrid()
    {
        Enemy enemy = null;
        while(enemy == null)
        {
            int x = Random.Range(0, CollumnAmount);
            int y = Random.Range(0, RowAmount);
            enemy = _enemyGrid[x, y];
        }

        return enemy;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
