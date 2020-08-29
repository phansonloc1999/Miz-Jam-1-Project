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

    public void flashTargetTilesAndChars(GameObject selectedChar)
    {
        var movementRanges = selectedChar.GetComponent<CharacterTilePositioning>().getMovementRanges();
        var currentSelectedCharTilePos = getPositionOfTile(selectedChar.transform.parent.gameObject);
        var tileScriptsToFlash = new List<Tile>();
        foreach (var range in movementRanges)
        {
            try
            {
                var tile = tiles[currentSelectedCharTilePos.row + range.rowOffset, currentSelectedCharTilePos.column + range.columnOffset];
                if (tile.transform.childCount == 0)
                    tileScriptsToFlash.Add(tile.GetComponent<Tile>());
            }
            catch (SystemException)
            {

            }
        }

        var slaveScript = selectedChar.GetComponent<Slave>();
        if (slaveScript != null)
        {
            var attackRanges = slaveScript.GetAttackRanges();
            if (attackRanges.Count > 0)
                foreach (var range in attackRanges)
                {
                    try
                    {
                        var tile = tiles[currentSelectedCharTilePos.row + range.rowOffset, currentSelectedCharTilePos.column + range.columnOffset];
                        var tileScript = tile.GetComponent<Tile>();

                        // If tile has a child slave or master gameobject and tilescript isn't in flashTileScripts yet
                        if (tile.transform.childCount > 0 && !tileScriptsToFlash.Contains(tileScript))
                            tileScriptsToFlash.Add(tileScript);
                    }
                    catch (System.Exception)
                    {

                    }
                }
        }

        flashTilesCoroutine = flashingTilesAndChars(currentSelectedCharTilePos, movementRanges, tileScriptsToFlash);
        StartCoroutine(flashTilesCoroutine);
    }

    private IEnumerator flashingTilesAndChars(TilePosition currentSelectedCharTilePos, List<MovementRange> movementRanges, List<Tile> flashTileScripts)
    {
        isFlashingTiles = true;
        this.flashTileScripts = flashTileScripts;

        var SECOND_INTERVAL = 1.0f;
        while (isFlashingTiles)
        {
            foreach (var tileScript in this.flashTileScripts)
            {
                tileScript.flash();

                tileScript.gameObject.GetComponentInChildren<CharacterFlashing>()?.Flash();
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

            tileScript.gameObject.GetComponentInChildren<CharacterFlashing>()?.StopFlashing();
        }

        StopCoroutine(flashTilesCoroutine);
    }
}
