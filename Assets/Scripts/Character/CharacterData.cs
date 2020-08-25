using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData", order = 0)]
public class CharacterData : ScriptableObject
{
    public Sprite sprite;

    public float MAX_HEALTH;

    public float ATTACK_DAMAGE;
    public List<AttackRange> attackRanges;

    public List<MovementRange> movementRanges;
}
