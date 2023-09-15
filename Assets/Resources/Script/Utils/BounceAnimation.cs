using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    float time = 0;
    public float _size = 5;
    public float _upSizeTime = 0.2f;

    void Update()
    {
        if(time <= _upSizeTime)
        {
            transform.localScale = Vector3.one * (1 + _size * time);
        }
        else if(time <= _upSizeTime * 2)
        {
            transform.localScale = Vector3.one * (2 * _size * _upSizeTime + 1 - time * _size);
        }
        else
        {
            transform.localScale = Vector3.one;
            time = 0;
        }
        time += Time.deltaTime;
    }
    public void resetAnim()
    {
        time = 0;
    }
}
