using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense
{
    public static class TowerSlotSave
    {
        private const string SaveKey = "MaxTowerSlot";
        private const string SaveKnight = "MaxKnightUpgrade";
        private const string SaveArcher = "MaxArcherUpgrade";
        private const string SavePriest = "MaxPriestUpgrade";
        private const string SaveSoldier = "MaxSoldierUpgrade";
        private const string SaveThief = "MaxThiefUpgrade";

        public static int GetMaxSlot()
        {
            return PlayerPrefs.GetInt(SaveKey, 3);
        }

        public static void SetMaxSlot(int count)
        {
            PlayerPrefs.SetInt(SaveKey, count);
            PlayerPrefs.Save();
        }
        public static int GetKnightUpgrade()
        {
            return PlayerPrefs.GetInt(SaveKnight, 0);
        }
        public static void SetKnightUpgrade(int count)
        {
            PlayerPrefs.SetInt(SaveKnight, count);
            PlayerPrefs.Save();
        }
        public static int GetArcherUpgrade()
        {
            return PlayerPrefs.GetInt(SaveArcher, 0);
        }
        public static void SetArcherUpgrade(int count)
        {
            PlayerPrefs.SetInt(SaveArcher, count);
            PlayerPrefs.Save();
        }
        public static int GetPriestUpgrade()
        {
            return PlayerPrefs.GetInt(SavePriest, 0);
        }
        public static void SetPriestUpgrade(int count)
        {
            PlayerPrefs.SetInt(SavePriest, count);
            PlayerPrefs.Save();
        }
        public static int GetSoldierUpgrade()
        {
            return PlayerPrefs.GetInt(SaveSoldier, 0);
        }
        public static void SetSoldierUpgrade(int count)
        {
            PlayerPrefs.SetInt(SaveSoldier, count);
            PlayerPrefs.Save();
        }
        public static int GetThiefUpgrade()
        {
            return PlayerPrefs.GetInt(SaveThief, 0);
        }
        public static void SetThiefUpgrade(int count)
        {
            PlayerPrefs.SetInt(SaveThief, count);
            PlayerPrefs.Save();
        }
    }
}