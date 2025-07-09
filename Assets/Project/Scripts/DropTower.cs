using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lean.Pool;

namespace TowerDefense {
    public class DropTower : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
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
            GameObject prefab = GetPrefabFromSprite(dropSprite);

            if (dropSprite != null)
            {

                if (receivingImage.overrideSprite != null)
                {
                    // 드롭된 아이템이 없으면 = 판매
                    if (prefab == null)
                    {
                        // 타일 위에 있는 기존 유닛 제거
                        foreach (Transform child in transform)
                        {
                            if (child.GetComponent<Playerable>() != null)
                            {
                                string name = child.GetComponent<Playerable>().charName;
                                LeanPool.Despawn(child.gameObject); // 기존 유닛 제거

                                // 코인 환급
                                switch (name)
                                {
                                    case "Knight":
                                        GameManager.Instance.coin += GameManager.Instance.knightCoin / 2;
                                        break;
                                    case "Archer":
                                        GameManager.Instance.coin += GameManager.Instance.archerCoin / 2;
                                        break;
                                    case "Priest":
                                        GameManager.Instance.coin += GameManager.Instance.priestCoin / 2;
                                        GameManager.priestNum--;
                                        break;
                                }
                                receivingImage.overrideSprite = null;
                                break;
                            }
                        }
                        return;
                    }

                    // 이미 타워가 있고, 또 다른 유닛을 드롭하려 하면 무시
                    return;
                }

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
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                            }
                            break;
                        case "Archer":
                            if (GameManager.Instance.coin >= GameManager.Instance.archerCoin)
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.archerCoin;
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                            }
                            break;
                        case "Priest":
                            if (GameManager.Instance.coin >= GameManager.Instance.priestCoin)
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.priestCoin;
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
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
