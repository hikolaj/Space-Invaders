using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShipController : MonoBehaviour
{

    public bool Active = true;
    
    public int HP = 3;
    public float Speed = 5f;
    public float ShootDelay = 1f;

    public Transform BulletSpawnPoint;

    public PlayerBullet BulletPrefab;

    private ObjectPool<PlayerBullet> _bulletPool;
    
    private float _leftBoundary = -4.5f;
    private float _rightBoundary = 4.5f;


    private float _shootTimer;

    public void Initialize(MapManager mapManager)
    {
        _shootTimer = 0;

        _leftBoundary = mapManager.LeftBoundary + mapManager.CellSize / 2;
        _rightBoundary = mapManager.RightBoundary - mapManager.CellSize / 2;
        transform.position = new Vector3(0, mapManager.YPositionAtIndex(mapManager.TotalRowAmount), 0);

        _bulletPool = new ObjectPool<PlayerBullet>(CreateBullet);
    }


    public void Start()
    {
    }

    private void Update()
    {
        if (!Active)
            return;

        float moveInput = GetMoveInput();
        bool shootInput = GetShootInput();

        // Moving
        Move(moveInput);

        //Shooting
        if(_shootTimer > 0)
        {
            _shootTimer = Mathf.Max(_shootTimer - Time.deltaTime, 0);
        }
        else
        {
            if (shootInput)
            {
                Shoot();
                _shootTimer = ShootDelay;
            }
        }
    }

    public void Hit()
    {
        if(HP > 0)
        {
            HP -= 1;
            GameplayManager.Instance.GameplayCanvasManager.UpdateHealthValue(HP);
            if (HP == 0)
                GameplayManager.Instance.EndGame();
        }
    }

    private float GetMoveInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            return -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private bool GetShootInput()
    {
        if (Input.GetKey(KeyCode.Space))
            return true;
        else
            return false;
    }

    private void Move(float moveInput)
    {
        float xPos = transform.position.x + moveInput * Time.deltaTime * Speed;
        xPos = Mathf.Clamp(xPos, _leftBoundary, _rightBoundary);

        transform.position = new Vector3(xPos, transform.position.y, 0);
    }

    private void Shoot()
    {
        PlayerBullet bullet = _bulletPool.Get();
        bullet.transform.position = BulletSpawnPoint.position;
        bullet.gameObject.SetActive(true);
    }

    public void ReturnBullet(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletPool.Release(bullet);
    }

    private PlayerBullet CreateBullet()
    {
        PlayerBullet bullet = Instantiate(BulletPrefab);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnDestroy()
    {
        _bulletPool.Dispose();
    }
}
