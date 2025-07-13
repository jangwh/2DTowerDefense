using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
namespace TowerDefense
{
    public class ShopManager : MonoBehaviour
    {
        public GameObject GameMenu;
        public GameObject ShopMenu;

        public Text KnightPrice;
        public Text ArcherPrice;
        public Text PriestPrice;
        public Text Gold;
        public Text ReturnGameMenu;
        public Text BestScoreText;
        public Image BuyNoticeImage;
        public Text BuyNoticeText;

        public int knightPrice;
        public int archerPrice;
        public int priestPrice;
        int gold;

        public Button buySlotButton;
        public Text slotText;
        //TODO : 상점에서 다른 타워 구매, 사용할 타워 배치
        //타워 강화시 레벨업해서 능력치 변경
        //ScriptableObject보다 Json 사용해볼것
        void Start()
        {
            UpdateUI();
            buySlotButton.onClick.AddListener(BuySlotExpansion);
            int bestScore = ScoreSave.GetBestScore();
            BestScoreText.text = $"최고 점수 : {bestScore}점";
            gold = ScoreSave.GetGold();
            KnightPrice.text = $"기사 공격력 강화 : {knightPrice}";
            ArcherPrice.text = $"궁수 공격력 강화 : {archerPrice}";
            PriestPrice.text = $"사제 체력 강화 : {priestPrice}";
            ReturnGameMenu.text = "게임 메뉴";
            ShopMenu.SetActive(false);
        }
        void Update()
        {
            Gold.text = $"소지금 : {gold}";
        }
        void BuySlotExpansion()
        {
            int current = TowerSlotSave.GetMaxSlot();
            if (current < 5 && gold >= 100)
            {
                gold -= 100;
                TowerSlotSave.SetMaxSlot(current + 1);
                UpdateUI();
            }
            else
            {
                Debug.Log("슬롯 최대치 도달 또는 코인 부족");
            }
        }

        void UpdateUI()
        {
            int current = TowerSlotSave.GetMaxSlot();
            slotText.text = $"슬롯: {current}/5";

            if (current >= 5)
            {
                buySlotButton.interactable = false;
                slotText.text = "최대 슬롯입니다";
            }
        }
        public void OnKnightUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= knightPrice)
            {
                BuyNoticeText.text = $"기사 공격력 상승";
                GameManager.Instance.playerables[0].damage += 5;
                gold -= knightPrice;
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnArcherUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= archerPrice)
            {
                BuyNoticeText.text = $"궁수 공격력 상승";
                GameManager.Instance.playerables[1].damage += 5;
                gold -= archerPrice;
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnPriestUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= priestPrice)
            {
                BuyNoticeText.text = $"사제 체력 상승";
                GameManager.Instance.playerables[2].maxHp += 5;
                gold -= priestPrice;
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnShopMenu()
        {
            GameMenu.SetActive(false);
            ShopMenu.SetActive(true);
        }
        public void OnReturnGameMenu()
        {
            ShopMenu.SetActive(false);
            GameMenu.SetActive(true);
        }
        IEnumerator Notice()
        {
            BuyNoticeImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            BuyNoticeImage.gameObject.SetActive(false);
        }
    }
}