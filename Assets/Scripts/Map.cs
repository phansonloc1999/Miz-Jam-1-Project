using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TilePosition
{
    public int row;
    public int column;

    public TilePosition(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
}

public class Map : MonoBehaviour
{
    private GameObject[,] tiles;

    [SerializeField] private GameObject tilePrefab = null;

    [Header("Map size")]
    public int NUM_OF_TILE_ROW = 5;
    public int NUM_OF_TILE_COLUMN = 5;

    [Header("Tile size")]
    public float TILE_WIDTH = 1;
    public float TILE_HEIGHT = 1;

    public void initTiles()
    {
        tiles = new GameObject[NUM_OF_TILE_ROW, NUM_OF_TILE_COLUMN];

        for (int row = 0; row < NUM_OF_TILE_ROW; row++)
        {
            for (int column = 0; column < NUM_OF_TILE_COLUMN; column++)
            {
                tiles[row, column] = Instantiate(tilePrefab,
                    new Vector3(transform.position.x + (column - 1) * TILE_WIDTH, transform.position.y - (row - 1) * TILE_HEIGHT, transform.position.z),
                    Quaternion.identity, transform
                );
            }
        }
    }

    public GameObject getTileAt(int row, int column)
    {
        return tiles[row, column];
    }

    public TilePosition getPositionOfTile(GameObject targetTile)
    {
        for (int row = 0; row < NUM_OF_TILE_ROW; row++)
        {
            for (int column = 0; column < NUM_OF_TILE_COLUMN; column++)
            {
                if (targetTile == tiles[row, column]) return new TilePosition(row, column);
            }
        }
        return new TilePosition(-1, -1);
    }
}
