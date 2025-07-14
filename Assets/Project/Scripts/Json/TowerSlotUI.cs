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

        public void Init(TowerData data, StartSceneManager selector)
        {
            this.selector = selector;
            this.towerId = data.id;

            towerNameText.text = data.towerName;

            Sprite loadedSprite = Resources.Load<Sprite>(data.spritePath);
            towerImage.sprite = loadedSprite;

            selectButton.onClick.AddListener(() =>
            {
                selector.ToggleTowerSelection(towerId);
            });
        }
    }
}