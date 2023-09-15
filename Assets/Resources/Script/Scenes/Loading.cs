using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Loading : MonoBehaviour
{
    public Slider progressbar;
    public TMP_Text loadingText;
    public Image plane1, plane2, plane3, plane4;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("SampleScene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime * 0.7f);
                if (progressbar.value < 0.3) loadingText.text = "로딩중.";
                else if (progressbar.value >= 0.3 && progressbar.value < 0.6) loadingText.text = "로딩중..";
                else loadingText.text = "로딩중...";

                if (progressbar.value > 0.1)
                    plane1.gameObject.SetActive(true);
                if (progressbar.value > 0.3)
                    plane2.gameObject.SetActive(true);
                if (progressbar.value > 0.5)
                    plane3.gameObject.SetActive(true);
                if (progressbar.value > 0.7)
                    plane4.gameObject.SetActive(true);


            }
            else if (progressbar.value >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if (progressbar.value >= 1f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
