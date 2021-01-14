using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public int time = 30;
    public int difficulty = 1;
    [SerializeField] Text timeText;
    [SerializeField] Text scoreText;
    
    public int score;

    public int Score
    {
        get { return score; }
        set { score = value; 
            scoreText.text = "Score: " + score.ToString();
            if (score % 1000 == 0)
                difficulty++;  
        }
    }

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownRoutine());
        timeText.text = string.Format("Time: {0}", time);
    }

    IEnumerator CountDownRoutine()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
            timeText.GetComponent<Text>().text = string.Format("Time: {0}", time);
        }

        UIManager.sharedInstance.ShowGameOverScreen();
    }
}
