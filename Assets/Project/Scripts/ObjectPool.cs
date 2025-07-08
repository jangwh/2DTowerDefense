using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using TowerDefense;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public Playerable knightPrefabs;
    public Playerable peasantPrefabs;
    public Playerable priestPrefabs;

    public Enemy zombiePrefabs;
    public Enemy orcPrefabs;

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

        PrewarmPool(knightPrefabs, 50);
        PrewarmPool(peasantPrefabs, 50);
        PrewarmPool(priestPrefabs, 50);
        PrewarmPool(zombiePrefabs, 50);
        PrewarmPool(orcPrefabs, 50);
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
        return LeanPool.Spawn(zombiePrefabs, position, Quaternion.identity);
    }
    public Enemy SpawnOrc(Vector3 position)
    {
        return LeanPool.Spawn(orcPrefabs, position, Quaternion.identity);
    }
}
