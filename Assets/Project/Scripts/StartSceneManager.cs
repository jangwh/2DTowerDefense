using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class StartSceneManager : MonoBehaviour
    {
        public TowerDatabase towerDatabase;
        public Transform gridParent;
        public GameObject slotPrefab;
        public Sprite defaultSprite;

        public List<string> selectedTowerIds = new List<string>();

        void Start()
        {
            selectedTowerIds.Clear(); 

            for (int i = 0; i < towerDatabase.towers.Count; i++)
            {
                TowerData data = towerDatabase.towers[i];
                GameObject slot = Instantiate(slotPrefab, gridParent);
                TowerSlotUI slotUI = slot.GetComponent<TowerSlotUI>();
                slotUI.Init(data, this);

                if (i < 3)
                {
                    selectedTowerIds.Add(data.id);
                    slotUI.SetSelected(true);
                }
                else
                {
                    slotUI.SetSelected(false); 
                }
            }
        }
        public void ToggleTowerSelection(string id)
        {
            int maxSelectableTowers = TowerSlotSave.GetMaxSlot();
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
            if (selectedTowerIds.Count > TowerSlotSave.GetMaxSlot())
            {
                Debug.LogWarning("슬롯 수보다 많은 타워가 선택됨");
                return;
            }
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
