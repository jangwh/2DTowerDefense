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
        public static GameManager Instance { get; private set; }

        public TowerDatabase towerDatabase;
        public Transform[] towerSpawnPositions; // 인스펙터에 3칸 지정
        private List<TowerData> selectedTowers = new List<TowerData>();
        public GameObject dragSlotPrefab; // 드래그 프리팹
        public Transform dragSlotParent;  // UI 부모 오브젝트

        public List<SpriteToPrefab> spritePrefabMappings;
        public List<GameObject> allSlots; // Inspector에 Slot들 순서대로 넣기

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
            enemySpawnRoutine = StartCoroutine(EnemySpawnCoroutine());
            coinplus = StartCoroutine(CoinPlus());

            LoadSelectedTowers();
            SpawnSelectedTowers();

            string json = PlayerPrefs.GetString("SelectedTowers", "");

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("⛔ 선택된 타워 데이터 없음");
                return;
            }

            SelectedTowerWrapper wrapper = JsonUtility.FromJson<SelectedTowerWrapper>(json);
            if (wrapper == null || wrapper.selected == null)
            {
                Debug.LogWarning("⛔ 타워 데이터 파싱 실패");
                return;
            }


            foreach (string id in wrapper.selected)
            {
                TowerData data = towerDatabase.FindById(id);
                if (data == null) continue;

                // 드래그 슬롯 생성
                GameObject slot = Instantiate(dragSlotPrefab, dragSlotParent);
                Image image = slot.GetComponent<Image>();
                DragMe dragMe = slot.GetComponent<DragMe>();

                Sprite sprite = Resources.Load<Sprite>(data.spritePath);
                if (image != null) image.sprite = sprite;

                // SpriteToPrefab 매핑에 추가
                SpriteToPrefab mapping = new SpriteToPrefab
                {
                    sprite = sprite,
                    prefab = Resources.Load<GameObject>(data.prefabPath)
                };
                spritePrefabMappings.Add(mapping);
            }
            foreach (GameObject slot in allSlots)
            {
                string slotId = slot.name.Replace("Slot", "").ToLower(); // ex: "KnightSlot" → "knight"
                bool isSelected = wrapper.selected.Contains(slotId);
                slot.SetActive(isSelected);
            }

            // DropTower 쪽에 spritePrefabMappings 전달
            DropTower[] dropTowers = FindObjectsOfType<DropTower>();
            foreach (DropTower drop in dropTowers)
            {
                drop.spritePrefabMappings = spritePrefabMappings;
            }
        }

        void LoadSelectedTowers()
        {
            string json = PlayerPrefs.GetString("SelectedTowers", "");

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("선택된 타워 데이터가 없습니다. 기본 타워를 로드합니다.");
                return;
            }

            SelectedTowerWrapper wrapper = JsonUtility.FromJson<SelectedTowerWrapper>(json);
            if (wrapper == null || wrapper.selected == null)
            {
                Debug.LogWarning("선택된 타워 JSON 파싱 실패");
                return;
            }

            foreach (string id in wrapper.selected)
            {
                TowerData data = towerDatabase.FindById(id);
                if (data == null)
                {
                    Debug.LogError($"❌ TowerDatabase에서 ID '{id}' 찾지 못함");
                }
                else
                {
                    Debug.Log($"✅ TowerDatabase에서 타워 로드: {data.id} → {data.prefabPath}");
                    selectedTowers.Add(data);
                }
            }
        }

        void SpawnSelectedTowers()
        {
            Debug.Log($"[SpawnSelectedTowers] 선택된 타워 수: {selectedTowers.Count}");

            for (int i = 0; i < selectedTowers.Count && i < towerSpawnPositions.Length; i++)
            {
                TowerData data = selectedTowers[i];
                GameObject towerPrefab = Resources.Load<GameObject>(data.prefabPath);

                if (towerPrefab == null)
                {
                    Debug.LogError($"❌ 프리팹 로드 실패: {data.prefabPath}");
                    continue;
                }

                Debug.Log($"✅ 프리팹 로드 성공: {data.prefabPath}");

                Vector3 spawnPos = towerSpawnPositions[i].position;
                GameObject obj = Instantiate(towerPrefab, spawnPos, Quaternion.identity);
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
            Enemy enemy;
            if (roundCount == 0)
            {
                enemy = ObjectPool.Instance.SpawnZombie(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                enemy.Init();
            }
            else if (roundCount == 1)
            {
                if (ranEnemy == 0)
                {
                    enemy = ObjectPool.Instance.SpawnZombie(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                    enemy.Init();
                }
                else
                {
                    enemy = ObjectPool.Instance.SpawnOrc(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                    enemy.Init();
                }
            }
            else if (roundCount == 2)
            {
                enemy = ObjectPool.Instance.SpawnOrc(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                enemy.Init();
            }
            else if (roundCount == 3)
            {
                enemy = ObjectPool.Instance.SpawnDreadnought(new Vector2((tile.genDistx * 2) + 1, (ranTileY * 2f) + 1f));
                enemy.Init();
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

            if (roundCount == 3)
            {
                StartCoroutine(Warning());
            }
        }
        IEnumerator Warning()
        {
            warning.gameObject.SetActive(true);
            warning.GetComponent<Animator>();
            yield return new WaitForSeconds(2f);
            warning.gameObject.SetActive(false);
        }
    }
}
