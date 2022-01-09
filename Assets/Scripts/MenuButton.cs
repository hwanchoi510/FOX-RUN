using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] AudioSource Sound;
    public void ChangeScene(string SceneName)
    {
        StartCoroutine(PlaySound(Sound, SceneName));
    }

    public void ExitGame()
    {
        Sound.Play();
        Application.Quit();
    }

    public void ActivatePanel(GameObject Panel)
    {
        Sound.Play();
        if(Panel.activeSelf)
        {
            Panel.SetActive(false);
        } else
        {
            Panel.SetActive(true);
        }
    }

    private IEnumerator PlaySound(AudioSource sound, string Name)
    {
        sound.Play();
        yield return new WaitForSeconds(sound.clip.length);
        SceneManager.LoadScene(Name);
    }
}