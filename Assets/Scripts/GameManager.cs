using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int levelStartDelay = 2;
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;


    private List<Enemy> enemies;
    private bool enemiesMoving;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;


    public static GameManager instance = null;

    private int level = 1;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void OnLevelWasLoaded(int level)
    {
        level++;
        InitGame();

    }

    //private void OnlevelFinishedLoading(Scene scene, LoadSceneMode mode)
    //{
    //    level++;
    //    InitGame();
    //}

    //void OnEnable()
    //{//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
    //    SceneManager.sceneLoaded += OnlevelFinishedLoading;

    //        //+= OnLevelFinishedLoading;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnlevelFinishedLoading;
    //}


    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        // levelText.text = "Day " + level.ToString();
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);


        enemies.Clear();
        boardScript.SetupScene(level);

        
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Update is called once per frame
    void Update () {

        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
		
	}

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i=0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving= false;

    }
}
