using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UIManager;
using static ObjectPooler;

public class LevelManager:MonoBehaviour
{
    [HideInInspector]public static LevelManager instance;
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform[] spawnPoints;
    float maxTime;
    [HideInInspector]public float curSpeed;
    [HideInInspector]public float maxShotDelay;

    int i;
    int level;
    float curTime;
    float itemTime;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        maxTime = levelData.time[9]; 
        curSpeed = levelData.speed[0];
        maxShotDelay = levelData.shotDelay[0];
        level = 1;
        curTime = levelData.time[0];
        itemTime = 10f;
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
                Debug.Log("레벨:" + level + " speed:" + curSpeed + " maxShotDelay:" + maxShotDelay);
            }

            //아이템 등장
            if (itemTime > 0) itemTime -= Time.deltaTime;
            else
            {
                StartCoroutine("ShowItems");
                itemTime = 10f;
            }
        }
    }

    IEnumerator ShowItems()
    {
        GameObject item;
        Transform randPos = spawnPoints[Random.Range(0, 6)];

        int randomItem = Random.Range(0, 2);
        item = (randomItem == 0)
            ?OP.PoolInstantiate("Prefabs/Bigger", randPos.position, randPos.rotation)
            : OP.PoolInstantiate("Prefabs/Heart", randPos.position, randPos.rotation);

        yield return new WaitForSeconds(1.5f);
        OP.PoolDestroy(item);
    }
}
