using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyCanvas : MonoBehaviour
{
    [SerializeField] Text ScoreText, MaxScoreText;

    private void Start()
    {
        int currentScore = PlayerPrefs.GetInt("score");
        int maxScore = PlayerPrefs.GetInt("max_score");

        ScoreText.text = currentScore.ToString();
        MaxScoreText.text = maxScore.ToString();
    }
    public void StartGame()
    {
        if (PlayerPrefs.HasKey("tutorial"))
        {
            SceneManager.LoadScene("Level");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}