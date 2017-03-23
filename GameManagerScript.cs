using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MakerState
{
    BOMB,
    COIN,
};

public class GameManagerScript : MonoBehaviour {
    const int rankNum = 5;

    public static GameManagerScript instance;

    public int mapWidth;
    public int mapHeight;
    public float tileUnit;
    public int[,] mapCheck; //0 아무것도 없음, 1 오브젝트 2 이동불가 오브젝트

    public GameObject coinObj;
    public GameObject fireObj;
    public GameObject bombObj;

    private Vector3 startPos;
    private GameObject rootLife;
    private GameObject lifeObj;
    private GameObject readyTime;


    private Image[] lifeImg;
    private Text scoreTxt;
    private Text coinTxt;

    private float moveX;
    private int score;
    private int coin;
    private int maxLife;

    private ObjMakerScript coinMaker;
    private ObjMakerScript bombMaker;
    private ScoreScript scoreData;
    private PlayerData playerData;

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

        LoadAssets();

        rootLife = GameObject.Find("lifePos");
        scoreTxt = GameObject.Find("scoreTxt").GetComponent<Text>();
        coinTxt = GameObject.Find("coinTxt").GetComponent<Text>();
        coinMaker = gameObject.AddComponent<ObjMakerScript>();
        bombMaker = gameObject.AddComponent<ObjMakerScript>();
        scoreData = gameObject.AddComponent<ScoreScript>();   
        playerData = gameObject.AddComponent<PlayerData>();

    }

    void LoadAssets()
    {
        fireObj = Resources.Load<GameObject>("Prefabs/Fire");
        lifeObj = Resources.Load<GameObject>("Prefabs/UI/lifeImg");
        coinObj = Resources.Load<GameObject>("Prefabs/coin");
        bombObj = Resources.Load<GameObject>("Prefabs/bomb");
    }

    void InitGameScene()
    {
        startPos = new Vector3(0 - (tileUnit * (mapWidth / 2)), 0 - (tileUnit * (mapHeight / 2)), 0.0f);

        mapCheck = new int[mapWidth, mapHeight];
        for (int i = 0; i < mapHeight; ++i)
        {
            for (int j = 0; j < mapWidth; ++j)
            {
                mapCheck[i, j] = 0;
            }
        }

        readyTime = GameObject.Find("ReadyTime");

        scoreData.InitScore();
        playerData.init();
        score = 0;
        scoreTxt.text = score.ToString();
      
    }

    void setLifeImg(int n)
    {
        lifeImg = new Image[n];
        for (int i = 0; i < n; ++i)
        {
            GameObject obj = Instantiate(lifeObj, rootLife.transform.position + (Vector3.right * moveX * i), transform.rotation);
            obj.transform.SetParent(rootLife.transform, false);
            obj.transform.localScale = Vector3.one;            
            lifeImg[i] = obj.GetComponent<Image>();
        }
    }

    void Start()
    {
        InitGameScene();
        coinMaker.Init();
        coinMaker.SetObject(coinObj);
        coinMaker.SetTime(3.0f);
        coinMaker.SetState(MakerState.COIN);

        bombMaker.Init();
        bombMaker.SetObject(bombObj);
        bombMaker.SetTime(1.0f);
        bombMaker.SetState(MakerState.BOMB);

             
        setLifeImg(3);
        StartCoroutine("CoCount");
        SoundManagerScript.instance.PlayBGM(1);
    }

    IEnumerator CoCount()
    {
        yield return new WaitForSeconds(1.0f);
        for(int i = 3; i > 0; --i)
        {
            readyTime.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        readyTime.SetActive(false);
        coinMaker.StartSpawn();
        bombMaker.StartSpawn();
        StartCoroutine("CoGetScore");
    }
	
    public void LostLife(int n)
    {
        
        lifeImg[n].enabled = false;
    }

    public void GameOver()
    {
        coinMaker.StopSpawn();
        bombMaker.StopSpawn();
        StopCoroutine("CoGetScore");
        //폭탄 생성도 취소...
        
        scoreData.AddScore(score);
        playerData.playerData.coin += coin;
        playerData.SaveData();

        DestroyTag("Bomb");
        DestroyTag("DamagedObj");
        DestroyTag("Item");
        //임시
        StartCoroutine("CoChangeScene");
    }

    void DestroyTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        for(int i = 0; i < objs.Length; ++i)
        {
            Destroy(objs[i]);
        }
    }

    IEnumerator CoChangeScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MenuScene");
    }

    public void GetCoin()
    {
        coin++;
        coinTxt.text = coin.ToString();
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
