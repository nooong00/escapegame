using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class PlayerData {    
    public List<int> charList;
    public List<int> mapList;
    public int coin;

    public PlayerData()
    {
        charList = new List<int>();
        mapList = new List<int>();
        coin = 0;
    }
}
