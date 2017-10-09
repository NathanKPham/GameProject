﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    private GameObject Player;
    public float speed = 1f;
    public float maxSpeed = .01f;
    public float health;
    public bool idle;
    public float enemyCount = 10f;
    private bool activateEnemy = false;
    private float randomIndex;
    private int itemIndex;
    private Vector3 enposition;

    private Animator anim;
    private Rigidbody2D rb2d;
    private float minDistance = .524848f;
    private float range;
    public float enemyDamage;
    public float enemyMadness;
    RaycastHit2D hit;
    Player controlscript;
    bool contact;

    public GameObject[] items;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        controlscript = Player.GetComponent<Player>();
        health = 300f;
        enemyDamage = 25f;
        enemyMadness = 10f;
        contact = false;
        idle = true;

    }

    void Update()
	{
		
		if (controlscript.playerHealth > 0 && controlscript.playerMadness < 100) {
			range = Vector2.Distance (transform.position, Player.transform.position);
		} 

        if (range <= 2.751363f)
        {
            activateEnemy = true;
        }

        if (activateEnemy == true)
        {
            anim.SetBool("Idle", false);
            idle = false;
            anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x) + Mathf.Abs(rb2d.velocity.y));
        }
		else {
			anim.SetBool ("Idle", true);
			idle = true;
		}

        if (Player.GetComponent<Animator>().GetBool("Dead") == true)
        {
            anim.SetBool("Idle", true);
        }
		if (range <= minDistance && !idle) {
			rb2d.isKinematic = true;
			anim.SetBool ("Attack", true);
		}

		if (range > minDistance && !idle) {
			rb2d.isKinematic = false;
			anim.SetBool ("Attack", false);

			transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
			if (Player.transform.position.x > transform.position.x) {
				//face right
				transform.localScale = new Vector3 (1.5f, 1.5f, 1);
			} else if (Player.transform.position.x < transform.position.x) {
				//face left
				transform.localScale = new Vector3 (-1.5f, 1.5f, 1);
			}
		}
	}

    void FixedUpdate()
    {

        Physics2D.gravity = Vector2.zero;

        rb2d.velocity = (Player.transform.position - transform.position).normalized * speed;

        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.y > maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxSpeed);
        }

        if (rb2d.velocity.y < -maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -maxSpeed);
        }
    }

    void Damage()
    {

        if (Player.transform.position.y >= gameObject.transform.position.y - .3f && Player.transform.position.y <= gameObject.transform.position.y + .3f)
        {
            Player.GetComponent<Player>().playerHealth -= enemyDamage;
            Player.GetComponent<Player>().playerMadness += enemyMadness;
            Player.GetComponent<Animator>().SetBool("Hit", true);
        }
    }

    void SetHit()
    {
        gameObject.GetComponent<Animator>().SetBool("Hit", false);
    }

    private void Destroy()
    {
        enposition = gameObject.GetComponent<EnemyMove>().transform.position;
        randomIndex = Random.Range(1f, 100f);
        if (randomIndex <= 10f && randomIndex >= 1f)
        {
            //itemIndex = Random.Range(0, collision.gameObject.GetComponent<EnemyMove>().items.Length - 1);
            GameObject coin = Instantiate(gameObject.GetComponent<EnemyMove>().items[3], enposition, Quaternion.identity);
            Destroy(coin, 5);
        }

        else
        {
            itemIndex = Random.Range(0, gameObject.GetComponent<EnemyMove>().items.Length - 1);
            GameObject coin = Instantiate(gameObject.GetComponent<EnemyMove>().items[itemIndex], enposition, Quaternion.identity);
            Destroy(coin, 5);
        }
        Destroy(gameObject);
        Player.GetComponent<Player>().enemiesKilled++;
    }

}
