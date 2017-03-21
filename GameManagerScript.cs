using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
    public static GameManagerScript instance;
    public float tileUnit;
    public int mapWidth;
    public int mapHeight;
    public int[,] mapCheck; //0 = 비어있음, 1 = 폭탄 생성중, 2 = 폭탄, 4 = 플레이어
    public Vector3 startPos;

    public GameObject FireObj;
    public GameObject lifeObj;
    GameObject rootlife;
    Image[] lifeImg;
    Text scoreTxt;
    float moveX = 40.0f;
    int score;

    void Awake()
    {
        if(!instance)
        {
            instance = this;
            if(!instance)
            {
                Debug.Log("GameManager Error");
            }
        }

        startPos = new Vector3(0 - (tileUnit * (mapWidth / 2)), 0 - (tileUnit * (mapHeight / 2)), 0.0f);

        mapCheck = new int[mapWidth, mapHeight];
        for(int i = 0; i < mapHeight; ++i)
        {
            for(int j = 0; j < mapWidth; ++j)
            {
                mapCheck[i, j] = 0;
            }
        }
        rootlife = GameObject.Find("lifePos");
        lifeImg = new Image[3];
        for(int i= 0; i < 3; ++i)
        {
            GameObject obj = Instantiate(lifeObj, rootlife.transform.position + (Vector3.right * moveX * i), transform.rotation);
            obj.transform.SetParent(rootlife.transform, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            lifeImg[i] = obj.GetComponent<Image>();
        }
        scoreTxt = GameObject.Find("scoreTxt").GetComponent<Text>();
        score = 0;
        StartCoroutine("CoGetScore");
    }
	
    public Vector3 getPos(int x, int y)
    {
        return new Vector3(startPos.x + (x * tileUnit), startPos.y + (y * tileUnit), startPos.z);
    }
    IEnumerator CoGetScore()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            score++;
            scoreTxt.text = score.ToString();
        }
    }
}
