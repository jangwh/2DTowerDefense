using UnityEngine;
using System.Collections.Generic;
using TowerDefense;

namespace TowerDefense
{
    [RequireComponent(typeof(Grid))]
    public class TileGenerator : MonoBehaviour
    {
        Grid grid;
        Transform tileGroup;
        public GameObject tilePrefab;
        //셀마다 1개의 타일을 생성하고, 그 타일 게임 오브젝트 참조를 유지할 Dictionary
        //타일을 회수할 때 멀어진 셀에 놓인 타일을 알아야 회수 할 수 있기 때문
        private Dictionary<Vector3Int, GameObject> tiles = new Dictionary<Vector3Int, GameObject>();
        public int genDist = 5; 
        Vector3Int lastCell;


        void Awake()
        {
            tileGroup = GetComponent<Transform>();
            grid = GetComponent<Grid>();
            SpriteRenderer tileRnerer = tilePrefab.GetComponent<SpriteRenderer>();
            grid.cellSize = tileRnerer.size;
        }
        void Start()
        {
            lastCell = grid.WorldToCell(tileGroup.position); //초기 위치 세팅
            GenerteTiles(); //초기 타일 생성
        }
        //타일 생성
        void GenerteTiles()
        {
            //cell 기준, x축 -5부터 +5까지 반복
            for (int x = -genDist; x <= genDist; x++)
            {
                //cell기준 y축 -5부터 +5까지 반복
                for (int y = -genDist; y <= genDist; y++)
                {
                    Vector3Int cell = new Vector3Int(lastCell.x + x, lastCell.y + y);
                    if (!tiles.ContainsKey(cell))
                    {
                        Vector3 genPos = grid.GetCellCenterWorld(cell);
                        genPos.z = 10; //타일을 살짤 화면 뒤쪽으로 밀어줌
                        GameObject tile = Instantiate(tilePrefab, genPos, Quaternion.identity);
                        tile.transform.SetParent(transform);
                        tiles.Add(cell, tile); //만약 키가 중복이 될 경우 excetion 발생
                        //tiles[cell] = tile; //키가 존재하면 그 값으로 대입, 없을경우 add
                    }
                }
            }
        }
    }
}
