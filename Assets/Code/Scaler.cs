using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    private float referenceScreenRatio = 1080f / 1920f;

    void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x + (Mathf.Abs(getScreenRatio()) - Mathf.Abs(referenceScreenRatio)), transform.localScale.y + (Mathf.Abs(getScreenRatio()) - Mathf.Abs(referenceScreenRatio)), transform.localScale.z);
    }

    private float getScreenRatio()
    {
        return ((float)Screen.width / (float)Screen.height);
    }
}
