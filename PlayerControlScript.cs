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

  
    float tileUnit = 0.7f;
    Vector3 startPos;

    bool damaged;
    int life;
 //   Color invisibleColor;
    SpriteRenderer playerRenderer;

    // Use this for initialization
    void Start () {
        animator = transform.Find("char").GetComponent<Animator>();
        
        state = PlayerState.IDLE;
        posX = 3;
        posY = 3;
        startPos.Set(-posX * tileUnit, -posY * tileUnit, 0.0f);
        damaged = false;

        playerRenderer = transform.Find("char").GetComponent<SpriteRenderer>();
        life = GameData.instance.charDataList[GameData.instance.playerData.charList[GameData.instance.selectedChar]].life;

        mapWidth = GameObject.Find("GameManager").GetComponent<MapCreater>().mapWidth;
        mapHeight = GameObject.Find("GameManager").GetComponent<MapCreater>().mapHeight;

// StartCoroutine("CO1");모바일

        StartCoroutine("CO2");

    }
    Vector2 touchPos;
    IEnumerator CO1()
    {
        Touch touch;
        while(true)
        {
            if(Input.touches.Length > 0)
            {
                if(state == PlayerState.IDLE)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchPos = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        if (Vector2.Distance(touchPos, touch.position) > 50.0f)
                        {
                            if (touchPos != Vector2.zero)
                            {
                                Move(getDir(touchPos - touch.position));
                                touchPos = Vector2.zero;
                            }
                        }
                    }
                }
                yield return null;
            }
            yield return null;
        }
    }

    public int GetX()
    {
        return posX;
    }
    public int GetY()
    {
        return posY;
    }

    IEnumerator CO2()
    {
        bool moved = false;
        Vector3 mousePos = Vector3.zero;
        while (true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
            }
            else if(Input.GetMouseButton(0))
            {
                if (Vector2.Distance(mousePos, Input.mousePosition) > 50.0f)
                {
                    if (!moved)
                    {
                        Move(getDir(mousePos - Input.mousePosition));
                        moved = true;
                    }
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                moved = false;
            }

            yield return null;
        }
    }







    Dir getDir(Vector2 p)
    {
        //방향 리턴
        if(Mathf.Abs(p.x) > Mathf.Abs(p.y))//hrizontal
        {
            if(p.x > 0)
            {
                return Dir.LEFT;
            }
            else
            {
                return Dir.RIGHT;
            }
        }
        else
        {
            if(p.y > 0)
            {
                return Dir.DOWN;
            }
            else
            {
                return Dir.UP;
            }
        }
    }

    int mapWidth;
    int mapHeight;

    bool Movable(int x, int y)
    {
        if (x < 0) return false;
        else if (x > mapWidth - 1) return false;
        else if (y < 0) return false;
        else if (y > mapHeight - 1) return false;
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
