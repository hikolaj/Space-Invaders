using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public int Points { get; private set; }

    public GameplayCanvasManager GameplayCanvasManager;
    public EnemyManager EnemyManager;
    public MapManager MapManager;
    public ShieldManager ShieldManager;
    public PlayerShipController PlayerShip;

    private bool _newWavePause = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Points = 0;

        MapManager.Initialize();
        EnemyManager.Initialize(MapManager, ShieldManager);
        ShieldManager.Initialize(MapManager);
        PlayerShip.Initialize(MapManager);

        NewWave();
    }

    void Update()
    {
        if (_newWavePause)
        {
            if (!EnemyManager.EnemyWave.PlacingNewWave)
            {
                PlayerShip.Active = true;
                _newWavePause = false;
            }
        }
    }

    public void AddPoints(int amount)
    {
        Points += amount;
        GameplayCanvasManager.UpdateScoreValueText(Points);
    }

    public void NewWave()
    {
        _newWavePause = true;
        PlayerShip.Active = false;
        EnemyManager.EnemyWave.NewWave();
    }

    public void EndGame()
    {
        PlayerShip.Active = false;
        CoreManager.Instance.OpenEndScreen();
    }

}
