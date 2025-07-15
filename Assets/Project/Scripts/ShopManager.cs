using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TowerDefense
{
    public class ShopManager : MonoBehaviour
    {
        public StartSceneManager StartSceneManager;

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
        public Text useTower;

        public int[] towerPrice;
        int gold;

        public Button[] buyButtons;
        public Text slotText;
        void Start()
        {
            UpdateUI();
            buyButtons[5].onClick.AddListener(BuySlotExpansion);
            int bestScore = ScoreSave.GetBestScore();
            BestScoreText.text = $"최고 점수 : {bestScore}점";
            gold = ScoreSave.GetGold();
            ReturnGameMenu.text = "게임 메뉴";
            ShopMenu.SetActive(false);
        }
        void Update()
        {
            int current = TowerSlotSave.GetMaxSlot();
            Gold.text = $"소지금 : {gold}";
            useTower.text = $"선택된 타워 : {StartSceneManager.selectedTowerIds.Count}\n사용가능 타워 : {current}";
        }
        void BuySlotExpansion()
        {
            int current = TowerSlotSave.GetMaxSlot();
            if (current < 5 && gold >= 100)
            {
                gold -= 100;
                ScoreSave.SaveGold(100);
                TowerSlotSave.SetMaxSlot(current + 1);
                UpdateUI();
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }

        void UpdateUI()
        {
            int current = TowerSlotSave.GetMaxSlot();
            slotText.text = $"슬롯: {current}/5";

            int knightUpgrade = TowerSlotSave.GetKnightUpgrade();
            int archerUpgrade = TowerSlotSave.GetArcherUpgrade();
            int priestUpgrade = TowerSlotSave.GetPriestUpgrade();
            int soldierUpgrade = TowerSlotSave.GetSoldierUpgrade();
            int thiefUpgrade = TowerSlotSave.GetThiefUpgrade();
            if (knightUpgrade >= 5)
            {
                buyButtons[0].interactable = false;
                TowerPriceText[0].text = "최대 강화";
            }
            else 
            {
                TowerPriceText[0].text = $"기사 공격업 : {towerPrice[0]}"; 
            }
            if (archerUpgrade >= 5)
            {
                buyButtons[1].interactable = false;
                TowerPriceText[1].text = "최대 강화";
            }
            else
            {
                TowerPriceText[1].text = $"궁수 공격업 : {towerPrice[1]}";
            }
            if (priestUpgrade >= 5)
            {
                buyButtons[2].interactable = false;
                TowerPriceText[2].text = "최대 강화";
            }
            else
            {
                TowerPriceText[2].text = $"사제 체력업 : {towerPrice[2]}";
            }
            if (soldierUpgrade >= 5)
            {
                buyButtons[3].interactable = false;
                TowerPriceText[3].text = "최대 강화";
            }
            else
            {
                TowerPriceText[3].text = $"솔져 공격업 : {towerPrice[3]}";
            }
            if (thiefUpgrade >= 5)
            {
                buyButtons[4].interactable = false;
                TowerPriceText[4].text = "최대 강화";
            }
            else 
            {
                TowerPriceText[4].text = $"도적 공격업 : {towerPrice[4]}";
            }
            if (current >= 5)
            {
                buyButtons[5].interactable = false;
                slotText.text = "최대 슬롯";
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
            int knightUpgrade = TowerSlotSave.GetKnightUpgrade();
            if (knightUpgrade < 5 && gold >= towerPrice[0])
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
                ScoreSave.SaveGold(towerPrice[0]);
                TowerSlotSave.SetKnightUpgrade(knightUpgrade + 1);
                UpdateUI();
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다.";
            }
        }
        public void OnArcherUpgrade()
        {
            StartCoroutine(Notice());
            int arhcherUpgrade = TowerSlotSave.GetArcherUpgrade();
            if (arhcherUpgrade < 5 && gold >= towerPrice[1])
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
                ScoreSave.SaveGold(towerPrice[1]);
                TowerSlotSave.SetArcherUpgrade(arhcherUpgrade + 1);
                UpdateUI();
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnPriestUpgrade()
        {
            StartCoroutine(Notice());
            int priestUpgrade = TowerSlotSave.GetPriestUpgrade();
            if (priestUpgrade < 5 && gold >= towerPrice[2])
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
                ScoreSave.SaveGold(towerPrice[2]);
                TowerSlotSave.SetPriestUpgrade(priestUpgrade + 1);
                UpdateUI();
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnSoldiertUpgrade()
        {
            StartCoroutine(Notice());
            int soldierUpgrade = TowerSlotSave.GetSoldierUpgrade();
            if (soldierUpgrade < 5 && gold >= towerPrice[3])
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
                ScoreSave.SaveGold(towerPrice[3]);
                TowerSlotSave.SetSoldierUpgrade(soldierUpgrade + 1);
                UpdateUI();
            }
            else
            {
                BuyNoticeText.text = $"골드가 부족합니다";
            }
        }
        public void OnThiefUpgrade()
        {
            StartCoroutine(Notice());
            int thiefUpgrade = TowerSlotSave.GetThiefUpgrade();
            if (thiefUpgrade < 5 && gold >= towerPrice[4])
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
                ScoreSave.SaveGold(towerPrice[4]);
                TowerSlotSave.SetThiefUpgrade(thiefUpgrade + 1);
                UpdateUI();
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