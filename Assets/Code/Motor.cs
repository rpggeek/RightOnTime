using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Motor : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] round roundScript;
    [SerializeField] Transform circle, round, main;
    [SerializeField] Text ScoreText;
    [SerializeField] SpriteRenderer circleSP, roundSP;

    private int totalScoreValue = 0;
    private bool played = false;
    private bool oneTime = true;
    private int score = 0;
    private float minGrowthSpeed = 1f;
    private bool openRandomGrowth = false;
    private int interAdCounter = 0;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (oneTime)
            {
                if (Mathf.Abs((circle.localScale.x - 0.08f) - round.localScale.x) <= 0.05f)
                {
                    Accomplish();
                }
                else if (Mathf.Abs((circle.localScale.x - 0.08f) - round.localScale.x) <= 0.1f)
                {
                    Accomplish();
                }
                else if (Mathf.Abs((circle.localScale.x - 0.08f) - round.localScale.x) <= 0.15f)
                {
                    Accomplish();
                }
                else
                {
                    oneTime = false;

                    StopGrowing();
                    ShowEndText();
                    SaveScore(score);
                    EndGame();
                }
            }
        }
    }

    void Accomplish()
    {
        oneTime = false;

        score += 1;

        StopGrowing();
        ShowScore(score);
        ParticleAnim(50);
        NextLevel();
    }

    void ParticleAnim(int amount)
    {
        particles.Emit(amount);
    }

    void SaveScore(int score)
    {
        int _score = PlayerPrefs.GetInt("score");
        int oldMaxScore = PlayerPrefs.GetInt("max_score");

        PlayerPrefs.SetInt("score", score);

        if(score > oldMaxScore) PlayerPrefs.SetInt("max_score", score);
    }

    public void EndGame()
    {
        StartCoroutine(IEnd());
    }

    void NextLevel()
    {
        ResetGame();
    }

    void StopGrowing()
    {
        roundScript.Stop();
    }

    void ShowEndText()
    {
        ScoreText.gameObject.SetActive(true);
        ScoreText.text = "Last Score: " +
            "" + score;
    }

    void ShowScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    void ResetGame()
    {
        if(minGrowthSpeed < 2.5f)
        {
            minGrowthSpeed += 0.05f;
            roundScript.growthSpeed = minGrowthSpeed;
        }
        else
        {
            float growthSpeed = Random.Range(2.5f, 3f);
            roundScript.growthSpeed = growthSpeed;
        }

        float random = Random.Range(0.35f, 0.8f);

        main.localScale = new Vector3(random, random, 1f);
        round.localScale = new Vector3(0f, 0f, 1f);

        //Color newColor = new Color(0f, Random.Range(0f,0.5f), Random.Range(0.5f, 1f), 0.75f);
        //Color newColor2 = new Color(0f, Random.Range(0f, 0.5f), Random.Range(0.5f, 1f), 1.0f);

        //circleSP.color = newColor;
        //roundSP.color = newColor2;

        roundScript._Start();
        oneTime = true;
    }


    int GetNextSceneIndis()
    {
        int currentSceneIndis = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndis+1;
    }

    IEnumerator IEnd()
    {
        yield return new WaitForSeconds(0.75f);
        if (++Temp.instance.interAdCounter >= 10)
        {
            Temp.instance.interAdCounter = 0;
            GoogleAdMobController.instance.ShowInterstitialAd();
        }
        SceneManager.LoadScene("MainMenu");
    }
}
