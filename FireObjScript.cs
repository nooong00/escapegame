﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour {
    public float timer;

    public void SetTimer(float t)
    {
        timer = t;
    }

    public void Play(float t)
    {
        Destroy(gameObject, t);
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<PlayerControlScript>().GetDamage();
            Destroy(gameObject);            
        }
    }
	
}
