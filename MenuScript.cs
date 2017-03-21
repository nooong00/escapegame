using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    public Image menuImg;
    public GameObject menuTxt;

    public GameObject buttonObj;
    public GameObject txtObj;
    GameObject rootBtn;
    GameObject rootTxt;
    string[] menus = {"시작", "점수", "상점", "규칙", "종료" };
    float moveY = 55.0f;
    public GameObject[] rankingScore;

  //  const string scoreFilePath = "/Data/ranking.bin";
    const int rankingNum = 5;

   public GameObject menuPanel;
   public GameObject panelTxt;
   public GameObject panelBackBtn;

    public ScoreScript scoreObj;
    public PlayerData playerData;

	// Use this for initialization
	void Awake () {
        menuImg = GameObject.Find("menuImg").GetComponent<Image>();
        menuTxt = GameObject.Find("menuImg").transform.FindChild("menuTxt").gameObject;
        rootBtn = GameObject.Find("button");
        rootTxt = GameObject.Find("rankingtxt");
        rankingScore = new GameObject[rankingNum];

        menuPanel = GameObject.Find("Panel");
        panelTxt = menuPanel.transform.FindChild("title").gameObject;
        panelBackBtn = menuPanel.transform.FindChild("backbtn").gameObject;
        menuPanel.SetActive(false);

        scoreObj = GetComponent<ScoreScript>();
        scoreObj.InitScore(rankingNum);
        playerData = GetComponent<PlayerData>();
        playerData.init();

        for (int i = 0; i < rankingNum; ++i)
        {
            rankingScore[i] = Instantiate(txtObj, rootTxt.transform.position + (Vector3.down * moveY * i), transform.rotation);
            rankingScore[i].transform.SetParent(rootTxt.transform, false);
            rankingScore[i].transform.localScale = new Vector3(1, 1, 1);            
            rankingScore[i].GetComponent<Text>().text = (i + 1) + ". " + scoreObj.scores[i];
        }

        rootTxt.SetActive(false);

        StartCoroutine("CoMenuImg");
        Vector3 pos = rootBtn.transform.position;
        for(int i = 0; i < 5; ++i)
        {
            //pos + (Vector3.down * moveY)
            GameObject obj = Instantiate(buttonObj, pos + (Vector3.down * moveY * i), transform.rotation);
            
            obj.transform.SetParent(rootBtn.transform, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<BtnScript>().init(i, menus[i]);
        }

	}

    public void PlayMenu(int n)
    {
        if(n != 0 && n != 4)
        {
            menuPanel.SetActive(true);
            panelTxt.SetActive(true);
            panelBackBtn.SetActive(true);
        }

        switch (n)
        {
            case 0://시작
                
                break;
            case 1://점수
                panelTxt.GetComponent<Text>().text = menus[n];
                rootTxt.SetActive(true);
                break;
            case 2://상점
                panelTxt.GetComponent<Text>().text = menus[n];
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
        menuPanel.SetActive(false);
        panelTxt.SetActive(false);
        panelBackBtn.SetActive(false);
        rootTxt.SetActive(false);
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
