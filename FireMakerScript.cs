using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMakerScript : MonoBehaviour {
    GameObject fireObj;

    int mapWidth;
    int mapHeight;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        mapWidth = GameManagerScript.instance.mapWidth;
        mapHeight = GameManagerScript.instance.mapHeight;
    }

    public void Play(int x, int y)
    {
        if (x < 0 || x > mapWidth - 1) return;
        if (y < 0 || y > mapHeight - 1) return;
        if(GameManagerScript.instance.mapCheck[x, y] == 1)
        {
            return;
        }//폭탄

        fireObj = GameManagerScript.instance.firePool.Pop();
        fireObj.GetComponent<FireObjScript>().Play(x, y);

    }
	
}
