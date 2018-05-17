using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 20;
    public int pointsPerSoda = 10;
    public float restartlevelDelay = 1f;
    public AudioClip mFX1;
    public AudioClip mFX2;
    public AudioClip eFX1;
    public AudioClip eFX2;
    public AudioClip dFX1;
    public AudioClip dFX2;
    public AudioClip lFX1;


    private Text foodText;

    private Animator animator;
    private int food = 100;
	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFood;

        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        foodText.text = "Food: " + food;
        base.Start();
	}

    // Update is called once per frame
    void Update () {
        if (!GameManager.instance.playersTurn) { return; }
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if(horizontal != 0) { vertical = 0; }
        if(horizontal != 0 || vertical != 0 && !GameManager.instance.doingSetup)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Exit")
        {
            GameManager.instance.playerFood = food;
            Restart();
            enabled = false;
        } else if(collision.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Food: " + food + "\n+" + pointsPerFood;
            SoundManager.instance.RandomizeSFX(eFX1, eFX2);
            collision.gameObject.SetActive(false);
        } else if( collision.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "Food: " + food + "\n+" + pointsPerSoda;
            SoundManager.instance.RandomizeSFX(dFX1, dFX2);
            collision.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        GameManager.instance.doingSetup = true;
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "Food: " + food + "\n-" + loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if(food <= 0) {
            GameManager.instance.GameOver();
            SoundManager.instance.PlaySingle(lFX1);
            SoundManager.instance.musicSource.Stop();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if(Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSFX(mFX1, mFX2);
        }
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }
}
