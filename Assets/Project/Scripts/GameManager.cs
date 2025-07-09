using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        //TODO : 사운드 추가
        public static GameManager Instance { get; private set; }

        public Playerable[] playerables;
        public Enemy[] enemies;
        public TileGenerator tile;
        public DropTower[] TowerPos;
        public Button nextRoundBtn;
        public int roundCount = 0;
        public int[] enemyCount;

        public int MaxLifeCount;
        public int currentLifeCount;
        public int coin = 0;

        public int knightCoin = 10;
        public int archerCoin = 15;
        public int priestCoin = 5;

        public int enemySpawnCount;
        public bool isWin = false;
        public bool isDefeat = false;

        public static int priestNum = 1;

        Coroutine enemySpawnRoutine;
        Coroutine coinplus;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
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
            bool isRoundValid = roundCount >= 0 && roundCount < enemyCount.Length;

            if (enemySpawnCount == 0 && (roundCount  == enemyCount.Length))
            {
                isWin = true;
                SceneManager.LoadScene(2);
                return;
            }
            if (isRoundValid && enemyCount[roundCount] <= 0)
            {
                if (enemySpawnRoutine != null)
                {
                    StopCoroutine(enemySpawnRoutine);
                    enemySpawnRoutine = null;
                }
                if (isRoundValid)
                {
                    enemySpawnRoutine = StartCoroutine(EnemySpawnCoroutine());
                }
            }
            if (isRoundValid && enemySpawnCount == 0 && enemyCount[roundCount] <= 0)
            {
                nextRoundBtn.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            if (currentLifeCount <= 0)
            {
                isDefeat = true;
                SceneManager.LoadScene(2);
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
                if (roundCount < 0 || roundCount >= enemyCount.Length)
                {
                    yield break;
                }
                EnemySpawn();
                enemyCount[roundCount]--;
                enemySpawnCount++;
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
        public void OnNextRound()
        {
            if (roundCount + 1 > enemyCount.Length)
            {
                return;
            }
            // 안전하게 Despawn 하도록 복사본 사용
            List<GameObject> toDespawn = new List<GameObject>();
            
            foreach(DropTower dropTower in TowerPos)
            {
                dropTower.receivingImage.overrideSprite = null;
            }

            foreach (Transform child in tile.towerParent)
            {
                if (child.GetComponent<Playerable>() != null)
                {
                    toDespawn.Add(child.gameObject);
                }
            }
            foreach (GameObject obj in toDespawn)
            {
                string name = obj.GetComponent<Playerable>().charName;
                LeanPool.Despawn(obj);
                if (name == "Priest")
                {
                    priestNum--;
                }
            }
            roundCount++;
            Time.timeScale = 1f;
            ScoreSave.currentGold += 100;
            coin = 0;
            nextRoundBtn.gameObject.SetActive(false);
        }
    }
}
