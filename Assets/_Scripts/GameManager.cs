using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Player pref for Max score.
    /// </summary>
    private const string MAX_SCORE = "MAX_SCORE";

    /// <summary>
    /// Game states enumeration.
    /// </summary>
    public enum GameState
    {
        boot,
        inGame,
        pause,
        gameOver
    }

    /// <summary>
    /// Current state of game.
    /// </summary>
    private GameState _currentGameState;

    /// <summary>
    /// Allow access to current game valiable.
    /// </summary>
    public GameState currentGameState
    {
        get
        {
            return _currentGameState;
        }
        set
        {
            _currentGameState = GameState.boot;
        }
    }

    /// <summary>
    /// Previous game state.
    /// </summary>
    private GameState _previousGameState;
    
    /// <summary>
    /// Collection of systems that GameManager has to start and control.
    /// </summary>
    [SerializeField, Tooltip("Collection of systems that GameManager has to start and control")]
    private GameObject[] SystemPrefabs;

    /// <summary>
    /// List of instantiate systems.
    /// </summary>
    private List<GameObject> _instantiateSystemPrefabs;

    /// <summary>
    /// List of load operations.
    /// </summary>
    List<AsyncOperation> _loadOperations;

    /// <summary>
    /// Prefab of the ball.
    /// </summary>
    [Tooltip("Prefab of the ball")]
    public GameObject ballPrefab;

    /// <summary>
    /// Collection of the balls in game.
    /// </summary>
    private List<GameObject> balls = new List<GameObject>();
    
    /// <summary>
    /// Current level of the game.
    /// </summary>
    private int _currentLevel;

    /// <summary>
    /// Allows access to current level value.
    /// </summary>
    private int currentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            _currentLevel = Mathf.Clamp(value, 0, 999);
        }
    }

    /// <summary>
    /// Score of the player.
    /// </summary>
    private int _score;

    /// <summary>
    /// Allows access to score value.
    /// </summary>
    private int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = Mathf.Clamp(value, 0, 999999);
        }
    }

    /// <summary>
    /// Number of lives to play.
    /// </summary>
    private int numberOfLives = 3;

    //Events
    public Events.EventStartGame eventStartGame;
    public Events.EventGameStateChanged eventGameStateChanged;
    public Events.EventBlockDestroyed eventBlockDestroyed;
    public Events.EventBallDestroyed eventBallDestroyed;
    public Events.EventExitGameOver eventExitGameOver;
    public Events.EventPowerUpBulldozer eventPowerUpBulldozer;
    public Events.EventPowerAddBall eventPowerUpAddBall;
    public Events.EventPowerAddLife eventPowerUpAddLive;
    
    // Start method.
    // Instantiate System Prefabs, show max score from player prefabs and create listener for the events.
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _instantiateSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();
        InstantiateSystemPrefabs();

        ShowMaxScore();

        eventStartGame.AddListener(StartGame);
        
        eventBlockDestroyed.AddListener(BrickDestroyed);
        eventBallDestroyed.AddListener(BallDestroyed);
        eventExitGameOver.AddListener(RestartGame);

        eventPowerUpAddBall.AddListener(AddBall);
        eventPowerUpAddLive.AddListener(AddLife);
    }

    /// <summary>
    /// Generate an instance of each system inside System prefabs list.
    /// </summary>
    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instantiateSystemPrefabs.Add(prefabInstance);
        }
    }

    /// <summary>
    /// Launch coroutine that start game.
    /// </summary>
    private void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    /// <summary>
    /// Generate a new level, update the score to 0, set number of lives to 3 and load Main scene and wait until the load finish to change the game state and create a new ball.
    /// </summary>
    private IEnumerator StartGameCoroutine()
    {
        GenerateLevel();
        UpdateScore(0);
        numberOfLives = 3;

        AsyncOperation ao = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);

        while (!ao.isDone)
        {            
            yield return null;
        }
        UpdateGameState(GameState.inGame);        

        CreateBall();
        yield break;
    }

    /// <summary>
    /// Unload a level.
    /// </summary>
    /// <param name="levelName">level name to unload</param>
    private void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogErrorFormat("[GameManager] Unable to unload level {0}", levelName);
            return;
        }
        //ao.completed += OnUnloadOperationComplete;
    }


    //private void OnUnloadOperationComplete(AsyncOperation ao)
    //{

    //}

    /// <summary>
    /// Increase level of the game and signal Creator level to generate a level.
    /// </summary>
    private void GenerateLevel()
    {
        UpdateLevel(1);
        CreatorLevel.Instance.CreateLevel(currentLevel);
    }

    /// <summary>
    /// Update game state and set time scale in relation, invoke game state changed event.
    /// </summary>
    /// <param name="state">new game state</param>
    private void UpdateGameState(GameState state)
    {
        _previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.boot:
                Time.timeScale = 1.0f;
                break;
            case GameState.inGame:
                Time.timeScale = 1.0f;
                break;
            case GameState.pause:
                Time.timeScale = 0.0f;
                break;
            case GameState.gameOver:
                Time.timeScale = 0.0f;
                break;
            default:
                Time.timeScale = 1.0f;
                break;
        }

        eventGameStateChanged.Invoke(_currentGameState, _previousGameState);
    }

    /// <summary>
    /// Increase the current level in 1.
    /// </summary>
    private void UpdateLevel()
    {
        currentLevel += 1;
    }

    /// <summary>
    /// Increase the current level in a specific amount.
    /// </summary>
    /// <param name="levelToAdd">Int amount of increment</param>
    private void UpdateLevel(int levelToAdd)
    {
        currentLevel += levelToAdd;
        UIManager.Instance.WriteLevel(currentLevel);
    }

    /// <summary>
    /// Return current level.
    /// </summary>
    /// <returns>Int current level</returns>
    public int GetLevel()
    {
        return currentLevel;
    }

    /// <summary>
    /// Add points to score and update textbox.
    /// </summary>
    /// <param name="scoreToAdd">Int points to add</param>
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIManager.Instance.WriteScore(score);
    }
    
    /// <summary>
    /// Recover max score from player prefs and show it in textbox.
    /// </summary>
    private void ShowMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE, 0);
        UIManager.Instance.WriteMaxScore(maxScore);
    }

    /// <summary>
    /// Save max score in player prefs.
    /// </summary>
    private void SetMaxScore()
    {
        int maxScore = PlayerPrefs.GetInt(MAX_SCORE, 0);
        if (maxScore < score)
        {
            PlayerPrefs.SetInt(MAX_SCORE, score);
        }
    }

    /// <summary>
    /// If aren't in game over state call power up spawner to generate a new power up, signal creator level about the destroy of a brick,
    /// update score and check if is the last brick.
    /// </summary>
    /// <param name="brick">Brick destroyed</param>
    /// <param name="pointValue">Int points for destoy it</param>
    private void BrickDestroyed(GameObject brick, int pointValue)
    {
        if(_currentGameState != GameState.gameOver)
        {
            PowerUpSpawner.Instance.GeneratePowerUp(brick.transform.position);
            CreatorLevel.Instance.EliminateBrick(brick);
            UpdateScore(pointValue);
            CheckUndestroyedBricks();
        }        
    }

    /// <summary>
    /// Check if remains bricks in game.
    /// </summary>
    private void CheckUndestroyedBricks()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        if(bricks.Length == 0)
        {
            GenerateLevel();
        }
    }

    /// <summary>
    /// Restores a life and remove transparency from image.
    /// </summary>
    private void AddLife()
    {
        if (numberOfLives != 3)
        {
            numberOfLives++;
            UIManager.Instance.AddLife(numberOfLives - 1);
        }
    }

    /// <summary>
    /// Remove ball from list, if is last ball rid a life.
    /// If have more lives create a new ball, else set max score and go to game over.
    /// </summary>
    /// <param name="ball">Ball destroyed</param>
    private void BallDestroyed(GameObject ball)
    {        
        balls.Remove(ball);

        if (balls.Count == 0)
        {
            numberOfLives--;

            if (numberOfLives >= 0)
            {
                UIManager.Instance.SpentLife(numberOfLives);

                CreateBall();
            }

            if (numberOfLives <= 0)
            {
                SetMaxScore();

                UpdateGameState(GameState.gameOver);
                CreatorLevel.Instance.ClearLevel();
                EliminatedBalls();
                int maxScore = PlayerPrefs.GetInt(MAX_SCORE, 0);
                UIManager.Instance.WriteMaxScoreGO(maxScore);
                UIManager.Instance.WriteScoreGO(score);
                UnloadLevel("MainScene");
            }
        }        
    }

    /// <summary>
    /// Check gameobject tag and call to generate a new ball.
    /// </summary>
    /// <param name="gameObject">Power up gameobject</param>
    private void AddBall(GameObject gameObject)
    {
        if(gameObject.CompareTag("PowerUpAddBall"))
        {
            CreateBall();
        }
    }

    /// <summary>
    /// Generate a new ball over player and add it to the list of balls.
    /// </summary>
    private void CreateBall()
    {
        GameObject ball = Instantiate(ballPrefab
                    , new Vector3(GameObject.FindWithTag("Player").transform.position.x, ballPrefab.transform.position.y, ballPrefab.transform.position.z)
                    , ballPrefab.transform.rotation);
        balls.Add(ball);
    }

    /// <summary>
    /// Destroy all balls in game.
    /// </summary>
    private void EliminatedBalls()
    {
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
        balls.Clear();
    }

    /// <summary>
    /// Restart game, set current level and score to 0, show max score, set game state to boot, restart lives images.
    /// </summary>
    private void RestartGame()
    {
        currentLevel = 0;
        score = 0;
        ShowMaxScore();
        UpdateGameState(GameState.boot);
        UIManager.Instance.restartLives();
    }
}
