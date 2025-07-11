using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerSelector : MonoBehaviour
{
    public List<string> selectedTowerIds = new List<string>();

    public void SelectTower(string id)
    {
        if (selectedTowerIds.Contains(id))
        {
            selectedTowerIds.Remove(id);
        }
        else if (selectedTowerIds.Count < 3)
        {
            selectedTowerIds.Add(id);
        }
    }

    public void StartGame()
    {
        string json = JsonUtility.ToJson(new SelectedTowerWrapper { selected = selectedTowerIds });
        PlayerPrefs.SetString("SelectedTowers", json);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    [System.Serializable]
    public class SelectedTowerWrapper
    {
        public List<string> selected;
    }
}