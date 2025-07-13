using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace TowerDefense
{
    public class TowerSelectorUI : MonoBehaviour
    {
        public TowerDatabase database; // Inspector에 연결
        public Transform gridParent;   // 슬롯들이 들어갈 부모 오브젝트 (예: Grid Layout Group)
        public GameObject slotPrefab;  // TowerSlotUI 프리팹
        public Sprite defaultSprite;

        public List<string> selectedTowerIds = new List<string>();

        void Start()
        {
            foreach (TowerData data in database.towers)
            {
                GameObject slot = Instantiate(slotPrefab, gridParent);
                TowerSlotUI slotUI = slot.GetComponent<TowerSlotUI>();
                slotUI.Init(data, this);
            }
        }

        public void ToggleSelect(string id)
        {
            int max = TowerSlotSave.GetMaxSlot();

            if (selectedTowerIds.Contains(id))
            {
                selectedTowerIds.Remove(id);
            }
            else if (selectedTowerIds.Count < max)
            {
                selectedTowerIds.Add(id);
            }

            Debug.Log("선택된 타워: " + string.Join(", ", selectedTowerIds));
        }

        public void StartGame()
        {
            string json = JsonUtility.ToJson(new SelectedTowerWrapper { selected = selectedTowerIds });
            PlayerPrefs.SetString("SelectedTowers", json);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}