using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense
{
    public static class TowerSlotSave
    {
        private const string SaveKey = "MaxTowerSlot";

        // 슬롯 수 불러오기 (기본값 3)
        public static int GetMaxSlot()
        {
            return PlayerPrefs.GetInt(SaveKey, 3);
        }

        // 슬롯 수 저장
        public static void SetMaxSlot(int count)
        {
            PlayerPrefs.SetInt(SaveKey, count);
            PlayerPrefs.Save();
        }
    }
}