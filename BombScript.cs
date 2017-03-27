using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {
    const float defaultTimer = 2.0f;
    const int defaultPower = 2;

    public float readyTime; //생성
    public float idleTime;  //대기
    public float emergencyTime; //경고

//    float timer;
    int posX;
    int posY;

    int power = 2;

	// Use this for initialization
	void Start () {
//        timer = 0.0f;
        power = defaultPower;
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
            setScale(tmp % 2);
            tmp++;
            yield return new WaitForSeconds(0.5f);
        }

    }

    IEnumerator CoEmergency()
    {
        yield return new WaitForSeconds(idleTime);
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(emergencyTime);

        Instantiate(GameManagerScript.instance.fireObj, transform.position, transform.rotation).GetComponent<FireObjScript>().Play(defaultTimer);

        for(int i = 1; i < power + 1; ++i)
        {
            if(posX + i < GameManagerScript.instance.mapWidth)
            {
                if (GameManagerScript.instance.mapCheck[posX + i, posY] != 2)
                {
                    Instantiate(GameManagerScript.instance.fireObj, GameManagerScript.instance.getPos(posX + i, posY), transform.rotation).GetComponent<FireObjScript>().Play(defaultTimer);
                }
            }
            if(posX > i - 1)
            {
                if (GameManagerScript.instance.mapCheck[posX - i, posY] != 2)
                {
                    Instantiate(GameManagerScript.instance.fireObj, GameManagerScript.instance.getPos(posX - i, posY), transform.rotation).GetComponent<FireObjScript>().Play(defaultTimer);
                }
            }
        }

        for (int i = 1; i < power + 1; ++i)
        {
            if(posY + i < GameManagerScript.instance.mapHeight)
            {
                if (GameManagerScript.instance.mapCheck[posX, posY + i] != 2)
                {
                    Instantiate(GameManagerScript.instance.fireObj, GameManagerScript.instance.getPos(posX, posY + i), transform.rotation).GetComponent<FireObjScript>().Play(defaultTimer);
                }
            }
            if(posY > i - 1)
            {
                if (GameManagerScript.instance.mapCheck[posX, posY - i] != 2)
                {
                    Instantiate(GameManagerScript.instance.fireObj, GameManagerScript.instance.getPos(posX, posY - i), transform.rotation).GetComponent<FireObjScript>().Play(defaultTimer);
                }
            }
        }

        SoundManagerScript.instance.PlaySE(0);
        GameManagerScript.instance.mapCheck[posX, posY] = 0;
        Destroy(gameObject);
    }

}
