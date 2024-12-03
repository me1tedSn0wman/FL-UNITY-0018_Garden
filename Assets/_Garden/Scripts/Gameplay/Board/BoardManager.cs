using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 Create a map, background for gameplay
 */
public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public int width;
    public int height;

    public int spawnWidth;
    public int spawnHeight;

    public RuleTile[] groundTiles;
    public RuleTile[] wallTiles;

    private CellData[,] m_BoardData;

    private List<Vector2Int> offScreenCells;

    public void Init() {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        m_BoardData = new CellData[width, height];

        offScreenCells = new List<Vector2Int>();

        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {

                RuleTile tile;
                m_BoardData[i,j] = new CellData();

                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    int ind = Random.Range(0, wallTiles.Length);
                    tile = wallTiles[ind];
                    offScreenCells.Add(new Vector2Int(i, j));
                }
                else if (i == 1 || i == width - 2 || j == 1 || j == height - 2)
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

    public Vector3 CenterOfGrid() {
        float centerHor = width * m_Grid.cellSize.x/2;
        float centerVer = height * m_Grid.cellSize.y/2;

        return new Vector3(centerHor, centerVer, 0);
    }

    public Vector3 GetRandomSpawnPos() {
        float posHor = Random.Range(-m_Grid.cellSize.x * spawnWidth, m_Grid.cellSize.x * spawnWidth);
        float posVert = Random.Range(-m_Grid.cellSize.y * spawnHeight, m_Grid.cellSize.y * spawnHeight);


        return CenterOfGrid() + new Vector3(posHor, posVert);
    }

    public Vector3 GetRandomOffScreenSpawnPos() {
        int randCell = Random.Range(0, offScreenCells.Count);

        Vector3 pos = m_Grid.GetCellCenterWorld((Vector3Int)offScreenCells[randCell]);

        return new Vector3(pos.x, pos.y,0);

    }
}
