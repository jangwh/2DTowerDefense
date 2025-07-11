using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerInventory
{
    public List<string> ownedTowerIds = new List<string>();
    private static string SaveKey = "OwnedTowers";

    public void AddTower(string id)
    {
        if (!ownedTowerIds.Contains(id))
        {
            ownedTowerIds.Add(id);
            SaveToJson();
        }
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadFromJson()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            TowerInventory loaded = JsonUtility.FromJson<TowerInventory>(json);
            ownedTowerIds = loaded.ownedTowerIds;
        }
    }
}