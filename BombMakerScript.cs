using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMakerScript : MonoBehaviour {
    public GameObject bombObj;
    public float spawnTime;

    int mapWidth;
    int mapHeight;

//    float tileUnit;
    int posX;
    int posY;

	// Use this for initialization
	void Start () {
        mapWidth = GameManagerScript.instance.mapWidth;
        mapHeight = GameManagerScript.instance.mapHeight;
//        tileUnit = GameManagerScript.instance.tileUnit;

//        GetSpawnPos();
        StartCoroutine("CoSpawn");
	}
	
    void GetSpawnPos()
    {
        do
        {
            posX = Random.Range(0, mapWidth);
            posY = Random.Range(0, mapHeight);
        } while (GameManagerScript.instance.mapCheck[posX, posY] != 0);
    }

    IEnumerator CoSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            GetSpawnPos();
            GameManagerScript.instance.mapCheck[posX, posY] = 1;
            Instantiate(bombObj, GameManagerScript.instance.getPos(posX, posY), transform.rotation).GetComponent<BombScript>().initPos(posX, posY);
        }
    }

}
