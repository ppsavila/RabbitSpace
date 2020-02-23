using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform Bg;
     public float bgMoveSpeed = 5f;

    public static BackgroundController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Mov();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).position.x < -60f)
            {
                transform.GetChild(i).position = new Vector3(100f, 0, 0);
            }
        }
    }

    private void Mov()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            this.transform.GetChild(i).transform.position += new Vector3(-1, 0, 0) * bgMoveSpeed * Time.deltaTime;
        }
    }
}
