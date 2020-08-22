using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    private const float MAX_HEALTH = 3;

    [SerializeField] private float health = MAX_HEALTH;

    [SerializeField] private Map map;

    [SerializeField] private List<GameObject> summonedSlaves;

    [SerializeField] private List<GameObject> slavePrefabSet;

    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }

    public void takeDamage(float ammount)
    {
        health -= ammount;
    }

    public void summonSlaveAt(int slaveIndexInSet, int atMapRow, int atMapColumn)
    {
        var newSlave = Instantiate(slavePrefabSet[slaveIndexInSet], Vector3.zero, transform.rotation, map.getTileAt(atMapRow, atMapColumn).transform);
        summonedSlaves.Add(newSlave);
        newSlave.transform.localPosition = Vector3.zero;
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
}
