using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour {
    public float timer;

    public void SetTimer(float t)
    {
        timer = t;
    }

    public void Play(int x, int y)
    {
        gameObject.SetActive(true);
        transform.position = GameManagerScript.instance.getPos(x, y);
        StartCoroutine("CoPlay");
    }

    IEnumerator CoPlay()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        GameManagerScript.instance.firePool.Push(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<PlayerControlScript>().GetDamage();
            gameObject.SetActive(false);
            GameManagerScript.instance.firePool.Push(gameObject);
        }
    }
	
}
