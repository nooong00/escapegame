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

    List<GameObject> tmpList;
    ItemDataScript[] itemDataList;

    // Use this for initialization
    void Awake () {

        tmpList = new List<GameObject>();

        itemDataList = new ItemDataScript[GameData.instance.itemList.Count];

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

        rootContent.GetComponent<RectTransform>().sizeDelta = new Vector2(200 + (GameData.instance.itemList.Count * 600), rootContent.GetComponent<RectTransform>().rect.height);
        firstPos = rootContent.transform.position;

        ActiveObjs(false);
    }

    //데이터 읽어옴
    void LoadList()
    {
        rootContent.transform.position = firstPos;
        for (int i = 0; i < GameData.instance.itemList.Count; ++i)
        {
            GameObject obj = Instantiate<GameObject>(itemPrefab, new Vector3(-300 * (GameData.instance.itemList.Count - 1) + (600 * i), 0, 0), transform.rotation);
            obj.GetComponent<ItemDataScript>().SetData(GameData.instance.itemList[i]);
            itemDataList[i] = obj.GetComponent<ItemDataScript>();
            obj.transform.SetParent(rootContent.transform, false);
            
            if(itemDataList[i].data.type == ItemType.MAP)
            {
                obj.GetComponent<Image>().sprite = GameData.instance.mapSprite[itemDataList[i].data.path];
            }
            else if(itemDataList[i].data.type == ItemType.CHARACTER)
            {
                obj.GetComponent<Image>().sprite = GameData.instance.charSprite[itemDataList[i].data.path];
            }


            if(obj.GetComponent<ItemDataScript>().data.having)
            {
                obj.GetComponent<SelectedItemScript>().Selected();
            }
            tmpList.Add(obj);
            
        }
        
        selectedIconObj.GetComponent<SelectedIconScript>().SetPosition(tmpList[targetNum].transform.position.x);

        SetPrice(itemDataList[targetNum].data.pay);

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

    void ClickBtn(SHOPBUTTONSTATE s)
    {
        if(s == SHOPBUTTONSTATE.PREV)
        {
            if (targetNum < 1) return;
            StartCoroutine("CoMoveList", s);
        }
        else if(s == SHOPBUTTONSTATE.NEXT)
        {
            if (targetNum > itemDataList.Length - 2) return;
            StartCoroutine("CoMoveList", s);
        }
        else if(s == SHOPBUTTONSTATE.BUY)
        {
            if (itemDataList[targetNum].data.having) return;
            if (GameData.instance.playerData.coin < itemDataList[targetNum].data.pay) return;
            GameData.instance.playerData.coin -= itemDataList[targetNum].data.pay;
            if(itemDataList[targetNum].data.type == ItemType.MAP)
            {
                GameData.instance.playerData.mapList.Add(itemDataList[targetNum].data);
            }
            else if(itemDataList[targetNum].data.type == ItemType.CHARACTER)
            {
                GameData.instance.playerData.charList.Add(itemDataList[targetNum].data);
            }
            GameData.instance.SavePlayerData();
            GetComponent<MenuScript>().viewCoin();
            itemDataList[targetNum].data.having = true;
            tmpList[targetNum].GetComponent<SelectedItemScript>().Selected();
            SoundManagerScript.instance.PlaySE(3);
        }

    }

    IEnumerator CoMoveList(SHOPBUTTONSTATE s)
    {
        float t = 0.0f;
        if(s == SHOPBUTTONSTATE.PREV)
        {
            targetNum--;
            SetPrice(itemDataList[targetNum].data.pay);
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
            SetPrice(itemDataList[targetNum].data.pay);
            while (t < 0.5f)
            {
                rootContent.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                t += Time.deltaTime;
                yield return null;
            }
        }
        selectedIconObj.GetComponent<SelectedIconScript>().SetPosition(tmpList[targetNum].transform.position.x);
    }

	
    public void ClosedShop()
    {
        rootContent.transform.position = firstPos;
        targetNum = 0;
        ActiveObjs(false);
    }
    
}
