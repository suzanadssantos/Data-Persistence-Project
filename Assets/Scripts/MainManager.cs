using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text newScoreText;
    
    private bool m_Started = false;
    public int m_Points;
    
    private bool m_GameOver = false;

    public class UserScore{
        public static int userScore;
    }
    
    void Start()
    {   
        ScoreText.text = $"Score : {m_Points}";
        newScoreText.text = $"Best Score: {PlayerPrefs.GetString("HighScoreName", "N/A")} : {PlayerPrefs.GetInt("HighScore", 0)}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        ScoreText.text = $"{MenuManager.UserName.userName} : {m_Points}";
        SaveUserData(MenuManager.UserName.userName, m_Points);
        CheckHighScore(MenuManager.UserName.userName, m_Points);
    }

    void SaveUserData(string userName, int score){
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.SetInt("UserScore", score);
        PlayerPrefs.Save();
    }

    void CheckHighScore(string userName, int score)
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.SetString("HighScoreName", userName);
            PlayerPrefs.Save();
            newScoreText.text = $"Best Score: {userName} : {score}";
        }
    }

    public void Menu(){
        SceneManager.LoadScene(0);
    }
}