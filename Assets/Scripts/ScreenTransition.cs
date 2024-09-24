using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public Animator transition;
    public float transition_time = 1f;
    bool startTransition = false;

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         LoadNextScene();
    //     }
    // }

    public void LoadNextScene()
    {
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().buildIndex + 1)); // loads next scene in the build list
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
