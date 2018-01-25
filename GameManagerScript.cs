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
    public int[,] mapCheck; //0 null, 1 obj


    //prefab
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
    private FireMakerScript fireMaker;
    
    private BinaryFileScript fileManager;

    string scoreFilePath;


    public Stack<GameObject> bombPool;
    public Stack<GameObject> firePool;
    public Stack<GameObject> coinPool;

    PlayerControlScript player;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        Screen.SetResolution(720, 1280, true);

        GetComponent<MapCreater>().init();
        LoadAssets();

        rootLife = GameObject.Find("lifePos");
        scoreTxt = GameObject.Find("scoreTxt").GetComponent<Text>();
        coinTxt = GameObject.Find("coinTxt").GetComponent<Text>();

        mapWidth = GameData.instance.mapDataList[GameData.instance.playerData.mapList[GameData.instance.selectedMap]].width;
        mapHeight = GameData.instance.mapDataList[GameData.instance.playerData.mapList[GameData.instance.selectedMap]].height;

        coinMaker = gameObject.AddComponent<ObjMakerScript>();
        bombMaker = gameObject.AddComponent<ObjMakerScript>();
        fireMaker = gameObject.AddComponent<FireMakerScript>();
     
        scoreFilePath = Application.persistentDataPath + "/score.bin";
        SetChar();

        bombPool = new Stack<GameObject>();
        GameObject tmpObj;
        for(int i = 0; i < 10; ++i)
        {
            tmpObj = Instantiate<GameObject>(bombObj);
            bombPool.Push(tmpObj);
        }

        coinPool = new Stack<GameObject>();

        for (int i = 0; i < 10; ++i)
        {
            tmpObj = Instantiate<GameObject>(coinObj);
            tmpObj.GetComponent<CoinScript>().SetTimer(5.0f);
            coinPool.Push(tmpObj);
            
        }

        firePool = new Stack<GameObject>();

        for (int i = 0; i < 40; ++i)
        {
            tmpObj = Instantiate<GameObject>(fireObj);
            tmpObj.GetComponent<FireObjScript>().SetTimer(2.0f);
            tmpObj.SetActive(false);
            firePool.Push(tmpObj);
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();
    }

    public bool playerCheck(int x, int y)
    {
        
        return (x == player.GetX() && y == player.GetY());
    }

    void LoadAssets()
    {
        fireObj = Resources.Load<GameObject>("Prefabs/Fire");
        lifeObj = Resources.Load<GameObject>("Prefabs/UI/lifeImg");
        coinObj = Resources.Load<GameObject>("Prefabs/coin");
        bombObj = Resources.Load<GameObject>("Prefabs/bomb");
    }

    void SetChar()
    {       
        GameObject charObj = Instantiate(Resources.Load<GameObject>("Prefabs/player"), getPos(0, 0), transform.rotation);
        charObj.transform.Find("char").GetComponent<Animator>().runtimeAnimatorController = Resources.LoadAll<RuntimeAnimatorController>("Animator")[GameData.instance.selectedChar];
    }

    public void MakeFire(int x, int y)
    {
        fireMaker.Play(x, y);
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

        score = 0;
        scoreTxt.text = score.ToString();
        moveX = 120.0f;

        coin = 0;
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
        coinMaker.SetTime(3.0f);
        coinMaker.SetState(MakerState.COIN);

        bombMaker.Init();
        bombMaker.SetTime(1.0f);
        bombMaker.SetState(MakerState.BOMB);

             
        setLifeImg(GameData.instance.charDataList[GameData.instance.playerData.charList[GameData.instance.selectedChar]].life);
        StartCoroutine("CoCount");
        SoundManagerScript.instance.PlayBGM(GameData.instance.selectedMap + 1);
    }

    IEnumerator CoCount()
    {
        yield return new WaitForSeconds(0.5f);
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

        GameData.instance.scoreData.AddScore(score);
        fileManager.SaveData<List<int>>(GameData.instance.scoreData.scores, scoreFilePath);
        GameData.instance.playerData.coin += coin;
        GameData.instance.SavePlayerData();

        DestroyTag("Bomb");
        DestroyTag("DamagedObj");
        DestroyTag("Item");
    
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
        instance = null;
        SceneManager.LoadScene("MenuScene");
    }

    public void GetCoin()
    {
        coin += 20;
        coinTxt.text = coin.ToString();
    }

    public Vector3 getPos(int x, int y)
    {
        return new Vector3(startPos.x + (x * tileUnit), startPos.y + (y * tileUnit), startPos.z - 1);
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
