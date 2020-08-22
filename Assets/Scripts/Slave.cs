using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slave : MonoBehaviour
{
    [SerializeField]
    private float MAX_HEALTH;

    [SerializeField]
    private float ATTACK_DAMAGE;

    [SerializeField]
    private float ATTACK_RANGE;

    [SerializeField]
    private float health;

    private void Update()
    {
        transform.localPosition = Vector3.zero;
    }
}
