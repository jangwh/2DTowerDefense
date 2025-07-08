using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Round;
    public Text LifeCount;
    public Text Coin;

    public Text KnightText;
    public Text ArcherText;
    public Text PriestText;
    public Text CellText;
    public Text ScoreText;

    void Update()
    {
        Round.text = $"Round {GameManager.Instance.roundCount + 1}/{GameManager.Instance.enemyCount.Length}";
        LifeCount.text = $"Life {GameManager.Instance.currentLifeCount}/{GameManager.Instance.MaxLifeCount}";
        Coin.text = $"코인 : {GameManager.Instance.coin}/ 골드 : {GameManager.Instance.gold}";

        KnightText.text = $"Knight : {GameManager.Instance.knightCoin}";
        ArcherText.text = $"Archer : {GameManager.Instance.archerCoin}";
        PriestText.text = $"Priest : {GameManager.Instance.priestCoin}";
        CellText.text = $"판매";
        ScoreText.text = $"Score : {GameManager.Instance.score}";
    }
}
