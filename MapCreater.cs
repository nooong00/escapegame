using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour {
    public GameObject[] tiles;
    public int mapWidth;
    public int mapHeight;
    float tileUnit = 0.7f;
    public Vector3 startPos;
    int tileNum;
    string tilePath;
    void Start()
    {
        tileNum = 0;
//        CreateMap();
    }

    public void init()
    {
        tilePath = "Prefabs/Tile/";
        mapWidth = GameData.instance.mapDataList[GameData.instance.selectedMap].width;

        mapHeight = GameData.instance.mapDataList[GameData.instance.selectedMap].height;

        tiles = Resources.LoadAll<GameObject>(tilePath + GameData.instance.selectedMap);

        SetBG();
        CreateMap();
    }

    void SetBG()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/BG"));
        obj.GetComponent<SpriteRenderer>().sprite = GameData.instance.mapSprite[GameData.instance.selectedMap];
    }

    void CreateMap()
    {
        Vector3 pos = startPos;
        for(int i = 0; i < mapHeight; ++i)
        {
            for(int j = 0; j < mapWidth; ++j)
            {
                
                Instantiate(tiles[tileNum], pos, transform.rotation);
                pos += (Vector3.right * tileUnit);
                tileNum++;
                if (tileNum > tiles.Length - 1) tileNum = 0;
            }
            pos.Set(startPos.x, pos.y, pos.z);           
            pos += (Vector3.up * tileUnit);
        }
    }
}
