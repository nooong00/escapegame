using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour {
    public float timer = 2.0f;//타이머
    int posX;
    int posY;

    public void setPos(int x, int y)
    {
        posX = x;
        posY = y;
        GameManagerScript.instance.mapCheck[x, y] = 1;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine("CoFire");
	}
	
    IEnumerator CoFire()
    {
        yield return new WaitForSeconds(timer);
        GameManagerScript.instance.mapCheck[posX, posY] = 0;
        Destroy(gameObject);
    }
}
