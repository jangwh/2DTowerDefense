using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TowerDefense
{
    public class TowerSelector : MonoBehaviour
    {
        public List<string> selectedTowerIds = new List<string>();

        public void SelectTower(string id)
        {
            int maxSlot = TowerSlotSave.GetMaxSlot();

            if (selectedTowerIds.Contains(id))
            {
                selectedTowerIds.Remove(id);
            }
            else if (selectedTowerIds.Count < maxSlot)
            {
                selectedTowerIds.Add(id);
            }
        }

        public void StartGame()
        {
            SelectedTowerWrapper wrapper = new SelectedTowerWrapper
            {
                selected = selectedTowerIds
            };

            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString("SelectedTowers", json);
            PlayerPrefs.Save();

            SceneManager.LoadScene("2DTDPlay");
        }
    }
}