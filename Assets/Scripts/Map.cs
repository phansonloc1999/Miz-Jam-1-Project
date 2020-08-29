using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
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

    [SerializeField] private bool isFlashingTiles = false;
    private List<Tile> flashTileScripts;

    private IEnumerator flashTilesCoroutine;

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

    public void flashPossibleMoveTiles(GameObject selectedChar)
    {
        var movementRanges = selectedChar.GetComponent<CharacterTilePositioning>().getMovementRanges();
        var currentSelectedCharTilePos = getPositionOfTile(selectedChar.transform.parent.gameObject);
        var flashTileScripts = new List<Tile>();
        foreach (var range in movementRanges)
        {
            try
            {
                var tile = tiles[currentSelectedCharTilePos.row + range.rowOffset, currentSelectedCharTilePos.column + range.columnOffset];
                if (tile.transform.childCount == 0)
                    flashTileScripts.Add(tile.GetComponent<Tile>());
            }
            catch (SystemException)
            {

            }
        }

        flashTilesCoroutine = flashingTiles(currentSelectedCharTilePos, movementRanges, flashTileScripts);
        StartCoroutine(flashTilesCoroutine);
    }

    private IEnumerator flashingTiles(TilePosition currentSelectedCharTilePos, List<MovementRange> movementRanges, List<Tile> flashTileScripts)
    {
        isFlashingTiles = true;
        this.flashTileScripts = flashTileScripts;

        var SECOND_INTERVAL = 1.0f;
        while (isFlashingTiles)
        {
            foreach (var tileScript in this.flashTileScripts)
            {
                tileScript.flash();
            }
            yield return new WaitForSeconds(SECOND_INTERVAL);
        }
    }

    public void stopFlashingTiles()
    {
        isFlashingTiles = false;

        foreach (var tileScript in flashTileScripts)
        {
            tileScript.stopFlashing();
        }

        StopCoroutine(flashTilesCoroutine);
    }
}
