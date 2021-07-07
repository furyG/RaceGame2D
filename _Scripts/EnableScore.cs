using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScore : MonoBehaviour
{
    static public int score = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }
        PlayerPrefs.SetInt("HighScore", score);
    }
    private void Update()
    {
        if (score > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", score);
    }
}
