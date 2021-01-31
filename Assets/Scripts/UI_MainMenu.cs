using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public void BtnPlay()
    {
        SceneManager.LoadScene(1);
    }


    /// <summary>
    /// asdasdasdasdasd
    /// </summary>
    public void BtnQuit()

    {
        Application.Quit();
    }
}
