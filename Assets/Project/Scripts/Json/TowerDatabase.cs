using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense
{
    public class TowerDatabase : MonoBehaviour
    {
        public List<TowerData> towers;

        void Awake()
        {
            string overrideJson = PlayerPrefs.GetString("TowerDataOverride", "");

            if (!string.IsNullOrEmpty(overrideJson))
            {
                towers = JsonUtility.FromJson<TowerDataListWrapper>(overrideJson).towers;
                Debug.Log("커스텀 업그레이드 데이터 적용");
            }
            else
            {
                TextAsset json = Resources.Load<TextAsset>("TowerDataList");
                towers = JsonUtility.FromJson<TowerDataListWrapper>("{\"towers\":" + json.text + "}").towers;
                Debug.Log(" 기본 데이터 로드");
            }
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