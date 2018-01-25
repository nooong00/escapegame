using System.Collections;
using UnityEngine;

public class CoinScript : MonoBehaviour  {
    public float timer;
    int posX;
    int posY;

    public void SetTimer(float t)
    {
        timer = t;
    }

    public void Play(int x, int y)
    {
        gameObject.SetActive(true);
        posX = x;
        posY = y;
        transform.position = GameManagerScript.instance.getPos(x, y);
        StartCoroutine("CoPlay");
    }

    IEnumerator CoPlay()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        GameManagerScript.instance.mapCheck[posX, posY] = 0;
        GameManagerScript.instance.coinPool.Push(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            GameManagerScript.instance.GetCoin();
            gameObject.SetActive(false);
            GameManagerScript.instance.mapCheck[posX, posY] = 0;
            GameManagerScript.instance.coinPool.Push(gameObject);
        }
    }

}
