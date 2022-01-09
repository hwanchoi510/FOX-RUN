using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    [SerializeField] AudioSource Sound;
    void Start()
    {
        Text FinalScoreText = GameObject.Find("ScoreNumber").GetComponent<Text>();
        FinalScoreText.text = PlayerPrefs.GetInt("Score").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(PlaySound(Sound, "MainMenu"));
        }
    }

    private IEnumerator PlaySound(AudioSource sound, string Name)
    {
        sound.Play();
        yield return new WaitForSeconds(sound.clip.length);
        SceneManager.LoadScene(Name);
    }
}
