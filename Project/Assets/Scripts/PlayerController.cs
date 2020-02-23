using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController instance;
    public static PlayerController getInstance()
    {
        return instance;
    }


    const float JUMP_AMOUNT = 20f;
    Quaternion rotationD = new Quaternion(0, 0, -.20f, 1);
    Quaternion rotationU = new Quaternion(0, 0, .40f, 1);

    public int coins = 0;
    public Animator anim;
    Rigidbody2D rb;
    State state = State.Paused;
    enum State { Playing,Dead,Paused}

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    public event EventHandler onDied;
    public event EventHandler onStart;

    public GameObject explosion;
    bool canStart;
    public bool canDie = false; 
    public SpriteRenderer spriteRender;

    int TapCount = 0;
    float MaxDubbleTapTime= .1f;
    float NewTime= 0f;

    bool canUsePU = true;

    Quaternion rotO;

    public int rescueRabbits =3 ;

    private void Awake()
    {
        state = State.Paused;
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.bodyType = RigidbodyType2D.Static;
        spriteRender = GetComponent<SpriteRenderer>();
       
    }

    private void Start()
    {
        state = State.Paused;
        anim.SetInteger("Skin", PlayerPrefs.GetInt("SelectSkin"));
        StopAllCoroutines();
        gameObject.SetActive(true);
        explosion.SetActive(false);
        coins = PlayerPrefs.GetInt("coins");
        rotO = transform.rotation;
        canStart = false;
        NewTime = 0.0f;
        TapCount = 0;

        dragDistance = Screen.height * 15 / 100;
        rescueRabbits = 3;
    }

    void FixedUpdate()
    {
        switch (state)
        {

            case State.Paused:
                if ((Input.touchCount > 0 || Input.GetKey(KeyCode.Space)) && canStart)
                {
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    if (onStart != null) onStart(this, EventArgs.Empty);
                    jump();
                }
                break;

            case State.Playing:

                if (Input.touchCount == 1 ) {            
                    jump();
                }
                else if (canDie)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationD, Time.deltaTime);
                }


                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    TapCount++;
                }

                if (TapCount > 0 )
                {
                    NewTime += Time.deltaTime;
                }

                if (TapCount >= 2 && canUsePU)
                {
                    //activatePU();
                    NewTime = 0.0f;
                    TapCount = 0;
                }

                if(NewTime > .05f)
                {
                    NewTime = 0f;
                    TapCount = 0;
                }

                break;

            case State.Dead:

                break;
        }
    }

    private void activatePU()
    {
        transform.rotation = rotO;
        anim.SetTrigger("PowerUp");
        StartCoroutine(cdPU());
        UIController.GetInstance().CdUI();
    }

 


    void input()
    {
        if (Input.touchCount > 0 ) 
        {
            jump();
            Touch touch = Input.GetTouch(0); // get the touch
            
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
            
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   
                        if ((lp.x > fp.x))  //Right swipe
                        {  
                            if (canUsePU && state == State.Playing)
                            {
                                transform.rotation = rotO;
                                anim.SetTrigger("PowerUp");
                                StartCoroutine(cdPU());
                                UIController.GetInstance().CdUI();
                            }  
                        }
                        else
                        {   //Left swipe

                        }
                    }
                    else
                    {   
                        if (lp.y > fp.y)  
                        {   //Up swipe

                        }
                        else
                        {   //Down swipe

                        }
                    }
                }
                else if (state == State.Paused)
                {
                   
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;

                    if (onStart != null) onStart(this, EventArgs.Empty);
                   
                }
            }
        }
        else if(state == State.Playing && canDie)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationD, Time.deltaTime);
        }
    }

    void Binput()
    {
            if(state == State.Paused)
            {
                  
            }


            jump();
            Touch touch = Input.GetTouch(0); // get the touch

            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag

                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        if ((lp.x > fp.x))  //Right swipe
                        {
                            if (canUsePU && state == State.Playing)
                            {
                                transform.rotation = rotO;
                                anim.SetTrigger("PowerUp");
                                StartCoroutine(cdPU());
                                UIController.GetInstance().CdUI();
                            }
                        }
                        else
                        {   //Left swipe

                        }
                    }
                    else
                    {
                        if (lp.y > fp.y)
                        {   //Up swipe

                        }
                        else
                        {   //Down swipe

                        }
                    }
                }
                else if (state == State.Paused)
                {

                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;

                    if (onStart != null) onStart(this, EventArgs.Empty);

                }
            }
    }


    public void start()
    {
        jump();
        canStart = true;
    }



    public void jump()
    {
        rb.velocity = Vector3.up *JUMP_AMOUNT  ;
        if(canDie)
        transform.rotation = Quaternion.Lerp(transform.rotation,rotationU, Time.deltaTime);
      
    }

    void setHighscore()
    {
        int newScore = Level.GetInstance().obstaclePassed;
        int actualHighScore = PlayerPrefs.GetInt("highScore");
        if (actualHighScore < newScore)
        {
            UIController.GetInstance().newOBJ.SetActive(true);
            UIController.GetInstance().emoticons.transform.GetChild(0).gameObject.SetActive(true);
            PlayerPrefs.SetInt("highScore", newScore);
        }else if(newScore > actualHighScore /2)
        {
            UIController.GetInstance().emoticons.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            UIController.GetInstance().emoticons.transform.GetChild(2).gameObject.SetActive(true);
        }
         
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            coins++;
            AudioController.getInstance().playOneTimeEffect(0);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Rabbit")
        {
            //if(rescueRabbits <=2)
            //{
            //    UIController.GetInstance().setOnHeart(rescueRabbits);
            //    rescueRabbits++;
            //}
            Level.GetInstance().obstaclePassed += 10;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Planet")
        {
            explosion.transform.position = gameObject.transform.position;
            explosion.SetActive(true);

            AudioController.getInstance().playOneTimeEffect(1);
            setHighscore();
            PlayerPrefs.SetInt("coins", coins);
            rb.bodyType = RigidbodyType2D.Static;

            gameObject.SetActive(false);
            canStart = false;
            if (onDied != null) onDied(this, EventArgs.Empty);
        }
        else if(canDie && rescueRabbits <= 0)
        {
            explosion.transform.position = gameObject.transform.position;
            explosion.SetActive(true);
           
            AudioController.getInstance().playOneTimeEffect(1);
            setHighscore();
            PlayerPrefs.SetInt("coins", coins);
            rb.bodyType = RigidbodyType2D.Static;
            canStart = false;

            gameObject.SetActive(false);

            if (onDied != null) onDied(this, EventArgs.Empty);
        }
        else if(canDie && rescueRabbits >= 0)
        {
            rescueRabbits--;

            UIController.GetInstance().setOffHeart(rescueRabbits);
           
            other.gameObject.SetActive(false);
            anim.SetTrigger("Revive");
        }
        else
        {
            other.gameObject.SetActive(false); 
        } 
    }


   

    public void reviveOn()
    {
        canDie = false;
    }

    public void reviveOff()
    {
        canDie = true;
    }


    public void powerUpOn()
    {
        AudioController.getInstance().bgMusic.pitch = 1.19f;
        BackgroundController.instance.bgMoveSpeed = 25f;
        transform.rotation = rotO;
        canDie = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void powerUpOff()
    {
        AudioController.getInstance().bgMusic.pitch = 1f;
        BackgroundController.instance.bgMoveSpeed = 5f;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 5;

    }

    IEnumerator cdPU()
    {
        canUsePU = false;
        yield return new WaitForSeconds(10f);
        canUsePU=true;
        TapCount = 0;
        NewTime = 0f;
        StopCoroutine(UIController.GetInstance().cd);
        
    }
 
}
