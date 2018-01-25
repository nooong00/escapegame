using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    MAP,
    CHARACTER,
}

public class GameData : MonoBehaviour {
    public static GameData instance;

    public Dictionary<int, MapData> mapDataList;
    public Dictionary<int, CharData> charDataList;

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
    string scoreFilePath;

    BinaryFileScript fileManager;

    public ScoreScript scoreData;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
      
        if (instance == null)
        {
            instance = this;

            mapDataPath = Application.streamingAssetsPath + "/mapData.bin";
            charDataPath = Application.streamingAssetsPath + "/charData.bin";
//          mapDataPath = "jar:file://" + Application.dataPath + @"!/assets/mapData.bin";
//          charDataPath = "jar:file://" + Application.dataPath + @"!/assets/charData.bin";

            playerDataFilePath = Application.persistentDataPath + "/playerdata.bin";

            fileManager = new BinaryFileScript();

            DontDestroyOnLoad(this);

            LoadData();
        }        
    }

	void LoadData()
    {
        //   mapDataList = fileManager.WWWLoadData<List<MapData>>(mapDataPath);
        //    charDataList = fileManager.WWWLoadData<List<CharData>>(charDataPath);

        mapDataList = fileManager.LoadData<Dictionary<int, MapData>>(mapDataPath);
        charDataList = fileManager.LoadData<Dictionary<int, CharData>>(charDataPath);

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



        if (!System.IO.File.Exists(playerDataFilePath))
        {
            playerData.charList.Add(1000);
            playerData.mapList.Add(2000);
            playerData.coin = 0;
            fileManager.SaveData<PlayerData>(playerData, playerDataFilePath);
        }
        else
        {
            playerData = fileManager.LoadData<PlayerData>(playerDataFilePath);
        }
        int t = 0;
        foreach(KeyValuePair<int, CharData> tmp in charDataList)
        {
            tmp.Value.path = t;
            tmp.Value.having = false;
            t++;
        }
        t = 0;
        foreach (KeyValuePair<int, MapData> tmp in mapDataList)
        {
            tmp.Value.path = t;
            tmp.Value.having = false;
            t++;
        }

        for (int i = 0; i < playerData.charList.Count; ++i)
        {
            if(charDataList.ContainsKey(playerData.charList[i]))
            {
                charDataList[playerData.charList[i]].having = true;
            }
        }

        for (int i = 0; i < playerData.mapList.Count; ++i)
        {
            if(mapDataList.ContainsKey(playerData.mapList[i]))
            {
                mapDataList[playerData.mapList[i]].having = true;
            }
        }

        scoreData = new ScoreScript();

        scoreFilePath = Application.persistentDataPath + "/score.bin";
        if (!System.IO.File.Exists(scoreFilePath))
        {
            for(int i = 0; i < 5; ++i)
            {
                scoreData.AddScore(i);
            }
            fileManager.SaveData<List<int>>(scoreData.scores, scoreFilePath);
        }
        else scoreData.scores = fileManager.LoadData<List<int>>(scoreFilePath);


    }


    public void SavePlayerData()
    {
        fileManager.SaveData<PlayerData>(playerData, playerDataFilePath);
    }

}
