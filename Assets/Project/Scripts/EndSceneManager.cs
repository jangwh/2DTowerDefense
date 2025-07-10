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
        public AudioClip[] audioClips;
        AudioSource audioSource;
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        void Start()
        {
            if (GameManager.Instance.isWin)
            {
                audioSource.PlayOneShot(audioClips[0]);
                GameOver.SetActive(false);
                GameWin.SetActive(true);
            }
            else if (GameManager.Instance.isDefeat)
            {
                audioSource.PlayOneShot(audioClips[1]);
                GameWin.SetActive(false);
                GameOver.SetActive(true);
            }
            scoreWinText.text = $"점수 : {ScoreSave.currentScore.ToString()}";
            scoreDefeatText.text = $"점수 : {ScoreSave.currentScore.ToString()}";
        }
    }
}
