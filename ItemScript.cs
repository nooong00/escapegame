using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {
    float timer = 3.0f;
    int posX;
    int posY;

	// Use this for initialization
	void Start () {
        StartCoroutine("CoCoin");
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator CoCoin()
    {
        yield return new WaitForSeconds(timer);
//        Destroy(gameObject);
    }
}
