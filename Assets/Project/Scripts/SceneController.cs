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
            ScoreSave.currentScore = 0;
            SceneManager.LoadScene(1);
        }
        public void OnGameQuit()
        {
            Application.Quit();
        }
        public void OnGameRetry()
        {
            ScoreSave.SaveScore();
            SceneManager.LoadScene(0);
        }
    }
}
