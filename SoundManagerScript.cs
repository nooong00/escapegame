using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class SoundManagerScript : MonoBehaviour {
    public static SoundManagerScript instance;
    public AudioSource bgm;
    public AudioSource se;

    public List<AudioClip> bgmList;
    public List<AudioClip> seList;

    public List<AudioClip> list;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        bgm = gameObject.AddComponent<AudioSource>();
        se = gameObject.AddComponent<AudioSource>();

        bgm.loop = true;
        bgm.volume = 0.1f;
        bgm.playOnAwake = false;

        se.volume = 0.1f;
        se.playOnAwake = false;

        LoadAssets();
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
        bgm.clip = bgmList[key];
        bgm.Play();
    }

    public void PlaySE(int key)
    {
        se.PlayOneShot(seList[key]);
    }
}
