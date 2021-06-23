using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public TMP_Text text;
    public Animator fadescreen;
    public AudioSource source;
    public AudioClip clip;

    private void Awake()
    {
        DontDestroyOnLoad(source);
        DontDestroyOnLoad(this.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && SceneManager.GetActiveScene().buildIndex==0)
        {
            StartCoroutine(LoadMainMenu());
        }
    }

    IEnumerator LoadMainMenu()
    {
        text.gameObject.SetActive(false);
        source.PlayOneShot(clip);
        fadescreen.Play("FadeToBlack", 0);
        yield return new WaitForSecondsRealtime(4f);
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
