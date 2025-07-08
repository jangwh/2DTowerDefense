using Lean.Pool;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(Grid))]
    public class TileGenerator : MonoBehaviour
    {
        Grid grid;
        Transform tileGroup;
        public GameObject[] tilePrefab;
        //셀마다 1개의 타일을 생성하고, 그 타일 게임 오브젝트 참조를 유지할 Dictionary
        private Dictionary<Vector3Int, GameObject> tiles = new Dictionary<Vector3Int, GameObject>();
        public int genDistx = 5;
        public int genDisty = 5;
        Vector3Int lastCell;


        void Awake()
        {
            tileGroup = GetComponent<Transform>();
            grid = GetComponent<Grid>();
            SpriteRenderer tileRenderer = tilePrefab[0].GetComponent<SpriteRenderer>();
            grid.cellSize = tileRenderer.size; 

            SpriteRenderer endTileRenderer = tilePrefab[1].GetComponent<SpriteRenderer>();
            grid.cellSize = endTileRenderer.size;
        }
        void Start()
        {
            lastCell = grid.WorldToCell(tileGroup.position); //초기 위치 세팅
            GenerteTiles(); //초기 타일 생성
            GenerteEndTiles(); //라이프 타일 생성
        }
        //타일 생성
        void GenerteTiles()
        {
            for (int x = -genDistx; x <= genDistx; x++)
            {
                for (int y = -genDisty; y <= genDisty; y++)
                {
                    Vector3Int cell = new Vector3Int(lastCell.x + x, lastCell.y + y);
                    if (!tiles.ContainsKey(cell))
                    {
                        if (x != genDistx)
                        {
                            Vector3 genPos = grid.GetCellCenterWorld(cell);
                            genPos.z = 10; //타일을 살짤 화면 뒤쪽으로 밀어줌
                            GameObject tile = Instantiate(tilePrefab[0], genPos, Quaternion.identity);
                            tile.transform.SetParent(transform);
                            tiles.Add(cell, tile); //만약 키가 중복이 될 경우 excetion 발생 }
                        }
                        else
                        {
                            Vector3 genPos = grid.GetCellCenterWorld(cell);
                            genPos.z = 10; //타일을 살짤 화면 뒤쪽으로 밀어줌
                            GameObject tile = Instantiate(tilePrefab[1], genPos, Quaternion.identity);
                            tile.transform.SetParent(transform);
                            tiles.Add(cell, tile); //만약 키가 중복이 될 경우 excetion 발생 }
                        }
                    }
                }
            }
        }
        void GenerteEndTiles()
        {
            int x = -genDistx - 1;
            for(int y = -genDisty;y <= genDisty; y++)
            {
                Vector3Int cell = new Vector3Int(lastCell.x + x, lastCell.y + y);
                Vector3 genPos = grid.GetCellCenterWorld(cell);
                genPos.z = 10; //타일을 살짤 화면 뒤쪽으로 밀어줌
                GameObject tile = Instantiate(tilePrefab[2], genPos, Quaternion.identity);
                tile.transform.SetParent(transform);
                tiles.Add(cell, tile); //만약 키가 중복이 될 경우 excetion 발생 }
            }
        }
    }
}
