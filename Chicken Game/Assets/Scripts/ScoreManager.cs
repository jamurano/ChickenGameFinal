using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public static int score;
    public int winScore;
    public Text text;

    public Text winText;

    void Awake()
    {
        Time.timeScale = 1;
    }
  
        void Start()
        {
            //winText.GetComponent<Text>().enabled = false;
            text = GetComponent<Text>();
            score = 0;

        }

        void Update()
        {
            if (score < 0)
                score = 0;
        
            text.text =" " + score;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

        }

        public static void AddPoints(int pointsToAdd)
        {
            score += pointsToAdd;
        }

        public void Reset()
        {
            score = 0;
        }

        public static void SubtractPoints(int pointsToSubtract)
        {
            score -= pointsToSubtract;
        }
    }
