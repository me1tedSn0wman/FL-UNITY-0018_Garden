using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public int width;
    public int height;

    public Tile[] groundTiles;
    public Tile[] wallTiles;

    private CellData[,] m_BoardData;

    public void Init() {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        m_BoardData = new CellData[width, height];

        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {

                Tile tile;
                m_BoardData[i,j] = new CellData();

                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    int ind = Random.Range(0, wallTiles.Length);
                    tile = wallTiles[ind];
                }
                else 
                {
                    int ind = Random.Range(0, groundTiles.Length);
                    tile = groundTiles[ind];
                }

                m_Tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }


    }

    public Vector3 CellToWorld(Vector2Int cellIndex) {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex) {
        if (cellIndex.x < 0 || cellIndex.x >= width || cellIndex.y<0|| cellIndex.y>=height)
            return null;
        return m_BoardData[cellIndex.x, cellIndex.y];
    }
}
