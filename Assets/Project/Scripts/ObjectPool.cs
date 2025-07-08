using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using TowerDefense;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public Playerable[] playerablePrefabs;

    public Enemy[] enemyPrefabs;

    void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else if(Instance != this)
        {
            DestroyImmediate(this);
            return;
        }

        PrewarmPool(playerablePrefabs[0], 50);
        PrewarmPool(playerablePrefabs[1], 50);
        PrewarmPool(playerablePrefabs[2], 50);
        PrewarmPool(enemyPrefabs[0], 50);
        PrewarmPool(enemyPrefabs[1], 50);
        PrewarmPool(enemyPrefabs[2], 50);
    }
    void PrewarmPool<T>(T prefab, int count) where T : Component
    {
        for (int i = 0; i < count; i++)
        {
            var obj = LeanPool.Spawn(prefab, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            LeanPool.Despawn(obj);
        }
    }
    public Enemy SpawnZombie(Vector3 position)
    {
        return LeanPool.Spawn(enemyPrefabs[0], position, Quaternion.identity);
    }
    public Enemy SpawnOrc(Vector3 position)
    {
        return LeanPool.Spawn(enemyPrefabs[1], position, Quaternion.identity);
    }
    public Enemy SpawnDreadnought(Vector3 position)
    {
        return LeanPool.Spawn(enemyPrefabs[2], position, Quaternion.identity);
    }
}
