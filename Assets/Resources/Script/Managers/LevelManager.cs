using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager:MonoBehaviour
{
    public static LevelManager instance;
    float[] time;
    float[] speed;
    float[] shotDelay;
    float maxTime;
    public float curSpeed;
    public float maxShotDelay;
    [SerializeField]
    GameObject waveText; 

    int i;
    int level;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {//Level Design
        time = new float[10] { 5.0f, 5.0f, 10.0f, 10.0f, 15.0f, 15.0f, 15.0f, 20.0f, 20.0f, 20.0f };
        speed = new float[10] { 2.4f, 2.7f, 3.0f, 3.3f, 3.6f, 3.9f, 4.2f, 4.5f , 4.8f, 4.8f};
        shotDelay = new float[10] { 0.5f, 0.5f, 0.4f, 0.4f, 0.4f, 0.3f, 0.3f, 0.3f, 0.2f, 0.2f};
        maxTime = time[9]; 
        curSpeed = speed[0];
        maxShotDelay = shotDelay[0];
        level = 1;
    }

    void Update()
    {
        if (!GameManager.instance.startGame){ waveText.SetActive(false); return;}
        else
        {
            if (waveText.activeSelf == false)
                waveText.SetActive(true);
            if (time[i] > 0)
                time[i] -= Time.deltaTime;
            else
            {
                if (i < time.Length - 1)
                {
                    i++;
                    curSpeed = speed[i];
                    maxShotDelay = shotDelay[i];
                    level++;
                }
                else
                {
                    time[i] = maxTime;
                }
                waveText.GetComponent<TMP_Text>().text = "Wave " + level.ToString();
                Debug.Log("·¹º§:" + level + " speed:" + curSpeed + " maxShotDelay:" + maxShotDelay);
            }
        }
    }

}
