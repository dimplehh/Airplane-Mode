using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
            return;
        else
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
