using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Script : MonoBehaviour
{
    public AudioClip clip;
    public Animator fadescreen;
    public MainMenuScript menuScript;

    private void Start()
    {
        menuScript = GameObject.FindObjectOfType<MainMenuScript>();
    }

    public void OnNewGameClick()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
    }
    public void OnOptionClick()
    {

    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int index)
    {
        menuScript.source.PlayOneShot(clip);
        fadescreen.Play("FadeToBlack", 0);
        yield return new WaitForSecondsRealtime(4f);
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
