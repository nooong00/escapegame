using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    //상수
    string[] menus = { "시작", "점수", "상점", "규칙", "종료" };
    string playerDataFilePath;
    string scoreFilePath;
    const int rankingNum = 5;
    float moveY = 55.0f;
    int keyMenuBGM = 0;
    int keyClickSE = 1;

    //프리팹
    GameObject buttonObj;
    GameObject txtObj;


    //게임 내 오브젝트
    Image menuImg;
    GameObject menuTxt;
    GameObject rootBtn;
    GameObject rootTxt;
    GameObject[] rankingScore;
    GameObject menuPanel;
    GameObject panelTxt;
    GameObject panelBackBtn;
    GameObject coinImg;
    GameObject coinTxt;

    BinaryFileScript fileManager;
    ScoreScript scoreObj;
    PlayerData playerData;

    AudioSource bgm;
    AudioSource se;

   
	// Use this for initialization
	void Awake () {
        LoadAssets();
        gameObject.AddComponent<SoundManagerScript>();
        StartCoroutine("CoMenuImg");

    }

    void Start()
    {
        SoundManagerScript.instance.PlayBGM(keyMenuBGM);
    }

    void LoadAssets()
    {
        buttonObj = Resources.Load<GameObject>("Prefabs/UI/Button");
        txtObj = Resources.Load<GameObject>("Prefabs/UI/text");
        

        menuImg = GameObject.Find("menuImg").GetComponent<Image>();
        menuTxt = GameObject.Find("menuImg").transform.FindChild("menuTxt").gameObject;
        rootBtn = GameObject.Find("button");
        rootTxt = GameObject.Find("rankingtxt");
        rankingScore = new GameObject[rankingNum];

        menuPanel = GameObject.Find("Panel");

        panelTxt = menuPanel.transform.FindChild("title").gameObject;
        panelBackBtn = menuPanel.transform.FindChild("backbtn").gameObject;
        menuPanel.SetActive(false);

        coinImg = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CoinImg"), new Vector3(-80, 90, 0), transform.rotation);
        coinImg.transform.SetParent(menuPanel.transform, false);
        coinImg.transform.localScale = Vector3.one;
        coinImg.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        coinImg.SetActive(false);

        coinTxt = Instantiate(Resources.Load<GameObject>("Prefabs/UI/text"), new Vector3(30, 90, 0), transform.rotation);
        coinTxt.transform.SetParent(menuPanel.transform, false);
        coinTxt.transform.localScale = Vector3.one;
        coinTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 45);
        coinTxt.SetActive(false);

        

        fileManager = new BinaryFileScript();

        scoreFilePath = Application.dataPath + "/Data/score.bin";
        scoreObj = new ScoreScript();
        if (!System.IO.File.Exists(scoreFilePath))
        {
            for(int i = 0; i < 5; ++i)
            {
                scoreObj.AddScore(i);
            }
            fileManager.SaveData<List<int>>(scoreObj.scores, scoreFilePath);                   
        }
        else scoreObj.scores = fileManager.LoadData<List<int>>(scoreFilePath);

        playerDataFilePath = Application.dataPath + "/Data/playerdata.bin"; 
        if (!System.IO.File.Exists(playerDataFilePath))
        {
            playerData = new PlayerData();
            playerData.charList.Add(0);
            playerData.mapList.Add(0);
            playerData.coin = 0;
            fileManager.SaveData<PlayerData>(playerData, playerDataFilePath);            
        }
        else playerData = fileManager.LoadData<PlayerData>(playerDataFilePath);

        for (int i = 0; i < rankingNum; ++i)
        {
            rankingScore[i] = Instantiate(txtObj, rootTxt.transform.position + (Vector3.down * moveY * i), transform.rotation);
            rankingScore[i].transform.SetParent(rootTxt.transform, false);
            rankingScore[i].transform.localScale = new Vector3(1, 1, 1);
            rankingScore[i].GetComponent<Text>().text = (i + 1) + ". " + scoreObj.scores[i];
        }

        rootTxt.SetActive(false);

        CreateMenu();
    }

    void CreateMenu()
    {
        for (int i = 0; i < menus.Length; ++i)
        {
            GameObject obj = Instantiate(buttonObj, rootBtn.transform.position + (Vector3.down * moveY * i), transform.rotation);

            obj.transform.SetParent(rootBtn.transform, false);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<BtnScript>().init(i, menus[i]);
        }
    }

    public void PlayMenu(int n)
    {
        SoundManagerScript.instance.PlaySE(keyClickSE);
        if(n != 0 && n != 4)
        {
            menuPanel.SetActive(true);
            panelTxt.SetActive(true);
            panelBackBtn.SetActive(true);
        }

        switch (n)
        {
            case 0://시작
                SceneManager.LoadScene("Test01");
                break;
            case 1://점수
                panelTxt.GetComponent<Text>().text = menus[n];
                rootTxt.SetActive(true);
                break;
            case 2://상점
                panelTxt.GetComponent<Text>().text = menus[n];
                coinImg.SetActive(true);
                coinTxt.SetActive(true);
                coinTxt.GetComponent<Text>().text = playerData.coin.ToString();
                break;
            case 3://규칙
                panelTxt.GetComponent<Text>().text = menus[n];
                break;
            case 4://종료
                Application.Quit();
                break;
        }
    }

    public void CloseMenu()
    {
        SoundManagerScript.instance.PlaySE(keyClickSE);
        menuPanel.SetActive(false);
        panelTxt.SetActive(false);
        panelBackBtn.SetActive(false);
        rootTxt.SetActive(false);
        coinImg.SetActive(false);
        coinTxt.SetActive(false);
    }
	
    IEnumerator CoMenuImg()
    {
        for(int i = 0; i < 50; ++i)
        {
            menuImg.fillAmount += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        menuTxt.SetActive(true);
        for(int i = 0; i < 12; ++i)
        {
            menuTxt.transform.Rotate(0.0f, 0.0f, 60.0f);
            yield return new WaitForSeconds(0.05f);
        }

    }
}
