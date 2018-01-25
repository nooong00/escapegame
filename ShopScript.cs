using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SHOPBUTTONSTATE
{
    PREV, 
    NEXT,
    BUY,
}

public class ShopScript : MonoBehaviour {

    GameObject rootContent;
    GameObject itemPrefab;
    GameObject selectIconPrefab;
    GameObject shopBtnPrefab;
    GameObject textPrefab;

    GameObject selectedIconObj;
    GameObject prevBtn;
    GameObject nextBtn;
    GameObject buyBtn;
    GameObject priceObj;

    int targetNum;
    float moveSpeed;

    Sprite nextSprite;
    Sprite buySprite;

    string strPrice = "가격\n";
    Vector3 firstPos;

    List<int> itemObjList;
    List<GameObject> objList;
    // Use this for initialization
    void Awake () {
        itemObjList = new List<int>();
        objList = new List<GameObject>();

        rootContent = GameObject.Find("Content");
        itemPrefab = Resources.Load<GameObject>("Prefabs/UI/ItemImg");
        selectIconPrefab = Resources.Load<GameObject>("Prefabs/UI/SelectedIcon");
        shopBtnPrefab = Resources.Load<GameObject>("Prefabs/UI/ShopBtn");
        textPrefab = Resources.Load<GameObject>("Prefabs/UI/text");

        nextSprite = Resources.Load<Sprite>("Images/UI/nextIcon");
        buySprite = Resources.Load<Sprite>("Images/UI/buyIcon");

        targetNum = 0;
        moveSpeed = 7.0f;
        
        MakeBtn();
        selectedIconObj = Instantiate<GameObject>(selectIconPrefab);
        selectedIconObj.transform.position += Vector3.up * 200.0f;
        selectedIconObj.transform.SetParent(rootContent.transform, false);
        selectedIconObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);

        LoadList();

        rootContent.GetComponent<RectTransform>().sizeDelta = new Vector2(200 + ((GameData.instance.mapDataList.Count + GameData.instance.charDataList.Count) * 600), rootContent.GetComponent<RectTransform>().rect.height);
        firstPos = rootContent.transform.position;

