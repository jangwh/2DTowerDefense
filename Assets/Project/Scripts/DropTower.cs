using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lean.Pool;

namespace TowerDefense {
    public class DropTower : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        //TODO : 드랍한 위치는 canvas지만 world좌표로 나오게 수정
        public Image containerImage;
        public Image receivingImage;
        public Transform tileGroup;
        private Color normalColor;
        public Color highlightColor = Color.yellow;
        public List<SpriteToPrefab> spritePrefabMappings;
        public void OnEnable()
        {
            if (containerImage != null)
                normalColor = containerImage.color;
        }
        public void OnDrop(PointerEventData eventData)
        {
            containerImage.color = normalColor;

            if (receivingImage == null)
                return;

            Sprite dropSprite = GetDropSprite(eventData);
            if (dropSprite != null)
            {
                if (receivingImage.overrideSprite != null)
                    return; // 이미 배치된 경우 무시
                receivingImage.overrideSprite = dropSprite;

                GameObject prefab = GetPrefabFromSprite(dropSprite);
                if (prefab != null)
                {
                    Vector3 spawnPosition = transform.position; // 드롭된 위치
                    spawnPosition.z = 0;
                    Instantiate(prefab, spawnPosition, Quaternion.identity, tileGroup);
                    containerImage.color = new Color(0, 0, 0, 0);
                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (containerImage == null)
                return;
            Sprite dropSprite = GetDropSprite(eventData);
            if (dropSprite != null)
                containerImage.color = highlightColor;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (containerImage == null)
                return;
            containerImage.color = new Color(0, 0, 0, 0); ;
        }
        private Sprite GetDropSprite(PointerEventData eventData)
        {
            var originalObj = eventData.pointerDrag;
            if (originalObj == null)
                return null;

            var dragMe = originalObj.GetComponent<DragMe>();
            if (dragMe == null)
                return null;

            var srcImage = originalObj.GetComponent<Image>();
            if (srcImage == null)
                return null;

            return srcImage.sprite;
        }
        private GameObject GetPrefabFromSprite(Sprite sprite)
        {
            foreach (var mapping in spritePrefabMappings)
            {
                if (mapping.sprite == sprite)
                    return mapping.prefab;
            }
            return null;
        }
    }
    [System.Serializable]
    public class SpriteToPrefab
    {
        public Sprite sprite;
        public GameObject prefab;
    }
}
