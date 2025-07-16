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
        public Transform towerParent;
        public GameObject[] tilePrefab;
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
            lastCell = grid.WorldToCell(tileGroup.position);
            GenerteTiles();
            GenerteEndTiles();
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
                            genPos.z = 10;
                            GameObject tile = Instantiate(tilePrefab[0], genPos, Quaternion.identity);
                            tile.transform.SetParent(transform);
                            tiles.Add(cell, tile);
                        }
                        else
                        {
                            Vector3 genPos = grid.GetCellCenterWorld(cell);
                            genPos.z = 10;
                            GameObject tile = Instantiate(tilePrefab[1], genPos, Quaternion.identity);
                            tile.transform.SetParent(transform);
                            tiles.Add(cell, tile); 
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
                genPos.z = 10; 
                GameObject tile = Instantiate(tilePrefab[2], genPos, Quaternion.identity);
                tile.transform.SetParent(transform);
                tiles.Add(cell, tile); 
            }
        }
    }
}
