using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class ScoreScript : MonoBehaviour {
    const int num = 5;

    public List<int> scores;

    string scoreFilePath;
    BinaryFormatter formatter;

    public void AddScore(int n)
    {
        scores.Add(n);
        scores.Sort();
        scores.Reverse();
        scores.RemoveAt(scores.Count - 1);
        SaveScore();
    }

    public void SaveScore()
    {
        FileStream stream = new FileStream(scoreFilePath, FileMode.Create);
        formatter.Serialize(stream, scores);
        stream.Close();
    }

    public void LoadScore()
    {
        if(!System.IO.File.Exists(scoreFilePath))
        {
            for(int i = 0; i < num; ++i)
            {
                scores.Add(10 - i);
            }
            SaveScore();
            return;
        }
        FileStream stream = new FileStream(scoreFilePath, FileMode.Open);
        scores = (List<int>)formatter.Deserialize(stream);
        stream.Close();
    }

    public void InitScore()
    {
        formatter = new BinaryFormatter();
        scoreFilePath = Application.dataPath + "/Data/score.bin";
        scores = new List<int>();
        LoadScore();
    }
}
