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
        private TowerSelectorUI selector;

        public void Init(TowerData data, TowerSelectorUI selector)
        {
            this.selector = selector;
            this.towerId = data.id;

            towerNameText.text = data.towerName;

            Sprite loadedSprite = Resources.Load<Sprite>(data.spritePath);
            towerImage.sprite = loadedSprite != null ? loadedSprite : selector.defaultSprite;

            selectButton.onClick.AddListener(() =>
            {
                selector.ToggleSelect(towerId);
                // 선택 상태에 따라 UI 효과 바꾸기 (색상 등)
            });
        }
    }
}