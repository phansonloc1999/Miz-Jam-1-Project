using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementRange
{
    public int rowOffset;
    public int columnOffset;
}

public class CharacterMovement : MonoBehaviour
{
    static Map map;

    [SerializeField]
    private List<MovementRange> movementRanges;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnAtTile(GameObject newTile)
    {
        transform.parent = newTile.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void moveToTile(GameObject newTile)
    {
        if (isWithinMovementRange(newTile))
        {
            transform.parent = newTile.transform;
            transform.localPosition = Vector3.zero;
        }
    }

    private bool isWithinMovementRange(GameObject newTile)
    {
        var tileContainingThisChar = transform.parent.gameObject;
        var currentTilePosition = map.getTilePositionOf(tileContainingThisChar);
        var newTilePositon = map.getTilePositionOf(newTile);
        foreach (var range in movementRanges)
        {
            if (currentTilePosition.row + range.rowOffset == newTilePositon.row && currentTilePosition.column + range.columnOffset == newTilePositon.column)
                return true;
        }
        return false;
    }
}
