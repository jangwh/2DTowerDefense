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
        public GameObject SelectTower;

        public TowerDatabase towerDatabase;

        public Text[] TowerPriceText;
        public Text Gold;
        public Text ReturnGameMenu;
        public Text BestScoreText;
        public Image BuyNoticeImage;
        public Text BuyNoticeText;

        public int[] towerPrice;
        int gold;

        public Button buySlotButton;
        public Text slotText;
        void Start()
        {
            UpdateUI();
            buySlotButton.onClick.AddListener(BuySlotExpansion);
            int bestScore = ScoreSave.GetBestScore();
            BestScoreText.text = $"최고 점수 : {bestScore}점";
            gold = ScoreSave.GetGold();
            TowerPriceText[0].text = $"기사 공격업 : {towerPrice[0]}";
            TowerPriceText[1].text = $"궁수 공격업 : {towerPrice[1]}";
            TowerPriceText[2].text = $"사제 체력업 : {towerPrice[2]}";
            TowerPriceText[3].text = $"솔져 공격업 : {towerPrice[3]}";
            TowerPriceText[4].text = $"도적 공격업 : {towerPrice[4]}";
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
        void SaveTowerDatabase()
        {
            TowerDatabase.TowerDataListWrapper wrapper = new TowerDatabase.TowerDataListWrapper
            {
                towers = towerDatabase.towers
            };
            string json = JsonUtility.ToJson(wrapper, true);
            PlayerPrefs.SetString("TowerDataOverride", json);
            PlayerPrefs.Save();
        }
        public void OnKnightUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= towerPrice[0])
            {
                BuyNoticeText.text = $"기사 공격력 상승";
                foreach ( TowerData towerData in towerDatabase.towers)
                {
                    if(towerData.towerName == "knight")
                    {
                        towerData.damage += 5;
                    }
                }
                SaveTowerDatabase();
                gold -= towerPrice[0];
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnArcherUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= towerPrice[1])
            {
                BuyNoticeText.text = $"궁수 공격력 상승";
                foreach (TowerData towerData in towerDatabase.towers)
                {
                    if (towerData.id == "archer")
                    {
                        towerData.damage += 5;

                    }
                }
                SaveTowerDatabase();
                gold -= towerPrice[1];
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnPriestUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= towerPrice[2])
            {
                BuyNoticeText.text = $"사제 체력 상승";
                foreach (TowerData towerData in towerDatabase.towers)
                {
                    if (towerData.id == "priest")
                    {
                        towerData.MaxHp += 5;
                    }
                }
                SaveTowerDatabase();
                gold -= towerPrice[2];
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnSoldiertUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= towerPrice[3])
            {
                BuyNoticeText.text = $"솔져 공격력 상승";
                foreach (TowerData towerData in towerDatabase.towers)
                {
                    if (towerData.towerName == "soldier")
                    {
                        towerData.damage += 5;
                    }
                }
                SaveTowerDatabase();
                gold -= towerPrice[3];
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnThiefUpgrade()
        {
            StartCoroutine(Notice());
            if (gold >= towerPrice[4])
            {
                BuyNoticeText.text = $"도적 공격력 상승";
                foreach (TowerData towerData in towerDatabase.towers)
                {
                    if (towerData.towerName == "thief")
                    {
                        towerData.damage += 5;
                    }
                }
                SaveTowerDatabase();
                gold -= towerPrice[4];
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnShopMenu()
        {
            GameMenu.SetActive(false);
            SelectTower.SetActive(false);
            ShopMenu.SetActive(true);
        }
        public void OnReturnGameMenu()
        {
            ShopMenu.SetActive(false);
            GameMenu.SetActive(true);
            SelectTower.SetActive(true);
        }
        IEnumerator Notice()
        {
            BuyNoticeImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            BuyNoticeImage.gameObject.SetActive(false);
        }
    }
}