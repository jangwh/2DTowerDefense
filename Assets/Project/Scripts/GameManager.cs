using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public TowerDatabase towerDatabase;
        public Transform[] towerSpawnPositions;

        public List<SpriteToPrefab> spritePrefabMappings;
        public List<GameObject> allSlots;

        public Playerable[] playerables;
        public Enemy[] enemies;
        public TileGenerator tile;
        public DropTower[] TowerPos;
        public Button nextRoundBtn;
        public Image warning;

        public int roundCount = 0;
        public int[] enemyCount;

        public int MaxLifeCount;
        public int currentLifeCount;
        public int coin = 0;

        public int[] towerCoin;

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
            }
            else if (Instance != this)
            {
                DestroyImmediate(this);
                return;
            }
        }

        void Start()
        {
            ScoreSave.currentScore = 0;
            currentLifeCount = MaxLifeCount;
            enemySpawnRoutine = StartCoroutine(EnemySpawnCoroutine());
            coinplus = StartCoroutine(CoinPlus());

            SetupDragSlots();
        }

        void SetupDragSlots()
        {
            string json = PlayerPrefs.GetString("SelectedTowers", "");
            SelectedTowerWrapper wrapper = JsonUtility.FromJson<SelectedTowerWrapper>(json);

            if (wrapper == null || wrapper.selected == null) return;

            foreach (GameObject slot in allSlots)
            {
                string slotId = slot.name.Replace("Slot", "").ToLower();
                bool isSelected = wrapper.selected.Contains(slotId);
                slot.SetActive(isSelected);
            }
        }
        void Update()
        {
            bool isRoundValid = roundCount >= 0 && roundCount < enemyCount.Length;

            if (enemySpawnCount == 0 && (roundCount == enemyCount.Length))
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
                yield return new WaitForSeconds(coinSpeed);
                coin++;
            }
        }

        IEnumerator EnemySpawnCoroutine()
        {
            yield return new WaitForSeconds(5f);

            while (true)
            {
                if (roundCount < 0 || roundCount >= enemyCount.Length) yield break;

                EnemySpawn();
                enemyCount[roundCount]--;
                enemySpawnCount++;
                yield return new WaitForSeconds(5f);
            }
        }

        void EnemySpawn()
        {
            if (tile == null) return;
            int ranEnemy = Random.Range(0, 2);
            int ranTileY = Random.Range(-tile.genDisty, tile.genDisty + 1);

            Vector2 spawnPos = new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f);
            Enemy enemy = null;

            if (roundCount == 0 || (roundCount == 1 && ranEnemy == 0))
            {
                enemy = ObjectPool.Instance.SpawnZombie(spawnPos);
            }
            else if ((roundCount == 1 && ranEnemy == 1) || roundCount == 2)
            {
                enemy = ObjectPool.Instance.SpawnOrc(spawnPos);
            }
            else if (roundCount == 3)
            {
                enemy = ObjectPool.Instance.SpawnDreadnought(spawnPos);
            }

            if (enemy != null) enemy.Init();
        }

        public void OnNextRound()
        {
            if (roundCount + 1 > enemyCount.Length) return;

            List<GameObject> toDespawn = new List<GameObject>();

            foreach (DropTower dropTower in TowerPos)
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
                if (name == "priest") priestNum--;
            }

            roundCount++;
            Time.timeScale = 1f;
            ScoreSave.currentGold += 100;
            coin = 0;
            nextRoundBtn.gameObject.SetActive(false);

            if (roundCount == 3)
            {
                StartCoroutine(Warning());
            }
        }

        IEnumerator Warning()
        {
            warning.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            warning.gameObject.SetActive(false);
        }
    }
}
