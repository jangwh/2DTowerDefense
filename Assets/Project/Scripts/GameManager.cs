using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Playerable[] playerables;
        public Enemy[] enemies;
        public TileGenerator tile;
        public int roundCount = 0;
        public int[] enemyCount;

        Coroutine enemySpawnRoutine;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                DestroyImmediate(this);
                return;
            }
        }
        void Start()
        {
            enemySpawnRoutine  = StartCoroutine(EnemySpawnCoroutine());
        }
        void Update()
        {
            if (roundCount < enemyCount.Length && enemyCount[roundCount] <= 0)
            {
                if (enemySpawnRoutine != null)
                {
                    StopCoroutine(enemySpawnRoutine);
                }
                roundCount++;
                if (roundCount < enemyCount.Length)
                {
                    enemySpawnRoutine = StartCoroutine(EnemySpawnCoroutine());
                }
            }
        }
        IEnumerator EnemySpawnCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);

            while (true)
            {
                EnemySpawn();
                enemyCount[roundCount]--;
                yield return wait;
            }
        }
        void EnemySpawn()
        {
            //TODO: Enemy 스폰 코루틴으로 만들기
            if (tile == null) return;
            int ranEnemy = Random.Range(0, enemies.Length);
            int ranTileY = Random.Range(-tile.genDisty, tile.genDisty + 1);

            LeanPool.Spawn(enemies[ranEnemy], new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1), Quaternion.identity);
        }
    }
}
