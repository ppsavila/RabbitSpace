using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{

    private Vector2 resetPos = new Vector2(-150f, 0f);
    private Vector2 newPos;

    private float speed=0;
    public Animator anim;

    private void Awake()
    {
      //  anim = GetComponentInChildren<Animator>();
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setNewPos(float x, float y)
    {
        newPos = new Vector2(x, y);
    }

    void FixedUpdate()
    {
        this.transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.x <= -40f)
            gameObject.SetActive(false);
    }


 

    private void OnEnable()
    {
        if (gameObject.CompareTag("Rabbit"))
        {
            int rnd = Random.Range(0, 6);
            anim.SetFloat("Blend", rnd);
        }

            this.transform.position = newPos;
    }

    private void OnDisable()
    {
        if(gameObject.CompareTag("Meteoro"))
        {
            Level.GetInstance().obstaclePassed++;
        }
           
        this.transform.position = resetPos;
    }


   
}
