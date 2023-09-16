using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UIManager;
using static ObjectPooler;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public static LevelManager instance;
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform[] spawnPoints;
    float maxTime;
    [HideInInspector] public float curSpeed;
    [HideInInspector] public float maxShotDelay;

    int i;
    int level;
    float curTime;
    float itemTime;

    int realPos;
    int realItem;
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

    void FixedUpdate()
    {
        if (!GameManager.instance.startGame) { return; }
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
                itemTime = 5f;
            }
        }
    }

    IEnumerator ShowItems()
    {
        GameObject item;
        Debug.Log("realPos : " + realPos + "realItem:" + realItem);
        realPos = ((int)curTime * 7) % 6;
        realItem = ((int)curTime * 11) % 2; //난수인것같은 숫자 생성
        Transform randPos = spawnPoints[realPos];
        item = (realItem == 0)
            ?OP.PoolInstantiate("Prefabs/Star", randPos.position, randPos.rotation)
            : OP.PoolInstantiate("Prefabs/Heart", randPos.position, randPos.rotation);
        yield return new WaitForSeconds(1.5f);
        if(item != null) OP.PoolDestroy(item);
    }
}
