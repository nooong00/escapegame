﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour {
    int num;
    MenuScript obj;

	public void init(int n, string str)
    {
        num = n;
        transform.FindChild("Text").GetComponent<Text>().text = str;
        obj = GameObject.Find("GameManager").GetComponent<MenuScript>();

    }

    public void Clicked()
    {
        StartCoroutine("CoClicked");
    }

    IEnumerator CoClicked()
    {

        obj.PlayMenu(num);
        yield return new WaitForSeconds(1.0f);
    }
}