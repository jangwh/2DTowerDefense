using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{ 
public class SceneController : MonoBehaviour
{
        public void OnGameStart()
        {
            SceneManager.LoadScene(1);
        }
        public void OnGameQuit()
        {
            Application.Quit();
        }
        public void OnGameRetry()
        {
            ScoreSave.SaveScore();
            ScoreSave.SaveGold(0);
            SceneManager.LoadScene(0);
        }
    }
}
