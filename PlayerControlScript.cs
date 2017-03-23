using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControlScript : MonoBehaviour {
enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    };

    enum PlayerState
    {
        IDLE,
        MOVE,
        DAMAGED,
        DEAD,
    };


    Animator animator;
    int posX;
    int posY;
    PlayerState state;
    float speed = 3.0f;

    //임시 변수
    float tileUnit = 0.7f;
    Vector3 startPos;

    bool damaged;
    int life;
 //   Color invisibleColor;
    SpriteRenderer playerRenderer;

    // Use this for initialization
    void Start () {
        animator = transform.FindChild("char").GetComponent<Animator>();
        
        state = PlayerState.IDLE;
        posX = 3;
        posY = 3;
        startPos.Set(-posX * tileUnit, -posY * tileUnit, 0.0f);
        damaged = false;
//        invisibleColor = new Color(1, 1, 1, 0);
        playerRenderer = transform.FindChild("char").GetComponent<SpriteRenderer>();
        life = 3;
    }
	
	// Update is called once per frame
	void Update () {
        if(state == PlayerState.IDLE)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (posY > 5) return;
                Move(Dir.UP);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (posY < 1) return;
                Move(Dir.DOWN);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (posX < 1) return;
                Move(Dir.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (posX > 5) return;
                Move(Dir.RIGHT);
            }
        }

    }

    bool Movable(int x, int y)
    {
        return (GameManagerScript.instance.mapCheck[x, y] == 0 || GameManagerScript.instance.mapCheck[x, y] == 1);
    }

    void Move(Dir dir)
    {
        if (state != PlayerState.IDLE) return;      
        switch (dir)
        {
            case Dir.UP:
                if (!Movable(posX, posY + 1)) return;
                animator.SetTrigger("up");
                posY++;                
                break;
            case Dir.DOWN:
                if (!Movable(posX, posY - 1)) return;
                animator.SetTrigger("down");
                posY--;
                break;
            case Dir.LEFT:
                if (!Movable(posX - 1, posY)) return;
                animator.SetTrigger("left");
                posX--;
                break;
            case Dir.RIGHT:
                if (!Movable(posX + 1, posY)) return;
                animator.SetTrigger("right");
                posX++;
                break;
        }

        state = PlayerState.MOVE;
        StartCoroutine("CoMove", dir);
    }

    Vector3 getPos(int mX, int mY)
    {
        return new Vector3(startPos.x + (mX * tileUnit), startPos.y + (mY * tileUnit), -1);
    }

    public void GetDamage()
    {
        if (state == PlayerState.DEAD) return;
        if (damaged) return;
        
        life--;
        GameManagerScript.instance.LostLife(life);
        if (life < 1)
        {
            //죽음
            state = PlayerState.DEAD;
            StartCoroutine("CoDead");            
            return;
        }
       

        StartCoroutine("CoDamaged");
    }


    IEnumerator CoDead()
    {
        damaged = true;
        Color tmp = Color.white;
        GameManagerScript.instance.GameOver();
        for(int i = 0; i < 20; ++i)
        {
            playerRenderer.color = tmp;
            tmp.a -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        } 
        Destroy(gameObject);
    }

    IEnumerator CoDamaged()
    {
        damaged = true;
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(2.0f);
        playerRenderer.color = Color.white;

        damaged = false;
    }

    IEnumerator CoMove(Dir dir)
    {
        Vector3 tmp = getPos(posX, posY);
        bool exited = true;
        
        while(exited)
        {
            switch(dir)
            {
                case Dir.UP:
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                    if (transform.position.y > tmp.y) exited = false;    
                    break;
                case Dir.DOWN:
                    transform.Translate(Vector3.down * speed * Time.deltaTime);
                    if (transform.position.y < tmp.y) exited = false;
                    break;
                case Dir.LEFT:
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    if (transform.position.x < tmp.x) exited = false;
                    break;
                case Dir.RIGHT:
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    if (transform.position.x > tmp.x) exited = false;
                    break;
            }
            yield return null;
        }
        animator.ResetTrigger("up");
        animator.ResetTrigger("down");
        animator.ResetTrigger("left");
        animator.ResetTrigger("right");

        transform.position = tmp;
        if (state != PlayerState.DEAD)
        {
            state = PlayerState.IDLE;
        }
    }

}
