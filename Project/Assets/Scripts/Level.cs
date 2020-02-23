using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    static Level instance;

    const float WIDTH = 7.8f;
    float SpeedMov = 30f;
    const float OBSTACLE_DESTROY_POS = -100f;
    const float OBSTACLE_INSTA_POS = 100f;
    const float minSize = 10f;
    const float maxSize = 40f;
    const float minPosX = -31f;
    const float maxPosX = 39f;


    public List<ObjectMovement> obstacles = new List<ObjectMovement>();
    public List<ObjectMovement> coins = new List<ObjectMovement>();
    public List<ObjectMovement> rabbits = new List<ObjectMovement>();

    public GameObject BGpreto;
    public GameObject BGbranco;
    bool scene = false;

    float spawnTimer =0;
    float spawnRabbitTimer;
    WaitForSeconds spawnTimeMax;
    WaitForSeconds spawnCoinTimeMax;
    WaitForSeconds spawnRabbitTimeMax;
    float gapSize = 70f;
    int obstacleQnt;
    public int obstaclePassed;
    State state;

    enum Difficulty { Easy, Medium, Hard, Impossible,Insane,Max}
    enum State { Playing, Paused, Dead }



    public static Level GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        obstaclePassed = 0;
        state = State.Paused;
        setDifficulty(Difficulty.Easy);
        spawnTimeMax = new WaitForSeconds(spawnTimer);
        spawnCoinTimeMax = new WaitForSeconds(2f);
        spawnRabbitTimeMax = new WaitForSeconds(20f);
        PlayerController.getInstance().onDied += Player_OnDied;
        PlayerController.getInstance().onStart += Player_OnStart;
    }



    void Player_OnDied(object sender, System.EventArgs e)
    {
        state = State.Dead;
        StopAllCoroutines();

    }

    private void Player_OnStart(object sender, EventArgs e)
    {
        state = State.Playing;
        StartCoroutine(MeteoroSpawn());
        StartCoroutine(CoinsSpawn());
        StartCoroutine(RabbitsSpawn());
    }

    IEnumerator MeteoroSpawn()
    {
        while(true)
        {
            
            yield return spawnTimeMax;
            setDifficulty(GetDifficulty()); 
            if(GetDifficulty() == Difficulty.Hard || GetDifficulty() == Difficulty.Impossible) 
                ObstacleSpawn();

           
            ObstacleSpawn(); 
        }
    }


    IEnumerator CoinsSpawn()
    {
        while (true)
        {
           
            yield return spawnCoinTimeMax;
            CoinSpawn();
        }
    }
    IEnumerator RabbitsSpawn()
    {
        while (true)
        {
           
            yield return spawnRabbitTimeMax;
            RabbitSpawn();
        }
    }

    private void ObstacleSpawn()
    {
            
            float size = UnityEngine.Random.Range(minSize, maxSize);
            float posX = UnityEngine.Random.Range(minPosX, maxPosX);
            float posY = UnityEngine.Random.Range(minPosX, maxPosX);
            ObjectMovement auxMeteoro = obstacles.Find(x => !x.gameObject.activeSelf);
            auxMeteoro.setNewPos(120f-posX,posY);
            auxMeteoro.setSpeed(SpeedMov);
            auxMeteoro.gameObject.SetActive(true);
    }

    private void CoinSpawn()
    {
            float size = UnityEngine.Random.Range(minSize, maxSize);
            float posX = UnityEngine.Random.Range(minPosX, maxPosX);
            float posY = UnityEngine.Random.Range(minPosX, maxPosX);
            ObjectMovement auxMeteoro = coins.Find(x => !x.gameObject.activeSelf);
            auxMeteoro.setNewPos(120f - posX, posY);
            auxMeteoro.setSpeed(SpeedMov);
            auxMeteoro.gameObject.SetActive(true);
    }

    private void RabbitSpawn()
    {
            float size = UnityEngine.Random.Range(minSize, maxSize);
            float posX = UnityEngine.Random.Range(minPosX, maxPosX);
            float posY = UnityEngine.Random.Range(minPosX, maxPosX);
        if (PlayerController.getInstance().rescueRabbits < 3)
        {
            ObjectMovement auxMeteoro = rabbits.Find(x => !x.gameObject.activeSelf);
            //float rnd = UnityEngine.Random.Range(0, 6);
            //auxMeteoro.anim.SetFloat("Blend", rnd);
            auxMeteoro.setNewPos(120f - posX, posY);
            auxMeteoro.setSpeed(SpeedMov);
            auxMeteoro.gameObject.SetActive(true);
        }    
    }

    private void PlayPassSound()
    {


        if(obstacleQnt == 60 || obstacleQnt == 40 || obstacleQnt == 20)
        {
            AudioController.getInstance().playOneTimeEffect(2);
        }
    }





    void setDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                
                gapSize = 40;
                spawnTimer = 1.5f;
                SpeedMov = 30f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;
            case Difficulty.Medium:
               
                gapSize = 35;
                spawnTimer = 1.2f;
                SpeedMov = 40f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;
            case Difficulty.Hard:

                //BGbranco.SetActive(true);
                //BGpreto.SetActive(false);
                //foreach (ObjectMovement obstacle in obstacles)
                //{
                //    obstacle.anim.SetBool("coockie", true);
                //}
                //Camera.main.backgroundColor = Color.white;

                gapSize = 35;
                spawnTimer = 1f;
                SpeedMov = 50f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;
            case Difficulty.Impossible:


                //BGbranco.SetActive(false);
                //BGpreto.SetActive(true);
                //foreach (ObjectMovement obstacle in obstacles)
                //{
                //    obstacle.anim.SetBool("coockie", false);
                //}
                //Camera.main.backgroundColor = Color.black;

                gapSize = 30;
                spawnTimer = .8f;
                SpeedMov = 55f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;

            case Difficulty.Insane:


                //BGbranco.SetActive(true);
                //BGpreto.SetActive(false);
                //foreach (ObjectMovement obstacle in obstacles)
                //{
                //    obstacle.anim.SetBool("coockie", true);
                //}
                //Camera.main.backgroundColor = Color.white;

                gapSize = 25;
                spawnTimer = .8f;
                SpeedMov = 60f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;

            case Difficulty.Max:


                //BGbranco.SetActive(false);
                //BGpreto.SetActive(true);
                //foreach (ObjectMovement obstacle in obstacles)
                //{
                //    obstacle.anim.SetBool("coockie", false);
                //}
                //Camera.main.backgroundColor = Color.black;

                gapSize = 25;
                spawnTimer = .8f;
                SpeedMov = 65f;
                spawnTimeMax = new WaitForSeconds(spawnTimer);
                break;

        }
    }

    Difficulty GetDifficulty()
    {

        if (obstaclePassed > 200)
        {

            return Difficulty.Max;
        }
        else if(obstaclePassed > 150)
        {

            return Difficulty.Insane;
        }
        else if (obstaclePassed > 100)
        {
           
            return Difficulty.Impossible;
        }
        else if (obstaclePassed > 50)
        {
            
            return Difficulty.Hard;
        }
        else if (obstaclePassed > 20)
        {
         
            return Difficulty.Medium;
        }
        
    
        return Difficulty.Easy;
    }


}

