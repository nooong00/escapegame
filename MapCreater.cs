using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour {
    public List<GameObject> tiles;
    public int mapWidth;
    public int mapHeight;
    float tileUnit = 0.7f;
    public Vector3 startPos;
    int tileNum;
    void Start()
    {
        //        tileUnit = tiles[0].GetComponent<SpriteRenderer>().bounds.size.x;
        //        Debug.Log(tileUnit);
        tileNum = 0;
        CreateMap();
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
                if (tileNum > tiles.Count - 1) tileNum = 0;
            }
            pos.Set(startPos.x, pos.y, pos.z);           
            pos += (Vector3.up * tileUnit);
        }
    }
}
