using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense
{
    public class TowerDatabase : MonoBehaviour
    {
        public List<TowerData> towers;

        void Awake()
        {
            TextAsset json = Resources.Load<TextAsset>("TowerDataList");
            towers = JsonUtility.FromJson<TowerDataListWrapper>("{\"towers\":" + json.text + "}").towers;
        }

        [System.Serializable]
        public class TowerDataListWrapper
        {
            public List<TowerData> towers;
        }

        public TowerData FindById(string id)
        {
            return towers.Find(t => t.id == id);
        }
    }
}