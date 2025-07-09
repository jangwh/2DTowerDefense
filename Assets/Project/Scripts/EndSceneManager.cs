using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;
namespace TowerDefense
{
    public class EneSceneManager : MonoBehaviour
    {
        public GameObject GameWin;
        public GameObject GameOver;
        public Text scoreWinText;
        public Text scoreDefeatText;

        void Start()
        {
            if (GameManager.Instance.isWin)
            {
                GameOver.SetActive(false);
                GameWin.SetActive(true);
            }
            else if (GameManager.Instance.isDefeat)
            {
                GameWin.SetActive(false);
                GameOver.SetActive(true);
            }
            scoreWinText.text = $"점수 : {ScoreSave.currentScore.ToString()}";
            scoreDefeatText.text = $"점수 : {ScoreSave.currentScore.ToString()}";
        }
    }
}
