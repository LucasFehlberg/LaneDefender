using TMPro;
using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour
{
    private bool gameRunning = false;
    [SerializeField] private Spawner spawner;

    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;

    [SerializeField] private int highScore = 0;

    [SerializeField] private GameObject retry;

    [SerializeField] private AudioSource loseLifeSound;

    [SerializeField] private AudioSource music;

    [SerializeField] private ScrollingBackground scrollingBackground;


    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text highScoreText;

    public bool GameRunning { get => gameRunning; set => gameRunning = value; }

    public void StartGame()
    {
        scrollingBackground.ScrollSpeed = 12f;

        lives = 3;
        score = 0;
        UpdateAllText();

        music.pitch = 1f;
        GameRunning = true;
        spawner.StartSpawning();
        foreach(GameObject missile in GameObject.FindGameObjectsWithTag("Missile"))
        {
            Destroy(missile);
        }
    }

    public void Start()
    {
        LoadHighScore();
        //StartGame();
    }

    public void LoseLife()
    {
        lives--;
        loseLifeSound.Play();
        if(lives == 0)
        {
            GameRunning = false;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }

            retry.SetActive(true);
            if (score > highScore)
            {
                highScore = score;

                print(highScore);

                SaveHighScore();
            }
            music.pitch = 0.7f;
            scrollingBackground.ScrollSpeed = 0f;
        }

        UpdateAllText();
    }

    private void SaveHighScore()
    {
        highScore = score;

        SaveData data = new SaveData();
        data.highScore = highScore;

        string saveFilePath = Application.persistentDataPath + "/TankyShooter.json";

        string savedData = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, savedData);
    }

    private void LoadHighScore()
    {
        string saveFilePath = Application.persistentDataPath + "/TankyShooter.json";

        if (File.Exists(saveFilePath))
        {
            string loadedData = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(loadedData);

            highScore = data.highScore;
            //data.LoadData();
        }

        UpdateAllText();
    }

    private void UpdateAllText()
    {
        scoreText.text = "Score\n" + score.ToString();
        highScoreText.text = "High Score\n" + highScore.ToString();
        lifeText.text = "Lives\n" + lives.ToString();
    }

    public void UpdateScore(int score)
    {
        this.score += score;
        UpdateAllText();
    }
}

public class SaveData
{
    public int highScore;
}