        ActiveObjs(false);
    }

    //데이터 읽어옴
    void LoadList()
    {
        rootContent.transform.position = firstPos;

        
        foreach (KeyValuePair<int, CharData> tmp in GameData.instance.charDataList)
        {
            itemObjList.Add(tmp.Key);
        }

        foreach (KeyValuePair<int, MapData> tmp in GameData.instance.mapDataList)
        {
            itemObjList.Add(tmp.Key);
        }

        GameObject obj;

        for (int i = 0; i < itemObjList.Count; ++i)
        {
            obj = Instantiate<GameObject>(itemPrefab, new Vector3(-300 * (itemObjList.Count - 1) + (600 * i), 0, 0), transform.rotation);
            if(itemObjList[i] > 1999) //map
            {
                obj.GetComponent<Image>().sprite = GameData.instance.mapSprite[GameData.instance.mapDataList[itemObjList[i]].path];
                if(GameData.instance.mapDataList[itemObjList[i]].having)
                {
                    obj.GetComponent<SelectedItemScript>().Selected();
                }
            }
            else
            {
                obj.GetComponent<Image>().sprite = GameData.instance.charSprite[GameData.instance.charDataList[itemObjList[i]].path];
                if(GameData.instance.charDataList[itemObjList[i]].having)
                {
                    obj.GetComponent<SelectedItemScript>().Selected();
                }
            }
            obj.transform.SetParent(rootContent.transform, false);

            objList.Add(obj);
            
        }
       
    }

    void MakeBtn()
    {
        prevBtn = Instantiate<GameObject>(shopBtnPrefab, new Vector3(-200, 200, 0), transform.rotation);
        prevBtn.transform.SetParent(GameObject.Find("Canvas").transform, false);
        prevBtn.transform.localScale = new Vector3(-1, 1, 1);
        prevBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
        prevBtn.GetComponent<Image>().sprite = nextSprite;
        prevBtn.GetComponent<Button>().onClick.AddListener(() => ClickBtn(SHOPBUTTONSTATE.PREV));

        nextBtn = Instantiate<GameObject>(shopBtnPrefab, new Vector3(200, 200, 0), transform.rotation);
        nextBtn.transform.SetParent(GameObject.Find("Canvas").transform, false);
        nextBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
        nextBtn.GetComponent<Image>().sprite = nextSprite;
        nextBtn.GetComponent<Button>().onClick.AddListener(() => ClickBtn(SHOPBUTTONSTATE.NEXT));

        buyBtn = Instantiate<GameObject>(shopBtnPrefab, new Vector3(250, -550, 0), transform.rotation);
        buyBtn.transform.SetParent(GameObject.Find("Canvas").transform, false);
        buyBtn.transform.localScale = new Vector3(1, 1.5f, 1);
        buyBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
        buyBtn.GetComponent<Image>().sprite = buySprite;
        buyBtn.GetComponent<Button>().onClick.AddListener(() => ClickBtn(SHOPBUTTONSTATE.BUY));

        priceObj = Instantiate<GameObject>(textPrefab, new Vector3(0, -550, 0), transform.rotation);
        priceObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        priceObj.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 300);
        priceObj.GetComponent<Text>().fontSize = 100;        
    }
    void SetPrice(int n)
    {
        priceObj.GetComponent<Text>().text = strPrice + n + "원";
    }

    public void ActiveObjs(bool b)
    {
        rootContent.SetActive(b);
        prevBtn.SetActive(b);
        nextBtn.SetActive(b);
        buyBtn.SetActive(b);
        priceObj.SetActive(b);
        selectedIconObj.GetComponent<SelectedIconScript>().EnableScript(b);
    }

    bool HavingItem(int k)
    {
        if (k > 1999) //map
        {
            if (GameData.instance.mapDataList.ContainsKey(k)) return GameData.instance.mapDataList[k].having;
        }
        else
        {
            if (GameData.instance.charDataList.ContainsKey(k)) return GameData.instance.charDataList[k].having; 
        }
        return false;
    }
    int GetPay(int k)
    {
        if (k > 1999) //map
        {
            if (GameData.instance.mapDataList.ContainsKey(k)) return GameData.instance.mapDataList[k].pay;
        }
        else
        {
            if (GameData.instance.charDataList.ContainsKey(k)) return GameData.instance.charDataList[k].pay;
        }
        return 0;
    }

    void ClickBtn(SHOPBUTTONSTATE s)
    {
        if(s == SHOPBUTTONSTATE.PREV)
        {
            if (targetNum < 1) return;
            StartCoroutine("CoMoveList", s);
        }
        else if(s == SHOPBUTTONSTATE.NEXT)
        {
            if (targetNum > itemObjList.Count - 2) return;
            StartCoroutine("CoMoveList", s);
        }
        else if(s == SHOPBUTTONSTATE.BUY)
        {
            if (HavingItem(itemObjList[targetNum])) return;
            if (GameData.instance.playerData.coin < GetPay(itemObjList[targetNum])) return;
            GameData.instance.playerData.coin -= GetPay(itemObjList[targetNum])
                ;
            if(itemObjList[targetNum] > 1999)
            {
                GameData.instance.playerData.mapList.Add(itemObjList[targetNum]);
                GameData.instance.mapDataList[itemObjList[targetNum]].having = true;
            }
            else
            {
                GameData.instance.playerData.charList.Add(itemObjList[targetNum]);
                GameData.instance.charDataList[itemObjList[targetNum]].having = true;
            }

            GameData.instance.SavePlayerData();
            GetComponent<MenuScript>().viewCoin();
            objList[targetNum].GetComponent<SelectedItemScript>().Selected();
            SoundManagerScript.instance.PlaySE(3);
        }

    }

    IEnumerator CoMoveList(SHOPBUTTONSTATE s)
    {
        float t = 0.0f;

        if (s == SHOPBUTTONSTATE.PREV)
        {
            targetNum--;
            SetPrice(GetPay(itemObjList[targetNum]));

            while (t < 0.5f)
            {


                rootContent.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                t += Time.deltaTime;
                yield return null;
            }            
        }
        else if(s == SHOPBUTTONSTATE.NEXT)
        {
            targetNum++;
            SetPrice(GetPay(itemObjList[targetNum]));

            
            while (t < 0.5f)
            {
                rootContent.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                t += Time.deltaTime;
                yield return null;
            }
        }
   }

	
    public void ClosedShop()
    {
        rootContent.transform.position = firstPos;
        targetNum = 0;
        ActiveObjs(false);
    }
    
}
