using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense 
{
    public class ScoreSave : MonoBehaviour
    {
        public static int currentScore = 0; 
        public static int currentGold = 0;

        public static void SaveScore()
        {
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (currentScore > bestScore)
            {
                PlayerPrefs.SetInt("BestScore", currentScore);
            }
            PlayerPrefs.Save();
        }

        public static int GetBestScore()
        {
            return PlayerPrefs.GetInt("BestScore", 0);
        }
        
        public static void SaveGold()
        {
            if (currentGold == 0)
            {
                int gold = PlayerPrefs.GetInt("Gold", 0);
            }
            else
            {
                PlayerPrefs.SetInt("Gold", currentGold);
            }
            PlayerPrefs.Save();
        }
        public static int GetGold()
        {
            return PlayerPrefs.GetInt("Gold", 0);
        }
    }
}