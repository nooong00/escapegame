using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour {
    [Serializable]
    public class Data
    {
        public List<int> charList;
        public List<int> mapList;
        public int coin;
        public Data()
            {
            charList = new List<int>();
            mapList = new List<int>();
            coin = 0;
            }
    }

    public Data playerData;
    string filePath;
    BinaryFormatter formatter;

    public void SaveData()
    {
        FileStream stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public void LoadData()
    {
        if (!System.IO.File.Exists(filePath))
        {
            playerData = new Data();
            playerData.charList.Add(0);
            playerData.mapList.Add(0);
            playerData.coin = 0;
            SaveData();
            return;
        }
        FileStream stream = new FileStream(filePath, FileMode.Open);
        playerData = (Data)formatter.Deserialize(stream);
        stream.Close();
    }

    public void init()
    {
        filePath = Application.dataPath + "/Data/playerdata.bin";
        formatter = new BinaryFormatter();
        LoadData();
    }
}
