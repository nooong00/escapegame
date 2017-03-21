using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class ScoreScript : MonoBehaviour {
    public List<int> scores;
    int num;
    string scoreFilePath;
    BinaryFormatter formatter;

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
                //                scores.Add(new Score(i + 1, 5 - i));
                scores.Add(10 - i);
            }
            SaveScore();
            return;
        }
        FileStream stream = new FileStream(scoreFilePath, FileMode.Open);
        scores = (List<int>)formatter.Deserialize(stream);
        stream.Close();
    }

    public void InitScore(int n)
    {
        formatter = new BinaryFormatter();
        scoreFilePath = Application.dataPath + "/Data/score.bin";
        num = n;
        scores = new List<int>();
        LoadScore();
    }
}
