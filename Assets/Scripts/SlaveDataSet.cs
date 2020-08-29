using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlaveDataSet", menuName = "SlaveDataSet", order = 0)]
public class SlaveDataSet : ScriptableObject
{
    public List<SlaveData> set;
}
