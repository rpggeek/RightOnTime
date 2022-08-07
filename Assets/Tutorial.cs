using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject parmak, eventSystem, text;
    [SerializeField] GameObject firstCircle, MidCircle, LastCirle;
    [SerializeField] ParticleSystem particles;
    private Animator anim;
    private bool _lock = true;
    private int touchCounter = 0;
    private bool oneTime = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_lock)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lock = true;

                touchCounter++;

                if (touchCounter < 2)
                {
                    Stages();
                }else if(touchCounter >=2 && touchCounter <= 4)
                {
                    if (oneTime)
                    {
                        oneTime = false;
                        Stage3();
                    }
                    else if(touchCounter != 4)
                    {
                        Stage4();
                    }
                    else
                    {
                        Stage5();
                    }
                }
                else if(touchCounter > 4 && touchCounter <= 6)
                {
                    
                    if(touchCounter == 6)
                    {
                        GetComponent<SpriteRenderer>().enabled = false;
                        ParticleAnim(50);
                        parmak.SetActive(false);
                        LastCirle.GetComponent<SpriteRenderer>().enabled = false;

                        PlayerPrefs.SetInt("tutorial", 1);
                        StartCoroutine(StartGame());
                    }
                    else
                    {
                        Stage6();
                    }
                }
            }
        }
    }

    private void Stages()
    {
        //PlayerPrefs.SetInt("tutorial", 1);
        GetComponent<SpriteRenderer>().enabled = false;
        parmak.SetActive(false);
        text.SetActive(false);
        ParticleAnim(50);

        Stage2();
    }

    private void Stage2()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        anim.Play("CircleTutorial", -1,0f);
    }

    public void Mid()
    {
        _lock = false;
        parmak.SetActive(true);
    }
    public void Last()
    {
        _lock = false;
        parmak.SetActive(true);
    }

    private void Stage3()
    {
        firstCircle.GetComponent<SpriteRenderer>().enabled = false;
        firstCircle.GetComponent<TutorialRotation>().enabled = false;
        //GetComponent<SpriteRenderer>().enabled = false;
        parmak.SetActive(false);
        text.SetActive(false);
        ParticleAnim(50);

        anim.SetBool("mid", true);
    }

    private void Stage4()
    {
        //GetComponent<SpriteRenderer>().enabled = false;
        parmak.SetActive(false);
        ParticleAnim(50);
        anim.Play("CircleTutorial2", -1, 0f);
    }

    private void Stage5()
    {
        parmak.SetActive(false);
        MidCircle.GetComponent<SpriteRenderer>().enabled = false;
        MidCircle.GetComponent<TutorialRotation>().enabled = false;
        ParticleAnim(50);
        anim.SetBool("last", true);
    }

    private void Stage6()
    {
        parmak.SetActive(false);
        ParticleAnim(50);
        anim.Play("CircleTutorial3", -1, 0f);
    }

    public void ShowHandAnim()
    {
        parmak.SetActive(true);
        eventSystem.SetActive(true);
        text.SetActive(true);
        _lock = false;
    }

    private void ParticleAnim(int amount)
    {
        particles.Emit(amount);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level");
    }
}
