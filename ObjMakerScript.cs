using System.Collections;
using UnityEngine;

public class ObjMakerScript : MonoBehaviour {

    public float spawnTime;

    public int mapWidth;
    public int mapHeight;

    public int posX;
    public int posY;

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
        } while (GameManagerScript.instance.mapCheck[posX, posY] != 0 && GameManagerScript.instance.playerCheck(posX, posY));
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            GetSpawnPos();
            GameManagerScript.instance.mapCheck[posX, posY] = 1;
            if(state == MakerState.BOMB)
            {
                GameManagerScript.instance.bombPool.Pop().GetComponent<BombScript>().Play(posX, posY);

            }        
            else if(state == MakerState.COIN)
            {
                GameManagerScript.instance.coinPool.Pop().GetComponent<CoinScript>().Play(posX, posY);
            }   
        }
    }

}

