using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class SoundManagerScript : MonoBehaviour {
    public static SoundManagerScript instance;

    AudioSource bgm;
    AudioSource se;

    public List<AudioClip> bgmList;
    public List<AudioClip> seList;

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
            bgmList = new List<AudioClip>();
            seList = new List<AudioClip>();

            bgm = gameObject.AddComponent<AudioSource>();
            bgm.volume = 0.1f;
            bgm.loop = true;

            se = gameObject.AddComponent<AudioSource>();
            se.volume = 0.1f;

            LoadAssets();
        }
    }
    
    public void LoadAssets()
    {
        AudioClip[] bgms = Resources.LoadAll<AudioClip>("Music/BGM");
        for (int i = 0; i < bgms.Length; ++i)
        {
            bgmList.Add(bgms[i]);
        }
        AudioClip[] ses = Resources.LoadAll<AudioClip>("Music/SE");
        for(int i = 0; i< ses.Length; ++i)
        {
            seList.Add(ses[i]);
        }

    }

    public void PlayBGM(int key)
    {
        if(bgm.isPlaying)
        {
            bgm.Stop();
        }
        bgm.clip = bgmList[key];
        bgm.Play();
    }

    public void PlaySE(int key)
    {
        se.PlayOneShot(seList[key]);
    }
}
