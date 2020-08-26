using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Master", menuName = "MasterData", order = 0)]
public class MasterData : ScriptableObject
{
    public Sprite masterSprite;
    public string masterName;
    public int HP;
    public List<SlaveData> lstSlave;

    //Range
}
