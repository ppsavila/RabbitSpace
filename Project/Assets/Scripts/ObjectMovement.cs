using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{

    private Vector2 resetPos = new Vector2(-150f, 0f);
    private Vector2 newPos;

    [field: SerializeField] private CircleCollider2D CircleCollider2D{get;set;}
    
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



    private void SetOffset()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(CircleCollider2D.transform.position, CircleCollider2D.radius);
        foreach (var other in colliders)
        {
            if (other != CircleCollider2D && !other.CompareTag("Player")) // Ignorar colisão consigo mesmo
            {
                Vector2 offset = CalculateOffset(CircleCollider2D, other);
                transform.position += (Vector3)offset; // Ajustar a posição para evitar sobreposição
                Debug.Log($"Offset aplicado: {offset}");
            }
        }
    }

       private Vector2 CalculateOffset(CircleCollider2D circle1, Collider2D circle2)
    {
        Vector2 direction = (Vector2)circle1.transform.position - (Vector2)circle2.transform.position;
        direction.Normalize();

        float distanceToMoveOut = circle1.radius + GetRadius(circle2) - Vector2.Distance(circle1.transform.position, circle2.transform.position);

        return direction * distanceToMoveOut;
    }

    private float GetRadius(Collider2D collider)
    {
        if (collider is CircleCollider2D circle)
        {
            return circle.radius * collider.transform.localScale.x; // Considere o scale
        }
        return 0;
    }

    void FixedUpdate()
    {
        this.transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.x <= -40f)
            gameObject.SetActive(false);

        SetOffset();
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
