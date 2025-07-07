using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lean.Pool;

namespace TowerDefense {
    public class DropTower : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        //TODO : 판매기능 구현하기
        public Image containerImage;
        public Image receivingImage;
        public RectTransform rect;
        public Transform tileGroup;
        private Color normalColor;
        public Color highlightColor = Color.yellow;
        public List<SpriteToPrefab> spritePrefabMappings;
        public void OnEnable()
        {
            if (containerImage != null)
                normalColor = new Color(0, 0, 0, 0);
        }
        public void OnDrop(PointerEventData eventData)
        {
            containerImage.color = normalColor;

            if (receivingImage == null)
                return;

            Sprite dropSprite = GetDropSprite(eventData);
            if (dropSprite != null)
            {
                GameObject prefab = GetPrefabFromSprite(dropSprite);

                if (receivingImage.overrideSprite != null)
                    return; // 이미 배치된 경우 무시

                
                if (prefab != null)
                {
                    Vector3 spawnPosition = tileGroup.transform.position + GetWorldPositon(rect); // 드롭된 위치

                    Mathf.Abs(Camera.main.transform.position.z); 

                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(spawnPosition);
                    worldPos.z = 0;
                    switch (prefab.GetComponent<Playerable>().charName)
                    {
                        case "Knight":
                            if (GameManager.Instance.coin >= GameManager.Instance.knightCoin)
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.knightCoin;
                                LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), transform);
                            }
                            break;
                        case "Archer":
                            if (GameManager.Instance.coin >= GameManager.Instance.archerCoin)
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.archerCoin;
                                LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), transform);
                            }
                            break;
                        case "Priest":
                            if (GameManager.Instance.coin >= GameManager.Instance.priestCoin)
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.priestCoin;
                                LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), transform);
                                GameManager.priestNum++;
                            }
                            break;
                    }
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
        private Vector3 GetWorldPositon(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners); //RectTransform의 꼭지점 좌표를 World 좌표로 가져옴

            Vector3 center = Vector3.zero;
            foreach( Vector3 corner in corners)
                center += corner;

            center /= 4f;
            return center;
        }
    }
    [System.Serializable]
    public class SpriteToPrefab
    {
        public Sprite sprite;
        public GameObject prefab;
    }
}
