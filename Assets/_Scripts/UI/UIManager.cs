using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// Camera only for boot screen.
    /// </summary>
    [Tooltip("Camera only for boot screen")]
    public Camera _dummyCamera;

    /// <summary>
    /// GameObject that group Title screen objects.
    /// </summary>
    [Tooltip("GameObject that group Title screen objects")]
    public GameObject _titleMenu;

    /// <summary>
    /// GameObject that group in game screen objects.
    /// </summary>
    [Tooltip("GameObject that group in game screen objects")]
    public GameObject _inGameUI;

    /// <summary>
    /// GameObject that group game over screen objects.
    /// </summary>
    [Tooltip("GameObject that group game over screen objects")]
    public GameObject _gameOverUI;

    /// <summary>
    /// In game score text.
    /// </summary>
    [Tooltip("In game score text")]
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// In game level text.
    /// </summary>
    [Tooltip("In game level text")]
    public TextMeshProUGUI levelText;

    /// <summary>
    /// Title max score text.
    /// </summary>
    [Tooltip("Title max score text")]
    public TextMeshProUGUI maxScoreText;

    /// <summary>
    /// Game over score text.
    /// </summary>
    [Tooltip("Game over score text")]
    public TextMeshProUGUI scoreGOText;

    /// <summary>
    /// Game over max score text.
    /// </summary>
    [Tooltip("Game over max score text")]
    public TextMeshProUGUI maxScoreGOText;

    /// <summary>
    /// List of lives representation objects.
    /// </summary>
    [Tooltip("List of lives representation objects")]
    public List<GameObject> lives;

    // Start method.
    // Add listener to game state event.
    void Start()
    {
        GameManager.Instance.eventGameStateChanged.AddListener(HandleGameStateChanged);
    }

    // Update method.
    // Catch click on game over screen.
    void Update()
    {
        if(GameManager.Instance.currentGameState == GameManager.GameState.gameOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.eventExitGameOver.Invoke();
            }
        }
    }

    /// <summary>
    /// Enable or disable dummy camera.
    /// </summary>
    /// <param name="active">Bool for set active method, true enable, false disable</param>
    private void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    /// <summary>
    /// Enable or disable Title screen UI.
    /// </summary>
    /// <param name="active">Bool for set active method, true enable, false disable</param>
    private void SetTitleMenuActive(bool active)
    {
        _titleMenu.gameObject.SetActive(active);
    }

    /// <summary>
    /// Enable or disable in game UI.
    /// </summary>
    /// <param name="active">Bool for set active method, true enable, false disable</param>
    private void SetInGameUIActive(bool active)
    {
        _inGameUI.gameObject.SetActive(active);
    }

    /// <summary>
    /// Enable or disable game over screen UI.
    /// </summary>
    /// <param name="active">Bool for set active method, true enable, false disable</param>
    private void SetGameOverUIActive(bool active)
    {
        _gameOverUI.gameObject.SetActive(active);
    }

    /// <summary>
    /// Call methods to enable or disable UI components according to game state changes.
    /// </summary>
    /// <param name="currentState">New game state</param>
    /// <param name="previousStat">previous game state</param>
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousStat)
    {
        SetDummyCameraActive(currentState == GameManager.GameState.boot || currentState == GameManager.GameState.gameOver);

        SetTitleMenuActive(currentState == GameManager.GameState.boot);

        SetInGameUIActive(currentState == GameManager.GameState.inGame);

        SetGameOverUIActive(currentState == GameManager.GameState.gameOver);
    }

    /// <summary>
    /// Write in title max score text.
    /// </summary>
    /// <param name="maxScore">int to write</param>
    public void WriteMaxScore(int maxScore)
    {
        UIManager.Instance.maxScoreText.text = "Max Score: \n" + maxScore;
    }

    /// <summary>
    /// Write in InGame level text.
    /// </summary>
    /// <param name="level">int to write</param>
    public void WriteLevel(int level)
    {
        UIManager.Instance.levelText.text = "Level: " + level;
    }

    /// <summary>
    /// Write in InGame score text.
    /// </summary>
    /// <param name="score">int to write</param>
    public void WriteScore(int score)
    {
        UIManager.Instance.scoreText.text = "Score: " + score;
    }      

    /// <summary>
    /// Make transparent the image of a life.
    /// </summary>
    /// <param name="numberOfLives">int, position of the life to make transparent</param>
    public void SpentLife(int numberOfLives)
    {
        Image lifeImage = UIManager.Instance.lives[numberOfLives].GetComponent<Image>();
        var tempColor = lifeImage.color;
        tempColor.a = 0.3f;
        lifeImage.color = tempColor;
    }

    /// <summary>
    /// Remove transparency of the image of a life.
    /// </summary>
    /// <param name="numberOfLives">int, position of the life to remove transparency</param>
    public void AddLife(int numberOfLives)
    {
        Image lifeImage = UIManager.Instance.lives[numberOfLives].GetComponent<Image>();
        var tempColor = lifeImage.color;
        tempColor.a = 1f;
        lifeImage.color = tempColor;
    }

    /// <summary>
    /// Restart transparency of the lives images.
    /// </summary>
    public void restartLives()
    {
        foreach (GameObject live in lives)
        {
            Image lifeImage = live.GetComponent<Image>();
            var tempColor = lifeImage.color;
            tempColor.a = 1.0f;
            lifeImage.color = tempColor;
        }        
    }

    /// <summary>
    /// Write in game over score text.
    /// </summary>
    /// <param name="score">int to write</param>
    public void WriteScoreGO(int score)
    {
        UIManager.Instance.scoreGOText.text = "Score: " + score;
    }

    /// <summary>
    /// Write in game over max score text.
    /// </summary>
    /// <param name="maxScore">int to write</param>
    public void WriteMaxScoreGO(int maxScore)
    {
        UIManager.Instance.maxScoreGOText.text = "Max Score: \n" + maxScore;
    }
}
