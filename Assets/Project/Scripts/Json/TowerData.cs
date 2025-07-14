using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense
{
    [System.Serializable]
    public class TowerData
    {
        public string id;
        public string towerName;
        public int cost;
        public float MaxHp;
        public int damage;
        public float AttackInterval;
        public string prefabPath; // Resources 폴더에서 불러오기 위함
        public string spritePath;
    }
}
