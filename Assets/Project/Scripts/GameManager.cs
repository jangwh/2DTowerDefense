using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        //TODO : 몬스터를 다 처치하면 시간정지, 다음 라운드 버튼 만들고 시작할 시 각종값들 초기화
        public static GameManager Instance { get; private set; }

        public Playerable[] playerables;
        public Enemy[] enemies;
        public TileGenerator tile;
        public int roundCount = 0;
        public int[] enemyCount;

        public int MaxLifeCount;
        public int currentLifeCount;
        public int coin = 0;

        public int knightCoin = 10;
        public int archerCoin = 15;
        public int priestCoin = 5;
        public int gold;

        public static int priestNum = 1;

        Coroutine enemySpawnRoutine;
        Coroutine coinplus;
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
            currentLifeCount = MaxLifeCount;
            enemySpawnRoutine  = StartCoroutine(EnemySpawnCoroutine());
            coinplus = StartCoroutine(CoinPlus());
        }
        void Update()
        {
            if (roundCount < enemyCount.Length && enemyCount[roundCount] <= 0)
            {
                if (enemySpawnRoutine != null)
                {
                    StopCoroutine(enemySpawnRoutine);
                    enemySpawnRoutine = null;
                }
                //roundCount++;//roundCount++다른곳에서 수행
                if (roundCount < enemyCount.Length)
                {
                    enemySpawnRoutine = StartCoroutine(EnemySpawnCoroutine());
                }
            }
        }
        IEnumerator CoinPlus()
        {
            float second = 1f;
            while (true)
            {
                float coinSpeed = second / Mathf.Max(priestNum, 1);
                WaitForSeconds wait = new WaitForSeconds(coinSpeed);
                yield return wait;
                coin++;
            }
        }
        IEnumerator EnemySpawnCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(5f);
            yield return wait;

            while (true)
            {
                EnemySpawn();
                enemyCount[roundCount]--;
                yield return wait;
            }
        }
        void EnemySpawn()
        {
            if (tile == null) return;
            int ranEnemy = Random.Range(0, enemies.Length);
            int ranTileY = Random.Range(-tile.genDisty, tile.genDisty + 1);

            if (roundCount == 0)
            {
                ObjectPool.Instance.SpawnZombie(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f)); 
            }
            else if (roundCount == 1)
            {
                if(ranEnemy == 0)
                {
                    ObjectPool.Instance.SpawnZombie(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                }
                else
                {
                    ObjectPool.Instance.SpawnOrc(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                }
            }
            else if (roundCount == 2)
            {
                ObjectPool.Instance.SpawnOrc(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
            }
            else if ( roundCount == 3)
            {
                ObjectPool.Instance.SpawnDreadnought(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
            }
        }
    }
}
