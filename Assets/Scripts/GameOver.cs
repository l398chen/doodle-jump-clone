using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text pointsText;
    // Start is called before the first frame update
    public void Setup()
    {
        gameObject.SetActive(true);
        pointsText.text = "Flew " + scoreText.text + "eters";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("TheGame");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
