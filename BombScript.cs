using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {
    public float readyTime; //위치 알려줌
    public float idleTime;  //대기
    public float emergencyTime; //터지기 일보직전

    float timer;
    int posX;
    int posY;

    int power = 2;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        
        StartCoroutine("CoReady");
  	}

    public void initPos(int x, int y)
    {
        posX = x;
        posY = y;
    }

    void setScale(int value)
    {
        if(value == 0)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0.0f);
        }
        else
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
        }

    }
    IEnumerator CoReady()
    {
        float t = 1 / (readyTime * 10);
        Color color = transform.GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < readyTime * 10; ++i)
        {
            color.a += t;
            transform.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
        GameManagerScript.instance.mapCheck[posX, posY] = 2;
        StartCoroutine("CoAlive");
        StartCoroutine("CoEmergency");
    }

    IEnumerator CoAlive()
    {
        int tmp = 0;
        while(true)
        {
            setScale(tmp);
            tmp++;
            if (tmp > 1) tmp = 0;
            
            yield return new WaitForSeconds(0.5f);
        }

    }

    IEnumerator CoEmergency()
    {
        yield return new WaitForSeconds(idleTime);
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(emergencyTime);



        /////임시
        Instantiate(GameManagerScript.instance.FireObj, transform.position, transform.rotation).GetComponent<FireObjScript>().setPos(posX, posY);

        for(int i = 1; i < power + 1; ++i)
        {
            if(posX + i < GameManagerScript.instance.mapWidth)
            {
                if (GameManagerScript.instance.mapCheck[posX + i, posY] == 0)
                {
                    Instantiate(GameManagerScript.instance.FireObj, GameManagerScript.instance.getPos(posX + i, posY), transform.rotation).GetComponent<FireObjScript>().setPos(posX + i, posY);
                }
            }
            if(posX > i - 1)
            {
                if (GameManagerScript.instance.mapCheck[posX - i, posY] == 0)
                {
                    Instantiate(GameManagerScript.instance.FireObj, GameManagerScript.instance.getPos(posX - i, posY), transform.rotation).GetComponent<FireObjScript>().setPos(posX - i, posY);
                }
            }
        }

        for (int i = 1; i < power + 1; ++i)
        {
            if(posY + i < GameManagerScript.instance.mapHeight)
            {
                if (GameManagerScript.instance.mapCheck[posX, posY + i] == 0)
                {
                    Instantiate(GameManagerScript.instance.FireObj, GameManagerScript.instance.getPos(posX, posY + i), transform.rotation).GetComponent<FireObjScript>().setPos(posX, posY + i);
                }
            }
            if(posY > i - 1)
            {
                if (GameManagerScript.instance.mapCheck[posX, posY - i] == 0)
                {
                    Instantiate(GameManagerScript.instance.FireObj, GameManagerScript.instance.getPos(posX, posY - i), transform.rotation).GetComponent<FireObjScript>().setPos(posX, posY - i);
                }
            }
        }

                


        Destroy(gameObject);
    }

}
