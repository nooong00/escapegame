using System.Collections.Generic;


public class ScoreScript {
    public const int num = 5;
    public List<int> scores;

    public ScoreScript()
    {
        scores = new List<int>();
    }

    public void AddScore(int n)
    {
        if(scores.Count < num)
        {
            scores.Add(n);
        }
        else
        {
            if (scores[scores.Count - 1] > n)
            {
                return;
            }

            scores[scores.Count - 1] = n;
            scores.Sort();
        }
    }
    
}
