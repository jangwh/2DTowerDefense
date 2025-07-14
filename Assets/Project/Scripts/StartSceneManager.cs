// Start 씬에서 타워 선택, 저장만 담당 (GameManager 분리 버전)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class StartSceneManager : MonoBehaviour
    {
        public TowerDatabase towerDatabase;
        public Transform gridParent;   // 슬롯들이 들어갈 부모 오브젝트 (예: Grid Layout Group)
        public GameObject slotPrefab;  // TowerSlotUI 프리팹
        public Sprite defaultSprite;

        // 선택한 타워 ID 목록 (예: ["knight", "archer", "priest"])
        public List<string> selectedTowerIds = new List<string>();

        // 최대 선택 가능한 타워 개수
        public int maxSelectableTowers = 5;
        void Start()
        {
            foreach (TowerData data in towerDatabase.towers)
            {
                GameObject slot = Instantiate(slotPrefab, gridParent);
                TowerSlotUI slotUI = slot.GetComponent<TowerSlotUI>();
                slotUI.Init(data, this);
            }
        }
        public void ToggleTowerSelection(string id)
        {
            if (selectedTowerIds.Contains(id))
            {
                selectedTowerIds.Remove(id);
            }
            else if (selectedTowerIds.Count < maxSelectableTowers)
            {
                selectedTowerIds.Add(id);
            }
            else
            {
                Debug.Log("최대 선택 수 초과");
            }

            Debug.Log("선택된 타워: " + string.Join(", ", selectedTowerIds));
        }

        public void StartGame()
        {
            Debug.Log("StartGame() 호출됨");
            if (selectedTowerIds.Count == 0)
            {
                Debug.LogWarning("선택된 타워가 없습니다.");
                return;
            }

            // JSON 저장
            SelectedTowerWrapper wrapper = new SelectedTowerWrapper
            {
                selected = selectedTowerIds
            };

            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString("SelectedTowers", json);
            PlayerPrefs.Save();

            Debug.Log("게임 시작 - 선택된 타워: " + string.Join(", ", selectedTowerIds));

            SceneManager.LoadScene("2DTDPlay"); 
        }
    }
}
