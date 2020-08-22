using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private GameObject[,] tiles;

    [SerializeField]
    private GameObject tilePrefab = null;

    [Header("Map size")]
    [SerializeField]
    private int NUM_OF_TILE_ROW = 5;
    [SerializeField]
    private int NUM_OF_TILE_COLUMN = 5;

    [Header("Tile size")]
    [SerializeField]
    private float TILE_WIDTH = 1;
    [SerializeField]
    private float TILE_HEIGHT = 1;

    // Start is called before the first frame update
    void Start()
    {
        instanciatingCells();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void instanciatingCells()
    {
        tiles = new GameObject[NUM_OF_TILE_ROW, NUM_OF_TILE_COLUMN];

        for (int row = 0; row < NUM_OF_TILE_ROW; row++)
        {
            for (int column = 0; column < NUM_OF_TILE_COLUMN; column++)
            {
                tiles[row, column] = Instantiate(tilePrefab,
                    new Vector3(transform.position.x + (column - 1) * TILE_WIDTH, transform.position.y - (row - 1) * TILE_HEIGHT, transform.position.z),
                    transform.rotation, transform
                );
            }
        }
    }
}
