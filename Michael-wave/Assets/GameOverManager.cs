using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public TextMeshProUGUI gameOverText;
    private bool gameOver = false;

    void Awake()
    {
        Instance = this;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // hidden until game over
        }
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TriggerGameOver()
    {
        gameOver = true;
        Debug.Log("Game Over — press R to restart");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Overheated. Press 'R' to restart.";
        }
    }
}