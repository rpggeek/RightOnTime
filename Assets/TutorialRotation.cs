using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRotation : MonoBehaviour
{
    [SerializeField] bool left_or_right = false;
    void Update()
    {
        if (left_or_right)
        {
            transform.Rotate(new Vector3(0, 0, 25f*Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -25f * Time.deltaTime));
        }
    }
}
