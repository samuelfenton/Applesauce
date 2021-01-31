using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

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
