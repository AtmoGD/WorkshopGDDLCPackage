using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Text gameOverScreenText;
    [SerializeField] private Text timer;
    [SerializeField] private Text actualScore;
    [SerializeField] private Text highScore;
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private RectTransform lifesGrid;
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private Color lifeFullColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    [SerializeField] private Color lifeEmptyColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    private PlayerController player;
    private List<Image> hearts = new List<Image>();

    bool gameEnded = false;
    float time = 0f;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();

        if (player) InitHearts();
    }

    private void InitHearts()
    {
        for (int i = 0; i < player.GetMaxLives(); i++)
        {
            GameObject heart = Instantiate(lifePrefab, lifesGrid);
            Image heartImage = heart.GetComponent<Image>();
            heartImage.color = lifeFullColor;
            hearts.Add(heartImage);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            PlayerPrefs.SetFloat("HighScore", 0f);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if(gameEnded) return;

        time += Time.deltaTime;

        if (player)
            UpdateHearts();

        timer.text = "Time: " + time.ToString("0.00");
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
            hearts[i].color = i < player.GetLives() ? lifeFullColor : lifeEmptyColor;
    }

    public void EndGame()
    {
        UpdateHearts();
        gameEnded = true;
    }
    public void GameOver()
    {
        if(gameEnded) return;

        scoreScreen.SetActive(false);
        anim.SetTrigger("OverlayFadeInLost");
        gameOverScreenText.text = "Game Over!";
        EndGame();
    }

    public void GameWon()
    {
        if(gameEnded) return;

        scoreScreen.SetActive(true);
        anim.SetTrigger("OverlayFadeInWon");

        float highScoreValue = PlayerPrefs.GetFloat("HighScore", 0f);

        if((time < highScoreValue) || (highScoreValue == 0f)) {
            PlayerPrefs.SetFloat("HighScore", time);
            highScoreValue = time;
        } 
            
        actualScore.text = time.ToString("0.00");
        highScore.text = highScoreValue.ToString("0.00");

        gameOverScreenText.text = "You won!";
        EndGame();
    }

    public void RestartScene()
    {
        anim.SetTrigger("ButtonsFadeOut");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
