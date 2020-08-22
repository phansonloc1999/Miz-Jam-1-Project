using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    private const float MAX_HEALTH = 5;

    [SerializeField]
    private float health = MAX_HEALTH;

    private GameObject map;

    // Update is called once per frame
    void Update()
    {
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
}
