using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour  {
    public float timer;

    public void SetTimer(float t)
    {
        timer = t;
    }

    public void Play()
    {
        Destroy(gameObject, timer);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            GameManagerScript.instance.GetCoin();
            Destroy(gameObject);
        }
    }

}
