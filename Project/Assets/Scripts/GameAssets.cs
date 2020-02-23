using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    static GameAssets instance;
    public static GameAssets getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public Transform obstaculo;
    public Transform Coin;
    public Transform Rabbit;

    #region FlappyBird
    public Transform pipeHead;
    public Transform pipeBody;
  
    #endregion

        
}
