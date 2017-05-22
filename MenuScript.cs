using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BUTTONSTATE
{
    READY,
    START,
    SCORE,
    SHOP,
    RULE,
    EXIT,
    MAPUP,
    MAPDOWN,
    CHARUP,
    CHARDOWN,
};


public class MenuScript : MonoBehaviour {
    //상수
    string[] menus = { "시작", "점수", "상점", "종료" };
    
    string scoreFilePath;
    const int rankingNum = 5;
    float moveY = 200.0f;
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

    GameObject shopListObj;
    GameObject startBtn;

    GameObject[] changeBtn;
    GameObject mapImg;
    GameObject charImg;


    int mapNum;
    int charNum;

	// Use this for initialization
	void Awake () {
        Screen.SetResolution(720, 1280, true);

        mapNum = 0;
        charNum = 0;

        LoadAssets();


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
        menuTxt = GameObject.Find("menuTxt");
        rootBtn = GameObject.Find("button");
        rootTxt = GameObject.Find("rankingtxt");
        rankingScore = new GameObject[rankingNum];

        menuPanel = GameObject.Find("Panel");

        Instantiate(Resources.Load<GameObject>("Prefabs/BG"));

        panelTxt = menuPanel.transform.FindChild("title").gameObject;
        panelBackBtn = menuPanel.transform.FindChild("backbtn").gameObject;
        menuPanel.SetActive(false);

        coinImg = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CoinImg"), new Vector3(-300, 380, 0), transform.rotation);
        coinImg.transform.SetParent(menuPanel.transform, false);
        coinImg.transform.localScale = Vector3.one;
        coinImg.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        coinImg.SetActive(false);

        coinTxt = Instantiate(Resources.Load<GameObject>("Prefabs/UI/text"), new Vector3(50, 380, 0), transform.rotation);
        coinTxt.transform.SetParent(menuPanel.transform, false);
        coinTxt.transform.localScale = Vector3.one;
        coinTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 150);
        coinTxt.GetComponent<Text>().fontSize = 100;
        coinTxt.SetActive(false);

        fileManager = new BinaryFileScript();

        scoreFilePath = Application.persistentDataPath + "/score.bin";
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

        for (int i = 0; i < rankingNum; ++i)
        {
            rankingScore[i] = Instantiate(txtObj, rootTxt.transform.position + (Vector3.down * moveY * i), transform.rotation);
            rankingScore[i].transform.SetParent(rootTxt.transform, false);
            rankingScore[i].transform.localScale = new Vector3(1, 1, 1);
            rankingScore[i].GetComponent<Text>().text = (i + 1) + ". " + scoreObj.scores[i];
        }

        rootTxt.SetActive(false);

        CreateMenu();
        gameObject.AddComponent<ShopScript>();
        shopListObj = GameObject.Find("ViewImg");
        shopListObj.SetActive(false);

        startBtn = Instantiate<GameObject>(buttonObj, new Vector3(200, -550, 0), transform.rotation);
        startBtn.transform.SetParent(menuPanel.transform, false);
        startBtn.GetComponent<Button>().onClick.AddListener(() => ClickButton(BUTTONSTATE.START));
        startBtn.transform.FindChild("Text").GetComponent<Text>().text = menus[0];

        changeBtn = new GameObject[4];
        changeBtn[0] = Instantiate<GameObject>(GameData.instance.prefabBtn);
        changeBtn[0].transform.SetParent(menuPanel.transform, false);
        changeBtn[0].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        changeBtn[0].GetComponent<RectTransform>().localPosition = new Vector3(-250, 250, 0);
        changeBtn[0].GetComponent<Image>().sprite = GameData.instance.upBtnSprite;
        changeBtn[0].GetComponent<Button>().onClick.AddListener(() => ClickButton(BUTTONSTATE.MAPUP));

        changeBtn[1] = Instantiate<GameObject>(GameData.instance.prefabBtn);
        changeBtn[1].transform.SetParent(menuPanel.transform, false);
        changeBtn[1].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        changeBtn[1].GetComponent<RectTransform>().localPosition = new Vector3(-250, -300, 0);
        changeBtn[1].GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
        changeBtn[1].GetComponent<Image>().sprite = GameData.instance.upBtnSprite;
        changeBtn[1].GetComponent<Button>().onClick.AddListener(() => ClickButton(BUTTONSTATE.MAPDOWN));

        changeBtn[2] = Instantiate<GameObject>(GameData.instance.prefabBtn);
        changeBtn[2].transform.SetParent(menuPanel.transform, false);
        changeBtn[2].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        changeBtn[2].GetComponent<RectTransform>().localPosition = new Vector3(250, 250, 0);
        changeBtn[2].GetComponent<Image>().sprite = GameData.instance.upBtnSprite;
        changeBtn[2].GetComponent<Button>().onClick.AddListener(() => ClickButton(BUTTONSTATE.CHARUP));

