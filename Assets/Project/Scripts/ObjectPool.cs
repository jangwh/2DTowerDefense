using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using TowerDefense;

public class ObjectPool : MonoBehaviour
{
    public ObjectPool Instance { get; private set; }

    public Playerable knightPrefabs;
    public Playerable peasantPrefabs;
    public Playerable priestPrefabs;

    public Enemy zombiePrefabs;
    public Enemy orcPrefabs;


    public List<Playerable> knightPrefabsPool = new List<Playerable>();
    public List<Playerable> peasantPrefabsPool = new List<Playerable>();
    public List<Playerable> priestPrefabsPool = new List<Playerable>();

    public List<Enemy> zombiePool = new List<Enemy>();
    public List<Enemy> orcPool = new List<Enemy>();

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
    }
    void Start()
    {
        for(int i = 0;  i < 50; i++)
        {
            Playerable knight = Instantiate(knightPrefabs, transform);
            knight.gameObject.SetActive(false);
            knightPrefabsPool.Add(knight);
        }
        for (int i = 0; i < 50; i++)
        {
            Playerable peasant = Instantiate(peasantPrefabs, transform);
            peasant.gameObject.SetActive(false);
            peasantPrefabsPool.Add(peasant);
        }
        for (int i = 0; i < 50; i++)
        {
            Playerable priest = Instantiate(priestPrefabs, transform);
            priest.gameObject.SetActive(false);
            priestPrefabsPool.Add(priest);
        }
        for (int i = 0; i < 50; i++)
        {
            Enemy zombie = Instantiate(zombiePrefabs, transform);
            zombie.gameObject.SetActive(false);
            zombiePool.Add(zombie);
        }
        for (int i = 0; i < 50; i++)
        {
            Enemy orc = Instantiate(orcPrefabs, transform);
            orc.gameObject.SetActive(false);
            orcPool.Add(orc);
        }
    }
}
