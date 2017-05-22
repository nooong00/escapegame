using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScript : MonoBehaviour {
    SpriteRenderer logo;

	void Awake()
    {
        Screen.SetResolution(720, 1280, true);
        logo = GameObject.Find("Logo").GetComponent<SpriteRenderer>();
        GameData d = gameObject.AddComponent<GameData>();
        d.init();
        SoundManagerScript s = gameObject.AddComponent<SoundManagerScript>();
        s.Init();
        DontDestroyOnLoad(this);
        StartCoroutine("LogoAnim", 0);
        StartCoroutine("NextScene");
    }

    IEnumerator LogoAnim(int a)
    {
        Color c = logo.color;
        c.a = a;
        float alpha = 0.05f;
        if (a == 1) alpha = -alpha;
        for(int i = 0; i < 20; ++i)
        {
            c.a += alpha;
            logo.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        if (a == 0) StartCoroutine("LogoAnim", 1);
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("MenuScene");
    }
}
