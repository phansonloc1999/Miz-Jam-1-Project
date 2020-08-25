using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlaveAttackStats", menuName = "SlaveAttackStats")]
public class SlaveAttackStats : ScriptableObject
{
    public float ATTACK_DAMAGE;

    public List<AttackRange> attackRanges;
}
