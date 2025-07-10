using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UIManager : MonoBehaviour
    {
        public Text Round;
        public Text LifeCount;
        public Text Coin;

        public Text[] TowerText;

        public Text CellText;
        public Text ScoreText;
        void Awake()
        {
            TowerText[0].text = $"Knight : {GameManager.Instance.towerCoin[0]}";
            TowerText[1].text = $"Archer : {GameManager.Instance.towerCoin[1]}";
            TowerText[2].text = $"Priest : {GameManager.Instance.towerCoin[2]}";
            CellText.text = $"판매";
        }
        void Update()
        {
            Round.text = $"Round {GameManager.Instance.roundCount + 1}/{GameManager.Instance.enemyCount.Length}";
            LifeCount.text = $"Life {GameManager.Instance.currentLifeCount}/{GameManager.Instance.MaxLifeCount}";
            Coin.text = $"코인 : {GameManager.Instance.coin}/ 골드 : {ScoreSave.currentGold}";
            ScoreText.text = $"Score : {ScoreSave.currentScore.ToString()}";
        }
    }
}