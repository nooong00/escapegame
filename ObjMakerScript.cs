using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMakerScript : MonoBehaviour {

    public GameObject obj;
    public float spawnTime;

    public int mapWidth;
    public int mapHeight;

    public int posX;
    public int posY;

    GameObject tmp;

    MakerState state;

    public void SetState(MakerState s)
    {
        state = s;
    }

    public void Init()
    {
        mapWidth = GameManagerScript.instance.mapWidth;
        mapHeight = GameManagerScript.instance.mapHeight;
    }
    
    public void SetObject(GameObject mObj)
    {
        obj = mObj;
    }

    public void SetTime(float t)
    {
        spawnTime = t;
    }

    public void StartSpawn()
    {
        StartCoroutine("CoSpawn");
    }

    public void StopSpawn()
    {
        StopCoroutine("CoSpawn");
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
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            GetSpawnPos();
            GameManagerScript.instance.mapCheck[posX, posY] = 1;
            tmp = Instantiate(obj, GameManagerScript.instance.getPos(posX, posY), transform.rotation); 
            if(state == MakerState.BOMB)
            {
                tmp.GetComponent<BombScript>().initPos(posX, posY);
            }        
            else if(state == MakerState.COIN)
            {
                tmp.GetComponent<CoinScript>().SetTimer(5.0f);
                tmp.GetComponent<CoinScript>().Play();
            }   
        }
    }

}

