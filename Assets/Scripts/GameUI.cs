using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private AudioSource Sound;

    public static bool isPaused;
    private Text ScoreText;

    [HideInInspector] public static int Score;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        isPaused = false;
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
        
    }

    private void Pause()
    {
        Sound.Play();
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Sound.Play();
        PauseMenuUI.SetActive(false);
        Time.timeScale = PlayerControl.scale;
        isPaused = false;
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(PlaySound(Sound, "MainMenu"));
    }

    private IEnumerator PlaySound(AudioSource sound, string SceneName)
    {
        sound.Play();
        yield return new WaitForSeconds(sound.clip.length);
        SceneManager.LoadScene(SceneName);
    }
}
