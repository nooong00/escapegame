using System.Collections;
using UnityEngine;

public class SelectedIconScript : MonoBehaviour {
    Vector3 dir;
    float speed;

    void Start()
    {
        dir = Vector3.up;
        speed = 0.2f;
    }

    public void SetPosition(float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void Move(Vector3 pos)
    {
        pos = new Vector3(pos.x, transform.position.y, transform.position.z);
        Vector3.Lerp(transform.position, pos, 1.0f);
    }

    public void EnableScript(bool b)
    {
        if(b)
        {
            StartCoroutine("DirChange");
            StartCoroutine("UpDownMove");
        }
        else
        {
            StopAllCoroutines();
            dir = Vector3.up;
        }
    }

    IEnumerator DirChange()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            dir *= -1;
        }
    }

    IEnumerator UpDownMove()
    {
        while(true)
        {
            transform.Translate(dir * speed * Time.deltaTime);
            yield return null;
        }        
    }
}
