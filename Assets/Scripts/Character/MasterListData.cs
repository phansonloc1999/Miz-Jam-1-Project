using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListMaster", menuName = "ListMasterData", order = 0)]
public class MasterListData : ScriptableObject
{
    public List<MasterData> lstMasterData;
}


