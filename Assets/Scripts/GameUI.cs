using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private Text ScoreText;
    [HideInInspector] public static int Score;

    void Start()
    {
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
    }
}
