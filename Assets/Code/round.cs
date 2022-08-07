using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round : MonoBehaviour
{
    [SerializeField] Motor motorScript;
    [SerializeField] Transform mainObject;
    public float growthSpeed, maximumGrowthScale, timeLater;
    private float timeLaterTemp;
    bool _end = false;

    void Start()
    {
        float random = Random.Range(0.5f, 0.85f);

        mainObject.localScale = new Vector3(random, random, 1f);
        growthSpeed = 0.9f;
        transform.localScale = new Vector3(0f, 0f, 0f);

        timeLaterTemp = timeLater;
    }

    void Update()
    {
        if (!_end)
        {
            if (timeLater > 0f) timeLater -= Time.deltaTime;

            if (timeLater < 0f)
            {
                if (transform.localScale.x < maximumGrowthScale)
                {
                    transform.localScale += new Vector3(growthSpeed * Time.deltaTime, growthSpeed * Time.deltaTime, 0f);
                }
                else
                {
                    _end = true;
                    motorScript.EndGame();
                }
            }
        }
    }

    public void Stop()
    {
        _end = true;
        timeLater = timeLaterTemp;
    }
    public void _Start()
    {
        _end = false;
    }
}
