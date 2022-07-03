using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShieldManager : MonoBehaviour
{
    public bool ShieldsDestroyed;

    public Shield Prefab;
    public int Amount = 4;
    public int SizeX = 2;
    public int SizeY = 1;
    public int YPositionIndexOffset = -3;

    private Shield[] _shieldsOnScene;
    
    public void Initialize(MapManager mapManager)
    {
        _shieldsOnScene = new Shield[4];
        PlaceShields(mapManager);
    }

    public void PlaceShields(MapManager mapManager)
    {
        // place shields
        float cellSize = mapManager.CellSize;
        int gapAmount = (Amount + 1);
        int gapSize = (mapManager.TotalCollumnAmount - Amount * SizeX) / gapAmount;
        float offsetBySize = cellSize * (SizeX - 1) / 2;// at size 1 it shoudnt be offseting

        Vector2 size = new Vector2((float)SizeX * cellSize, (float)SizeY * cellSize);
        Vector3 pos = mapManager.PositionAtIndex(gapSize, mapManager.TotalRowAmount + YPositionIndexOffset);
        pos.x += offsetBySize;

        for (int i = 0; i < Amount; i++)
        {

            // instantiate
            Shield newShield = Instantiate(Prefab);
            newShield.transform.parent = transform;

            // sizes
            newShield.transform.localScale = size;

            // positions
            newShield.transform.position = pos;
            pos.x += (gapSize + 1 + SizeX / 2) * cellSize;

            //save
            _shieldsOnScene[i] = newShield;
        }

        ShieldsDestroyed = false;
    }

    public void DestroyShields()
    {
        for(int i = 0; i < Amount; i++)
        {
            if(_shieldsOnScene[i] != null)
            {
                _shieldsOnScene[i].gameObject.SetActive(false);
                Destroy(_shieldsOnScene[i].gameObject);
            }
        }
        ShieldsDestroyed = true;
    }
}
