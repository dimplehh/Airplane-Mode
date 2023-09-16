using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UIManager;

public class LevelManager:MonoBehaviour
{
    [HideInInspector]public static LevelManager instance;
    [SerializeField] private LevelData levelData;
    float maxTime;
    [HideInInspector]public float curSpeed;
    [HideInInspector]public float maxShotDelay;

    int i;
    int level;
    float curTime;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {//Level Design
        //time = new float[10] { 5.0f, 5.0f, 10.0f, 10.0f, 15.0f, 15.0f, 15.0f, 20.0f, 20.0f, 20.0f };
        //speed = new float[10] { 2.4f, 2.7f, 3.0f, 3.3f, 3.6f, 3.9f, 4.2f, 4.5f , 4.8f, 4.8f};
        //shotDelay = new float[10] { 0.5f, 0.5f, 0.4f, 0.4f, 0.4f, 0.3f, 0.3f, 0.3f, 0.2f, 0.2f};
        maxTime = levelData.time[9]; 
        curSpeed = levelData.speed[0];
        maxShotDelay = levelData.shotDelay[0];
        level = 1;
        curTime = levelData.time[0];
    }

    void Update()
    {
        if (!GameManager.instance.startGame){ return;}
        else
        {
            if (curTime > 0) curTime -= Time.deltaTime;
            else
            {
                if (i < levelData.time.Length - 1)
                {
                    i++;
                    curSpeed = levelData.speed[i];
                    maxShotDelay = levelData.shotDelay[i];
                    curTime = levelData.time[i];
                    level++;
                }
                else
                    curTime = maxTime;
                UI.waveTxt.GetComponent<TMP_Text>().text = "Wave " + level.ToString();
                Debug.Log("·¹º§:" + level + " speed:" + curSpeed + " maxShotDelay:" + maxShotDelay);
            }
        }
    }

}
