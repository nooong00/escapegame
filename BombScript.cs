using System.Collections;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public float readyTime; //생성
    public float idleTime;  //대기
    public float emergencyTime; //경고

    int posX;
    int posY;

    int power = 2;

	// Use this for initialization
    void Awake()
    {
        Init();
    }

    void Init()
    {
        power = 2;
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);
    }

    public void Play(int x, int y)
    {
        gameObject.SetActive(true);

        posX = x;
        posY = y;

        transform.position = GameManagerScript.instance.getPos(x, y);

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
        while(true)
        {
            setScale(0);
            yield return new WaitForSeconds(0.5f);

            setScale(1);
            yield return new WaitForSeconds(0.5f);
        }

    }

    IEnumerator CoEmergency()
    {
        yield return new WaitForSeconds(idleTime);
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(emergencyTime);
        
        for(int i = -2; i < 3; ++i)
        {
            if(i == 0)
            {
                GameManagerScript.instance.MakeFire(posX, posY);
            }
            else
            {
                GameManagerScript.instance.MakeFire(posX + i, posY);
                GameManagerScript.instance.MakeFire(posX, posY + i);
            }
        }

        SoundManagerScript.instance.PlaySE(0);
        GameManagerScript.instance.mapCheck[posX, posY] = 0;

        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);
        GameManagerScript.instance.bombPool.Push(gameObject);
    }

}
