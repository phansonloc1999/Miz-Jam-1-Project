using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private GameObject[,] cells;

    [SerializeField]
    private GameObject cellPrefab = null;

    [Header("Board size")]
    [SerializeField]
    private int NUM_OF_ROW = 0;
    [SerializeField]
    private int NUM_OF_COLUMN = 0;

    [Header("Cell size")]
    [SerializeField]
    private float CELL_WIDTH = 0;
    [SerializeField]
    private float CELL_HEIGHT = 0;

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
        cells = new GameObject[NUM_OF_ROW, NUM_OF_COLUMN];

        for (int row = 0; row < NUM_OF_ROW; row++)
        {
            for (int column = 0; column < NUM_OF_COLUMN; column++)
            {
                cells[row, column] = Instantiate(cellPrefab,
                    new Vector3(transform.position.x + (column - 1) * CELL_WIDTH, transform.position.y - (row - 1) * CELL_HEIGHT, transform.position.z),
                    transform.rotation, transform
                );
            }
        }
    }
}
