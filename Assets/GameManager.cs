using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GameOver { get; private set; }
    public float GameTime { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (GameOver) return;

        GameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void EndGame(bool win)
    {
        if (GameOver) return;

        GameOver = true;

        if (win)
        {
            Debug.Log("You Win!");
            UIManager.Instance.ShowWin(GameTime);
        }
        else
        {
            Debug.Log("Game Over!");
            UIManager.Instance.ShowGameOver(GameTime);
        }

        Invoke(nameof(RestartGame), 2f);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

UIManager.cs
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI gameStatusText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameOver && timerText != null)
        {
            timerText.text = "Time: " + Mathf.FloorToInt(GameManager.Instance.GameTime) + "s";
        }
    }

    public void ShowGameOver(float time)
    {
        if (gameStatusText != null)
            gameStatusText.text = "GAME OVER!";
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void ShowWin(float time)
    {
        if (gameStatusText != null)
            gameStatusText.text = "YOU WIN!";
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}

Player.cs
using UnityEngine;

public class Player : MonoBehaviour
{
    private int score = 0;
    public int Score => score;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore(10);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        if (score >= 30)
        {
            GameManager.Instance.EndGame(win: true);
        }
    }
}
