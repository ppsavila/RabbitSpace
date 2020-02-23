using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    static AudioController instance;  
    public AudioSource[] effects;
    public AudioSource bgMusic;

    public static AudioController getInstance()
    {
       return instance;
    }

    private void Awake()
    {
       // DontDestroyOnLoad(gameObject);
        instance = this;
       
    }

    private void Start()
    {
        bgMusic = GetComponent<AudioSource>();
    }

    public void playOneTimeEffect(int i)
    {
        effects[i].loop = false;
        effects[i].Play();
    }


}
