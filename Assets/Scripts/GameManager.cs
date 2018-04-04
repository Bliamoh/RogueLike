using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public float LevelStartDelay = 2f;
    public float TurnDelay = 0.1f;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector] public bool PlayerTurn = true;

    private Text _levelText;
    private GameObject _levelImage;
    private int _level;
    private List<Enemy> _enemies;
    private bool _enemiesMoving;
    private bool _doingSetup;

	void Awake ()
	{
        if (Instance == null)
	        Instance = this;
	    else if (Instance != this)
	        Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _enemies = new List<Enemy>();
	    BoardScript = GetComponent<BoardManager>();
	}

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        _level++;
        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame()
    {
        _doingSetup = true;

        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.fontSize = 32;
        _levelText.text = "Day " + _level;
        _levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        _enemies.Clear();
        BoardScript.SetupScene(_level);
    }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _doingSetup = false;
    }

    public void GameOver()
    {
        _levelText.fontSize = 16;
        _levelText.text = "After " + _level + " days, you starved...";
        _levelImage.SetActive(true);
        enabled = false;
    }

    void Update ()
    { 
        if (PlayerTurn || _enemiesMoving || _doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        _enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        _enemiesMoving = true;
        yield return new WaitForSeconds(TurnDelay);
        if (_enemies.Count == 0)
        {
            yield return new WaitForSeconds(TurnDelay);
        }

        foreach (Enemy enemy in _enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.MoveTime);
        }

        PlayerTurn = true;
        _enemiesMoving = false;
    }
}
