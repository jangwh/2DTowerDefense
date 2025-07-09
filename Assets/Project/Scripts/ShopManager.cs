using System.Collections;
using System.Collections.Generic;
using TowerDefense;
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

        public int knightPrice;
        public int archerPrice;
        public int priestPrice;
        int gold;

        void Start()
        {
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
        public void OnKnightUpgrade()
        {
            if (gold >= knightPrice)
            {
                print("기사 강화 완료");
                GameManager.Instance.playerables[0].damage += 5;
                gold -= knightPrice;
            }
            else
            {
                print("기사 강화 돈부족");
            }
        }
        public void OnArcherUpgrade()
        {
            if (gold >= archerPrice)
            {
                print("궁수 강화 완료");
                GameManager.Instance.playerables[1].damage += 5;
                gold -= archerPrice;
            }
            else
            {
                print("궁수 강화 돈부족");
            }
        }
        public void OnPriestUpgrade()
        {
            if (gold >= priestPrice)
            {
                print("사제 강화 완료");
                GameManager.Instance.playerables[2].maxHp += 5;
                gold -= priestPrice;
            }
            else
            {
                print("사제 강화 돈부족");
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
    }
}