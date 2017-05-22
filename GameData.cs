using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    MAP,
    CHARACTER,
}

public class GameData : MonoBehaviour {
    public static GameData instance;

    public List<MapData> mapDataList;
    public List<CharData> charDataList;

    public List<ItemData> itemList;

    public GameObject prefabBtn;
    public GameObject prefabUIImg;


    public Sprite[] mapSprite;
    public Sprite[] charSprite;


    public Sprite upBtnSprite;

    public PlayerData playerData;

    public int selectedMap;
    public int selectedChar;

    string mapDataPath;
    string charDataPath;
    string playerDataFilePath;

    BinaryFileScript fileManager;

    public void init()
    {
      
        if (instance == null)
        {
            instance = this;

  //          mapDataPath = Application.streamingAssetsPath + "/mapData.bin";
    //        charDataPath = Application.streamingAssetsPath + "/charData.bin";
          mapDataPath = "jar:file://" + Application.dataPath + @"!/assets/mapData.bin";
          charDataPath = "jar:file://" + Application.dataPath + @"!/assets/charData.bin";

            playerDataFilePath = Application.persistentDataPath + "/playerdata.bin";

            fileManager = new BinaryFileScript();
            itemList = new List<ItemData>();

            LoadData();
        }        
    }

	void LoadData()
    {
        mapDataList = fileManager.WWWLoadData<List<MapData>>(mapDataPath);
       charDataList = fileManager.WWWLoadData<List<CharData>>(charDataPath);

   //     mapDataList = fileManager.LoadData<List<MapData>>(mapDataPath);
    //    charDataList = fileManager.LoadData<List<CharData>>(charDataPath);

        prefabBtn = Resources.Load<GameObject>("Prefabs/UI/ShopBtn");
        prefabUIImg = Resources.Load<GameObject>("Prefabs/UI/UIImg");

        mapSprite = Resources.LoadAll<Sprite>("Images/Map");

        charSprite = new Sprite[charDataList.Count];
        Sprite[] charImg01 = Resources.LoadAll<Sprite>("Images/Char/char001");
        Sprite[] charImg02 = Resources.LoadAll<Sprite>("Images/Char/char002");
        charSprite[0] = charImg01[1];
        charSprite[1] = charImg02[4];

        upBtnSprite = Resources.Load<Sprite>("Images/UI/upBtnImg");

        playerData = new PlayerData();

        ItemData t;
        if (!System.IO.File.Exists(playerDataFilePath))
        {
            t = new ItemData(charDataList[0].key, 0, charDataList[0].pay, ItemType.CHARACTER);
            playerData.charList.Add(t);
            t = new ItemData(mapDataList[0].key, 0, mapDataList[0].pay, ItemType.MAP);
            playerData.mapList.Add(t);
            playerData.coin = 0;
            fileManager.SaveData<PlayerData>(playerData, playerDataFilePath);
        }
        else
        {
            playerData = fileManager.LoadData<PlayerData>(playerDataFilePath);
        }
  
        
        for(int i = 1; i < mapDataList.Count; ++i)
        {           
            t = new ItemData(mapDataList[i].key, i, mapDataList[i].pay, ItemType.MAP);
            for(int j = 0; j < playerData.mapList.Count; ++j)
            {
                if(playerData.mapList[j].key == t.key)
                {
                    t.having = true;
                    break;
                }
            }
            itemList.Add(t);            
        }
        for(int i = 1; i < charDataList.Count; ++i)
        {
            t = new ItemData(charDataList[i].key, i, charDataList[i].pay, ItemType.CHARACTER);
            for(int j = 0; j < playerData.charList.Count; ++j)
            {
                if(playerData.charList[j].key == t.key)
                {
                    t.having = true;
                    break;
                }
            }
            itemList.Add(t);
        }

    }


    public void SavePlayerData()
    {
        fileManager.SaveData<PlayerData>(playerData, playerDataFilePath);
    }

}
