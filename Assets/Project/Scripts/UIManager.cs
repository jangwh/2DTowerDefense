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

    void Update()
    {
        Round.text = $"Round {GameManager.Instance.roundCount + 1}/{GameManager.Instance.enemyCount.Length}";
        LifeCount.text = $"Life {GameManager.Instance.currentLifeCount}/{GameManager.Instance.MaxLifeCount}";
        Coin.text = $"{GameManager.Instance.coin}";
    }
}
