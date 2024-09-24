using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;
        
    public float startingScore = 0;

    public TMP_Text scoreText;
    public TMP_Text highScore;

    private int scoreCount;
    private float bonusScore;

    private Camera camera;
    private float cameraStartHeight;
    private float greatestCameraHeight;

    public void Start()
    {
        camera = Camera.main;
        cameraStartHeight = camera.transform.position.y;
        Instance = this;
    }

    public void Update()
    {
        scoreCount = (int)Mathf.Floor(GetScore());
        scoreText.text = scoreCount.ToString() + " M";
        CheckHighScore();
    }

    public void AddScore(float amountToAdd)
    {
        bonusScore += amountToAdd;
    }

    public float GetScore()
    {
        return CalculateHeightScore() + bonusScore;
    }

    private float CalculateHeightScore()
    {
        float cameraHeight = camera.transform.position.y;

        if (cameraHeight > greatestCameraHeight)
        {
            greatestCameraHeight = cameraHeight;
        }

        return (greatestCameraHeight - cameraStartHeight);
    }

    void CheckHighScore()
    {
        if (scoreCount > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scoreCount);
        }
        SetHighScore();
    }

    void SetHighScore()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString() + " M";
    }
}