﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private List<GameObject> summonedSlaves;

    [SerializeField] private List<CharacterData> slaveSetStats;

    [SerializeField] private GameObject slavePrefab;

    public List<CharacterData> SlaveSetStats
    {
        get { return slaveSetStats; }
        set { slaveSetStats = value; }
    }

    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }

    private void Start()
    {
        if (gameObject.name == "Player 2 Master")
            GetComponent<SpriteRenderer>().material.color = Color.black;
    }

    public void moveToTile(GameObject newTile)
    {
        transform.parent = newTile.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void summonSlaveAt(int slaveIndexInSet, int atMapRow, int atMapColumn)
    {
        var newSlave = Instantiate(slavePrefab, Vector3.zero, transform.rotation, map.getTileAt(atMapRow, atMapColumn).transform);

        newSlave.transform.up = gameObject.transform.up;
        newSlave.transform.localPosition = new Vector3(0, 0, -0.5f);
        newSlave.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var newSlaveScript = newSlave.GetComponent<Slave>();
        newSlaveScript.setMaster(this.gameObject);
        newSlaveScript.loadScriptableData(slaveSetStats[slaveIndexInSet]);

        GameObject.Find("Game Manager").GetComponent<MyGame.GameManager>().OnSummoningSlave(newSlave);

        summonedSlaves.Add(newSlave);
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

    public List<GameObject> getSummonedSlaves()
    {
        return summonedSlaves;
    }
}
