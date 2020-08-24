using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementRanges", menuName = "MovementRanges", order = 0)]
public class MovementRanges : ScriptableObject
{
    public List<MovementRange> ranges;
}