using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class PlayerData {    
    public List<ItemData> charList;
    public List<ItemData> mapList;
    public int coin;

    public PlayerData()
    {
        charList = new List<ItemData>();
        mapList = new List<ItemData>();
        coin = 0;
    }

}
