using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager current;

    [SerializeField] private Text scoreText;

    private int score;
    
    void Start()
    {
        current = this;
        scoreText.text = "0";
    }

    static public int IncrementScore()
    {
        return current.IncrementScoreImpl();
    }

    private int IncrementScoreImpl()
    {
        ++score;
        scoreText.text = score.ToString();

        return score;
    }
}
