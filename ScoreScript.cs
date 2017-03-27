using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class ScoreScript {
    public const int num = 5;
    public List<int> scores;

    public ScoreScript()
    {
        scores = new List<int>();
    }

    public void AddScore(int n)
    {
        scores.Add(n);
        scores.Sort();
        scores.Reverse();
        if(scores.Count > num)
        {
            scores.RemoveAt(num);
        }
    }
    
}
