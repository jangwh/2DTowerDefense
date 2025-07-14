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
            Sprite dropSprite = GetDropSprite(eventData);
            GameObject prefab = GetPrefabFromSprite(dropSprite);

            Debug.Log($"▶ 드래그된 스프라이트 이름: {dropSprite?.name}");
            Debug.Log($"▶ 매핑된 프리팹: {prefab?.name}");

            if (dropSprite != null)
            {

                if (receivingImage.overrideSprite != null)
                {
                    // 드롭된 아이템이 없으면 = 판매
                    if (prefab == null)
                    {
                        // 타일 위에 있는 기존 유닛 제거
                        foreach (Transform child in tileGroup.GetComponent<TileGenerator>().towerParent)
                        {
                            Playerable player = child.GetComponent<Playerable>();
                            if (player != null && player.dropTower == this)
                            {
                                string name = player.charName;

                                // 코인 환급
                                switch (name)
                                {
                                    case "knight":
                                        GameManager.Instance.coin += GameManager.Instance.towerCoin[0] / 2;
                                        break;
                                    case "archer":
                                        GameManager.Instance.coin += GameManager.Instance.towerCoin[1] / 2;
                                        break;
                                    case "priest":
                                        GameManager.Instance.coin += GameManager.Instance.towerCoin[2] / 2;
                                        GameManager.priestNum--;
                                        break;
                                    case "soldier":
                                        GameManager.Instance.coin += GameManager.Instance.towerCoin[3] / 2;
                                        break;
                                    case "thief":
                                        GameManager.Instance.coin += GameManager.Instance.towerCoin[4] / 2;
                                        break;
                                }
                                player.dropTower.receivingImage.overrideSprite = null;
                                LeanPool.Despawn(child.gameObject); // 기존 유닛 제거
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
                        case "knight":
                            if (GameManager.Instance.coin >= GameManager.Instance.towerCoin[0])
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.towerCoin[0];
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this); 
                                TowerData data = GameManager.Instance.towerDatabase.FindById(player.charName.ToLower());  // 예: "knight"
                                player.InitTower(data);
                            }
                            break;
                        case "archer":
                            if (GameManager.Instance.coin >= GameManager.Instance.towerCoin[1])
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.towerCoin[1];
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                                TowerData data = GameManager.Instance.towerDatabase.FindById(player.charName.ToLower());  // 예: "knight"
                                player.InitTower(data);
                            }
                            break;
                        case "priest":
                            if (GameManager.Instance.coin >= GameManager.Instance.towerCoin[2])
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.towerCoin[2];
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                                GameManager.priestNum++;
                                TowerData data = GameManager.Instance.towerDatabase.FindById(player.charName.ToLower());  // 예: "knight"
                                player.InitTower(data);
                            }
                            break;
                        case "soldier":
                            if (GameManager.Instance.coin >= GameManager.Instance.towerCoin[3])
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.towerCoin[3];
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                                TowerData data = GameManager.Instance.towerDatabase.FindById(player.charName.ToLower());  // 예: "knight"
                                player.InitTower(data);
                            }
                            break;
                        case "thief":
                            if (GameManager.Instance.coin >= GameManager.Instance.towerCoin[4])
                            {
                                receivingImage.overrideSprite = dropSprite;
                                GameManager.Instance.coin -= GameManager.Instance.towerCoin[4];
                                GameObject playerObj = LeanPool.Spawn(prefab, worldPos, Quaternion.Euler(0, 180, 0), tileGroup.transform.GetComponent<TileGenerator>().towerParent);
                                Playerable player = playerObj.GetComponent<Playerable>();
                                player.Init(this);
                                TowerData data = GameManager.Instance.towerDatabase.FindById(player.charName.ToLower());  // 예: "knight"
                                player.InitTower(data);
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
            if (sprite == null)
            {
                Debug.LogWarning("Sprite가 null입니다.");
                return null;
            }

            foreach (var mapping in spritePrefabMappings)
            {
                if (mapping.sprite != null && mapping.prefab != null)
                {
                    // 🎯 이름으로 비교
                    if (mapping.sprite.name == sprite.name)
                    {
                        Debug.Log($"✔ 매칭 성공: {sprite.name} → {mapping.prefab.name}");
                        return mapping.prefab;
                    }
                }
            }

            Debug.LogWarning($"⛔ Sprite({sprite.name})에 해당하는 프리팹을 못 찾음");
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
