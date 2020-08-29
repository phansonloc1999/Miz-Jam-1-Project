using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackRange
{
    public int rowOffset;
    public int columnOffset;
}

public class Slave : MonoBehaviour
{
    [SerializeField] private SlaveData data;

    [SerializeField] private GameObject master;

    [SerializeField] private Map map;

    private void Start()
    {
        if (master.name == "Player 2 Master")
            GetComponent<SpriteRenderer>().material.color = Color.black;

        map = GameObject.Find("Map").GetComponent<Map>();
    }

    public float getAttackDamage()
    {
        return data.ATTACK_DAMAGE;
    }

    public delegate void SelectCharacterHandler(GameObject selectedChar);
    public event SelectCharacterHandler selectedChar;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(MOUSE_BUTTON.PRIMARY))
        {
            selectedChar?.Invoke(this.gameObject);
        }
    }

    public void setMaster(GameObject player)
    {
        master = player;
    }

    public GameObject getMaster()
    {
        return master;
    }

    public bool canAttackAtTile(GameObject targetTile)
    {
        var tileContainingThisChar = transform.parent.gameObject;
        var currentTilePosition = map.getPositionOfTile(tileContainingThisChar);
        var newTilePositon = map.getPositionOfTile(targetTile);
        foreach (var range in data.attackRanges)
        {
            if (currentTilePosition.row + range.rowOffset == newTilePositon.row && currentTilePosition.column + range.columnOffset == newTilePositon.column)
                return true;
        }
        return false;
    }

    public void loadScriptableData(SlaveData data)
    {
        this.data = data;

        GetComponent<SpriteRenderer>().sprite = data.sprite;
        GetComponent<CharacterTilePositioning>().loadMovementRanges(data.movementRanges);
        GetComponent<Health>().loadMaxHealth(data.MAX_HEALTH);
    }


    public List<AttackRange> GetAttackRanges()
    {
        return data.attackRanges;
    }
}
