﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;


    // Use this for initialization
    protected override void Start () {

        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    // Update is called once per frame
    void Update () {
        // If it's not the player's turn, just return
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
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

        // Application.LoadLevel(Application.loadedLevel); // this is obsolete
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }
}