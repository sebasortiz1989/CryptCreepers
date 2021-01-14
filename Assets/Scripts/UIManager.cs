using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

    [SerializeField] Text finalScoreText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] AudioClip buttonPressed;
    [SerializeField] AudioClip gameOverSound;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else
            Destroy(gameObject);
    }

    public void ShowGameOverScreen()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        finalScoreText.text = "Score: " + GameManager.sharedInstance.score;
        AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, 0.3f);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonPressed, Camera.main.transform.position, 0.3f);
        Invoke("LoadGameScene", 0.5f);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
