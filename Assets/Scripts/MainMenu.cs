using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transition_time = 1f;
    public void StartGame()
    {
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().buildIndex + 1)); // loads next scene in the build list
    }

    public void QuitGame()
    {
        Application.Quit(); // quits application
    }

    IEnumerator SceneTransition(int LevelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transition_time);

        //Load scene
        SceneManager.LoadScene(LevelIndex);
    }

}
