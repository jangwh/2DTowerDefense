using UnityEngine;
using UnityEngine.UI;
namespace TowerDefense
{
    public class TowerSlotUI : MonoBehaviour
    {
        public Image towerImage;
        public Text towerNameText;
        public Button selectButton;

        private string towerId;
        private StartSceneManager selector;

        private bool isSelected = false;
        private StartSceneManager manager;
        private TowerData towerData;

        public void Init(TowerData data, StartSceneManager mgr)
        {
            towerData = data;
            manager = mgr;
            towerImage.sprite = Resources.Load<Sprite>(data.spritePath);
            isSelected = false;
            UpdateVisual();


            selectButton.onClick.AddListener(ToggleSelect);
        }
        public void SetSelected(bool value)
        {
            isSelected = value;

            if (isSelected && !manager.selectedTowerIds.Contains(towerData.id))
                manager.selectedTowerIds.Add(towerData.id);
            else if (!isSelected && manager.selectedTowerIds.Contains(towerData.id))
                manager.selectedTowerIds.Remove(towerData.id);
            UpdateVisual();

        }
        void ToggleSelect()
        {
            int maxSlots = TowerSlotSave.GetMaxSlot();

            if (!isSelected && manager.selectedTowerIds.Count >= maxSlots)
            {
                Debug.Log("슬롯 제한 초과");
                return;
            }

            isSelected = !isSelected;

            if (isSelected)
                manager.selectedTowerIds.Add(towerData.id);
            else
                manager.selectedTowerIds.Remove(towerData.id);
            UpdateVisual();

        }
        void UpdateVisual()
        {
            towerImage.color = isSelected ? Color.white : new Color(1f, 1f, 1f, 0.3f);
        }

    }
}