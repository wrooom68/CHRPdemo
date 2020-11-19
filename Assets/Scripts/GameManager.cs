using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public GameObject Player;
    public GameObject cubePrefab;

    public Material[] colorMaterials;

    int addPreviouScore = 0;
    int currentStreak = 1;

    int GameScore = 0;

    float timerCount = 0;

    //UI components
    public Text _timerText;
    public Text _scoreText;

    //Panel Manage
    public GameObject GamePanel;
    public GameObject StartPanel;
    public GameObject streakPrefab;

    public static int gameStatus = 0;

    int totalObjectToSpawn = 10;
    int objectSpawnedCount = 0;


    public Text highScore;
    public Text yourScore;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        gameStatus = 0;
        StartPanel.SetActive(true);
        GamePanel.SetActive(false);

        highScore.text = "High Score : " + PlayerPrefs.GetInt("highscore").ToString();
        yourScore.text = "Your Score : 00";
    }

    public void StartTheGame()
    {
        Reset();
        StartPanel.SetActive(false);
        GamePanel.SetActive(true);
        StartInstantiation();

        _scoreText.text = "Score : 00";
    }

    // To reset the values of the game
    void Reset()
    {
        addPreviouScore = 0;
        currentStreak = 1;
        GameScore = 0;
        timerCount = 60;
        gameStatus = 1;
        objectSpawnedCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStatus == 1)
        {
            if(timerCount > 0)
            {
                timerCount -= Time.deltaTime;
                _timerText.text = "Timer : " + Mathf.Round(timerCount).ToString();
            }
            else
            {
                gameStatus = 0;
                GameOver();
            }
        }
        else if (gameStatus == 2)
            {
            if (timerCount > 0)
            {
                if(timerCount % 3 == 0)
                {
                    GameScore++;
                    yourScore.text = "Your Score : " + GameScore.ToString();
                }
                timerCount -= 1.0f;
            }
            else
            {
                gameStatus = 0;
                if (GameScore > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", GameScore);
                }
                highScore.text = "High Score : " + PlayerPrefs.GetInt("highscore").ToString();
            }
            Debug.Log("meCalled2" + timerCount);
        }
        //else if(timerCount <= timerCount)
        //{
        //    gameStatus = 3;
        //    GameOver();
        //    Debug.Log("meCalledddd" + timerCount);
        //}
    }

    public void GetScore(Material mat)
    {
        int addScore = 0;
        if (mat.color == colorMaterials[0].color)
        {
            addScore = 20;
            Debug.Log(addScore);
        }
        else if (mat.color == colorMaterials[1].color)
        {
            addScore = 15;
            Debug.Log(addScore);
        }

        if (addScore == addPreviouScore)
        {
            currentStreak++;
            ShowStreakAnimation(currentStreak,mat.color);
        }
        else
        {
            addPreviouScore = addScore;
            currentStreak = 1;
        }
        GameScore += (addScore * currentStreak);
        _scoreText.text = "Score : " + GameScore.ToString();

        Invoke("StartInstantiation",1f);
    }

    void ShowStreakAnimation(int count , Color col)
    {
        GameObject spawnObject = Instantiate(streakPrefab, GamePanel.transform.parent);
        spawnObject.GetComponentInChildren<Text>().text = addPreviouScore.ToString() + " X " + currentStreak;
        spawnObject.GetComponentInChildren<Text>().color = col;
        Destroy(spawnObject, 2f);
    }

    public void StartInstantiation()
    {
        if(objectSpawnedCount < totalObjectToSpawn)
        {
            objectSpawnedCount++;

            bool objectSpawned = false;

            while (!objectSpawned)
            {
                Vector3 objectPos = new Vector3(Random.Range(-4f, 4f), -1, Random.Range(-3.5f, 3.5f));
                if (Vector3.Distance(objectPos, Player.transform.position) < 2)
                {
                    continue;
                }
                else
                {
                    GameObject spawnObject = Instantiate(cubePrefab, objectPos, Quaternion.identity);
                    spawnObject.GetComponent<MeshRenderer>().material = colorMaterials[Random.Range(0, colorMaterials.Length)];
                    objectSpawned = true;
                }
            }
        }
        else
        {
            gameStatus = 2;
            timerCount = Mathf.Round(timerCount) * 3;
            GameOver();
        }
    }

    void GameOver()
    {
        if (GameScore > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", GameScore);
        }
        StartPanel.SetActive(true);
        GamePanel.SetActive(false);

        highScore.text = "High Score : " + PlayerPrefs.GetInt("highscore").ToString();
        yourScore.text = "Your Score : "+ GameScore.ToString();
    }
}
