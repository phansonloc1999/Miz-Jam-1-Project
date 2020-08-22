using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    private const float MAX_HEALTH = 3;

    [SerializeField]
    private float health = MAX_HEALTH;

    [SerializeField]
    private Map map;

    [SerializeField]
    private List<GameObject> summonedSlaves;

    [SerializeField]
    private List<GameObject> slavePrefabSet;

    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }

    public void takeDamage(float ammount)
    {
        health -= ammount;
    }

    public void moveToTile(GameObject newTile)
    {
        transform.parent = newTile.transform;
        transform.localPosition = Vector3.zero;
    }

    public void summonSlaveAt(int slaveIndexInSet, int atMapRow, int atMapColumn)
    {
        var newSlave = Instantiate(slavePrefabSet[slaveIndexInSet], Vector3.zero, transform.rotation, map.getTile(atMapRow, atMapColumn).transform);
        summonedSlaves.Add(newSlave);
        newSlave.transform.localPosition = Vector3.zero;
    }
}
