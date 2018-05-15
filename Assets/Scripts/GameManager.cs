using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;

    public int playerFood = 100;
    [HideInInspector] public bool playersTurn = true;

    private List<Enemy> enemies;
    private bool enemiesMoving;

	// Use this for initialization
	void Awake () {
        if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}
	
    void InitGame()
    {
        enemies.Clear();
        boardScript.SetupScene(3);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
