using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBlueprints", menuName = "ScriptableObjects/EnemyBlueprints")]
public class EnemyBlueprints : ScriptableObject
{
    public EnemyBlueprint[] Enemies;

    public EnemyBlueprint GetBlueprintByName(string name)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            EnemyBlueprint enemyBP = Enemies[i];
            if (name == enemyBP.Prefab.name)
            {
                return enemyBP;
            }
        }

        return null;
    }

    public Enemy GetPrefabByName(string name)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            EnemyBlueprint enemyBP = Enemies[i];
            if (name == enemyBP.Prefab.name)
            {
                return enemyBP.Prefab;
            }
        }

        return null;
    }

    public int GetPointsByName(string name)
    {
        for(int i = 0; i < Enemies.Length; i++)
        {
            EnemyBlueprint enemyBP = Enemies[i];
            if (name == enemyBP.Prefab.name)
            {
                return enemyBP.Points;
            }
        }

        return 0;
    }

    public Enemy GetPrefabOfType(EnemyType type, int id)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemy enemy = Enemies[i].Prefab;
            if (enemy.Type == type && enemy.Id == id)
            {
                return enemy;
            }
        }

        return null;
    }

    public int CountEnemiesOfType(EnemyType type)
    {
        int count = 0;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].Prefab.Type == type)
                count += 1;
        }

        return count;
    }
}
