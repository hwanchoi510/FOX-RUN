using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ActivatePanel(GameObject Panel)
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
        } else
        {
            Panel.SetActive(true);
        }
    }
}