        changeBtn[3] = Instantiate<GameObject>(GameData.instance.prefabBtn);
        changeBtn[3].transform.SetParent(menuPanel.transform, false);
        changeBtn[3].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        changeBtn[3].GetComponent<RectTransform>().localPosition = new Vector3(250, -300, 0);
        changeBtn[3].GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
        changeBtn[3].GetComponent<Image>().sprite = GameData.instance.upBtnSprite;
        changeBtn[3].GetComponent<Button>().onClick.AddListener(() => ClickButton(BUTTONSTATE.CHARDOWN));

        mapImg = Instantiate<GameObject>(GameData.instance.prefabUIImg);
        mapImg.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 350);
        mapImg.GetComponent<RectTransform>().localPosition = new Vector3(-250, 0, 0);
        mapImg.GetComponent<Image>().sprite = GameData.instance.mapSprite[mapNum];
        mapImg.transform.SetParent(menuPanel.transform, false);

        charImg = Instantiate<GameObject>(GameData.instance.prefabUIImg);
        charImg.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 350);
        charImg.GetComponent<RectTransform>().localPosition = new Vector3(250, 0, 0);
        charImg.GetComponent<Image>().sprite = GameData.instance.charSprite[charNum];
        charImg.transform.SetParent(menuPanel.transform, false);
        EnableStartMenu(false);
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

    public void viewCoin()
    {
        coinTxt.GetComponent<Text>().text = GameData.instance.playerData.coin.ToString();
    }

    public void ClickButton(BUTTONSTATE s)
    {
        switch(s)
        {
            case BUTTONSTATE.START:
                EnableStartMenu(false);
                GameData.instance.selectedChar = GameData.instance.playerData.charList[charNum].path;
                GameData.instance.selectedMap = GameData.instance.playerData.mapList[mapNum].path;

                SceneManager.LoadScene("Test01");
                break;
            case BUTTONSTATE.MAPUP:
                mapNum++;
                if (mapNum > GameData.instance.playerData.mapList.Count - 1) mapNum = 0;
                mapImg.GetComponent<Image>().sprite = GameData.instance.mapSprite[GameData.instance.playerData.mapList[mapNum].path];
                break;
            case BUTTONSTATE.MAPDOWN:
                mapNum--;
                if (mapNum < 0) mapNum = GameData.instance.playerData.mapList.Count - 1;
                mapImg.GetComponent<Image>().sprite = GameData.instance.mapSprite[GameData.instance.playerData.mapList[mapNum].path];
                break;
            case BUTTONSTATE.CHARUP:
                charNum++;
                if (charNum > GameData.instance.playerData.charList.Count - 1) charNum = 0;
                charImg.GetComponent<Image>().sprite = GameData.instance.charSprite[GameData.instance.playerData.charList[charNum].path];
                break;
            case BUTTONSTATE.CHARDOWN:                
                charNum--;
                if (charNum < 0) charNum = GameData.instance.playerData.charList.Count - 1;
                charImg.GetComponent<Image>().sprite = GameData.instance.charSprite[GameData.instance.playerData.charList[charNum].path];
                break;
        }
    }

    public void PlayMenu(int n)
    {
        SoundManagerScript.instance.PlaySE(keyClickSE);
        if(n != 4)
        {
            menuPanel.SetActive(true);
            panelTxt.SetActive(true);
            panelBackBtn.SetActive(true);
            menuTxt.SetActive(false);
        }

        switch (n)
        {
            case 0://시작
                panelTxt.GetComponent<Text>().text = menus[n];
                EnableStartMenu(true);
                break;
            case 1://점수
                panelTxt.GetComponent<Text>().text = menus[n];
                rootTxt.SetActive(true);
                break;
            case 2://상점
                panelTxt.GetComponent<Text>().text = menus[n];
                coinImg.SetActive(true);
                coinTxt.SetActive(true);
                viewCoin();
                shopListObj.SetActive(true);
                GetComponent<ShopScript>().ActiveObjs(true);                
                break;
            case 3://종료
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
        shopListObj.SetActive(false);
        GetComponent<ShopScript>().ClosedShop();
        menuTxt.SetActive(true);
        EnableStartMenu(false);

    }

    void EnableStartMenu(bool enabled)
    {
        for(int i = 0; i < changeBtn.Length; ++i)
        {
            changeBtn[i].SetActive(enabled);
        }
        charImg.SetActive(enabled);
        mapImg.SetActive(enabled);
        startBtn.SetActive(enabled);
        if(enabled)
        {
            charNum = 0;
            mapNum = 0;
            charImg.GetComponent<Image>().sprite = GameData.instance.charSprite[charNum];
            mapImg.GetComponent<Image>().sprite = GameData.instance.mapSprite[mapNum];
        }
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
