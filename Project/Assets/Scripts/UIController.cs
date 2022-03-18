using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public struct Skin
{
    public int index;
    public Sprite sprite;
    public int price;
    public bool buyed;
    public bool select;
}
public class UIController : MonoBehaviour
{
    static UIController instance;

    [Header("In Game Hud")]
    public Text points;
    public Text highScore;
    public Text coins;
    public Text coins1;
    public Text endPoints;
    public List<Transform> hearts = new List<Transform>();

    [Header("In Game Hud")]
    public GameObject gameOverWindow;
    public GameObject titleWindow;
    public GameObject storeWindow;
    public GameObject configWindow;
    public GameObject rankingWindow;
    public ModalController creditsWindow;
    public GameObject pauseWindow; 
    public GameObject pauseButon;
    public Image CD;
    public Coroutine cd; 

    [Header("Audio config")]
    public AudioMixer mixer;
    public Toggle muteToggle1;
    public Toggle muteToggle2;

    [Header("GameOver")]
    public GameObject newOBJ;
    public GameObject emoticons;


    [Header( "Skins")]
    //public List<Skin> skins = new List<Skin>();
    public Skin[] skins;
    public Image acualSkin;
    public Text actualText;
    public GameObject buyed;
    public GameObject select;
    int index;




    public static  UIController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < skins.Length-1; i++)
        {
            int index = -1;
               index = PlayerPrefs.GetInt("buy" + i);
            if(index == -1)
            {
                skins[i].buyed = false;
                skins[i].select = false; 
            }
            else
            {
                skins[i].buyed = true;
                if (PlayerPrefs.GetInt("SelectSkin") == i)
                    skins[i].select = true;
                else
                    skins[i].select = false;
            }
            
        }
    }

    private void Start()
    {
        emoticons.transform.GetChild(0).gameObject.SetActive(false);
        emoticons.transform.GetChild(1).gameObject.SetActive(false);
        emoticons.transform.GetChild(2).gameObject.SetActive(false);
        newOBJ.SetActive(false);
        titleWindow.SetActive(true);
        gameOverWindow.SetActive(false);       
        resetValues();
        PlayerController.getInstance().onDied += Player_OnDied;
        PlayerController.getInstance().onStart += Player_OnStart;

        for (int i = 0; i < skins.Length ; i++)
        {
            int index = -1;
            index = PlayerPrefs.GetInt("buy" + i);
            if (index == -1)
            {
                skins[i].buyed = false;
                skins[i].select = false;
            }
            else
            {
                skins[i].buyed = true;
                if (PlayerPrefs.GetInt("SelectSkin") == i)
                    skins[i].select = true;
                else
                    skins[i].select = false;
            }

        }

        acualSkin.sprite = skins[0].sprite;
        actualText.text = skins[0].price.ToString();
        actualText.gameObject.SetActive(!skins[0].buyed);
        buyed.SetActive(!skins[0].buyed);
        if (skins[0].select)
            select.SetActive(true);
        else
            select.SetActive(false);
    }

    private void resetValues()
    {
        float aux;
        mixer.GetFloat("MusicVolume", out aux);
        if (aux == -80)
            muteToggle1.isOn = true;
        else
            muteToggle1.isOn = false;

        mixer.GetFloat("EffectVolume", out aux);
        if (aux == -80)
            muteToggle2.isOn = true;
        else
            muteToggle2.isOn = false;
    }

    private void Player_OnStart(object sender, EventArgs e)
    { 
        pauseButon.SetActive(true);
        titleWindow.SetActive(false);
    }

    private void Player_OnDied(object sender, EventArgs e)
    {
        int hs = PlayerPrefs.GetInt("highScore");
        highScore.text = hs.ToString();
        pauseButon.SetActive(false);
    }


    void Update()
    {
        points.text = Level.GetInstance().obstaclePassed.ToString();
        coins.text = PlayerController.getInstance().coins.ToString();  
        coins1.text = PlayerController.getInstance().coins.ToString();  
        endPoints.text = points.text;
    }

    public void playEffect()
    {
        AudioController.getInstance().playOneTimeEffect(3);
    }


    public void setOnHeart(int pos)
    {
        hearts[pos].GetChild(0).gameObject.SetActive(true);
    }

    public void setOffHeart(int pos)
    {
        hearts[pos].GetChild(0).gameObject.SetActive(false);
    }

    public void openShop()
    {
        configWindow.SetActive(false);
        rankingWindow.SetActive(false);
        creditsWindow.Hide();
        storeWindow.SetActive(!storeWindow.activeSelf);
    }

    public void openConfig()
    {
        storeWindow.SetActive(false);
        rankingWindow.SetActive(false);
        creditsWindow.Hide();
        configWindow.SetActive(!configWindow.activeSelf); 
    }

    public void rankingConfig()
    {
        storeWindow.SetActive(false);
        configWindow.SetActive(false);
        creditsWindow.Hide();
        rankingWindow.SetActive(!rankingWindow.activeSelf);
    }

    public void CreditsConfig()
    {
        storeWindow.SetActive(false);
        configWindow.SetActive(false);
        rankingWindow.SetActive(false);
        
        if (creditsWindow.gameObject.activeSelf)
            creditsWindow.Hide();
        else
            creditsWindow.gameObject.SetActive(true);
    }


    public void openPause()
    {
        pauseWindow.SetActive(!pauseWindow.activeSelf);  
    }

    public void pause()
    {
        Time.timeScale = 0;
    }

    public void unPause()
    {
        Time.timeScale = 1;
    }

    public void buy()
    {
        
        if(!skins[index].buyed)
        {
            if (PlayerController.getInstance().coins >= skins[index].price)
            {
                PlayerController.getInstance().coins -= skins[index].price;
               
                PlayerPrefs.SetFloat("coins", PlayerController.getInstance().coins);
                skins[index].buyed = true;
                PlayerPrefs.SetInt("buy" + index, index);
            }
        }
        else
        {
            PlayerController.getInstance().anim.SetInteger("Skin", index);
            PlayerPrefs.SetInt("SelectSkin", index);
            skins[index].select = true;
        }
        
        actualText.gameObject.SetActive(!skins[index].buyed);
        buyed.SetActive(!skins[index].buyed);
        if (skins[index].select)
            select.SetActive(true);
        else
            select.SetActive(false);
    }

    public void setMusicVolume()
    {
        if(muteToggle1.isOn)
        {
            mixer.SetFloat("MusicVolume", -80);
        }else
        {
            mixer.SetFloat("MusicVolume", 0);
        }
    }

    public void setEffectVolume()
    {
        if (muteToggle2.isOn)
        {
            mixer.SetFloat("EffectVolume", -80);
        }
        else
        {
            mixer.SetFloat("EffectVolume", 0);
        }
    }
    public void Retry()
    {
        //StopAllCoroutines();
        SceneManager.LoadScene("Game");
        
    }

    public void nextSkin()
    {
        index++;
        if (index >= skins.Length )
            index = 0;

        acualSkin.sprite = skins[index].sprite;
        actualText.text = skins[index].price.ToString();
        actualText.gameObject.SetActive(!skins[index].buyed);
        buyed.SetActive(!skins[index].buyed);
        if (PlayerPrefs.GetInt("SelectSkin") == index)
            skins[index].select = true;
        else
            skins[index].select = false;
        if (skins[index].select)
            select.SetActive(true);
        else
            select.SetActive(false);

    }

    public void previusSkin()
    {
        index--;
        if (index < 0)
            index = skins.Length-1;

        acualSkin.sprite = skins[index].sprite;
        actualText.text = skins[index].price.ToString();
        actualText.gameObject.SetActive(!skins[index].buyed);
        buyed.SetActive(!skins[index].buyed);
        if (PlayerPrefs.GetInt("SelectSkin") == index)
            skins[index].select = true;
        else
            skins[index].select = false;

        if (skins[index].select)
            select.SetActive(true);
        else
            select.SetActive(false);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus && pauseButon.activeSelf)
        {
            Time.timeScale = 0;
            pauseWindow.SetActive(true);
        }
            
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus && pauseButon.activeSelf)
        {
            Time.timeScale = 0;
            pauseWindow.SetActive(true);
        }
            
    }

    public void CdUI()
    {
        CD.fillAmount = 1;
        cd = StartCoroutine(CDroutine());
    }


    IEnumerator CDroutine()
    {
       while(true)
       {
            CD.fillAmount -= .01f;
            yield return new WaitForSeconds(.1f);
       }
    }

}
