using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    //Game Components
    private GameObject Player;
    Player controlscript;
    private Animator anim;
    private Animator playerAnim;
    private Rigidbody2D rb2d;
    public GameObject attackTrigger;

    //Camera
    public GameObject mainCamera;
    public Camera camera;

    //Range Enemy
    private bool rangeEnemyAttack;
    private float range;

    //Bullet
    public Transform spawnPoint;
    public Vector3 playerPosition;
    public GameObject bullet;

    //Dash 
    public DashState dashState;
    public float dashTimer;
    public float maxDash = 1f;
    public Vector2 savedVelocity;
    private bool moveEnemy;
    private bool controlDashCollision;
    public float randomNumber;

    //Items
    private float randomIndex;
    private int itemIndex;
    private Vector3 enposition;
    public GameObject[] items;

    //Melee Attack
    public bool idle;
    private float minDistance;
    public bool attackMark;
    public bool controlMeleeCollision;

    //Enemy Attributes
    public float speed;
    public float maxSpeed;
    public float health;
    public float enemyCount = 10f;
    public bool enemyShooting;
    public float enemyDamage;
    public float enemyMadness;
    private bool bossDied;
    public bool enemyAlreadyHit;
    public bool cameFromSpawner;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        playerAnim = Player.GetComponent<Animator>();
        controlscript = Player.GetComponent<Player>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        camera = mainCamera.GetComponent<Camera>();

        if (gameObject.name == "MeleeBoss" || gameObject.name == "RangeBoss")
        {
            health = 1500f;
            speed = 1f;
        }

        else
        {
            health = 300f;
            speed = .75f;
        }
        
        enemyDamage = 25f;
        enemyMadness = 10f;
        moveEnemy = false;
        rangeEnemyAttack = false;
        attackMark = false;
        controlMeleeCollision = false;
        controlDashCollision = false;
        enemyAlreadyHit = false;
        maxSpeed = .01f;

        minDistance = .50f;
        randomNumber = Random.Range(0f, 100f);
        if (randomNumber <= 20)
        {
            dashState = DashState.Ready;
        }

        else
        {
            dashTimer = maxDash;
            dashState = DashState.Cooldown;

        }
        anim.SetBool("Idle", true);
        idle = true;
        attackTrigger.SetActive(false);
    }

    void Update()
    {
        enemyShooting = false;

		if (controlscript.dead || controlscript.reviving)
        {
            anim.SetBool("Idle", true);
            idle = true;
            anim.SetFloat("Speed", 0f);
        }
        else {
            range = Vector2.Distance(transform.position, Player.transform.position);
        }

        Vector3 viewPos = camera.WorldToViewportPoint(gameObject.transform.position);

        if (cameFromSpawner && controlscript.dead == false)
        {
            anim.SetBool("Idle", false);
            idle = false;
            anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x) + Mathf.Abs(rb2d.velocity.y));
        }

		else if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0 && !controlscript.dead && !controlscript.reviving) //activate enemy
        {
            anim.SetBool("Idle", false);
            idle = false;
            anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x) + Mathf.Abs(rb2d.velocity.y));
        }

        if (rangeEnemyAttack == true)
        {
            rb2d.isKinematic = true;
            if (gameObject.name == "MeleeBoss" || gameObject.name == "RangeBoss")
            {
                if (Player.transform.position.x > transform.position.x)
                {
                    //face right
                    transform.localScale = new Vector3(2.5f, 2.5f, 1);
                }
                else if (Player.transform.position.x < transform.position.x)
                {
                    //face left
                    transform.localScale = new Vector3(-2.5f, 2.5f, 1);
                }
            }

            else
            {
                if (Player.transform.position.x > transform.position.x)
                {
                    //face right
                    transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
                else if (Player.transform.position.x < transform.position.x)
                {
                    //face left
                    transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                }
            }
            anim.SetBool("Attack", true);
        }

        if (gameObject.tag == "Enemy")
        {  
            if (cameFromSpawner)
            {
                if (Player.transform.position.x > transform.position.x)
                {
                    //face right
                    transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
                else if (Player.transform.position.x < transform.position.x)
                {
                    //face left
                    transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                }
            }

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_DeepOnes"))
            {
                attackTrigger.SetActive(false);
            }
            
            if (range <= minDistance && !idle && !anim.GetBool("Death") == true && Player.transform.position.y >= gameObject.transform.position.y - .2f && Player.transform.position.y <= gameObject.transform.position.y + .2f)
            {
                rb2d.isKinematic = true;
                anim.SetBool("Attack", true);
                anim.SetBool("Dash", false);

                if (dashState == DashState.Cooldown)
                {
                    dashTimer -= Time.deltaTime * .1f;
                }

                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }

            }
          
            else if (range <= 1.093846f && !idle && !anim.GetBool("Death") == true && Player.transform.position.y >= gameObject.transform.position.y - .2f && Player.transform.position.y <= gameObject.transform.position.y + .2f)
            {
                //dashState = DashState.Ready;
                anim.SetBool("Attack", false);
                //Debug.Log(dashState);
                switch (dashState)
                {
                    case DashState.Ready:
                        savedVelocity = rb2d.velocity;
                        
                        rb2d.velocity = new Vector2(rb2d.velocity.x * 3f, rb2d.velocity.y);

                        dashState = DashState.Dashing;
                        break;
                    case DashState.Dashing:

                        if (Player.GetComponent<Player>().dead == true)
                        {
                            anim.SetBool("Dash", false);
                            dashState = DashState.End;
                        }

                        else
                        {
                            rb2d.isKinematic = false;
                            anim.SetBool("Dash", true);

                            dashTimer += Time.deltaTime * 100;
                            //Debug.Log("This is dashtime " + dashTimer);
                            if (dashTimer >= maxDash)
                            {

                                dashTimer = maxDash;
                                rb2d.velocity = savedVelocity;
                                dashState = DashState.Cooldown;
                            }
                        } 
                    break;
                    case DashState.Cooldown:
                        anim.SetBool("Dash", false);
                        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
                        dashTimer -= Time.deltaTime * .1f;

                        if (dashTimer <= 0)
                        {
                           dashTimer = 0;
                           dashState = DashState.Ready;
                        }
                        break;
                    default:
                        break;
                }
            }

            else if (range > minDistance && !idle && !anim.GetBool("Death") == true && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dash_DeepOnes"))
            {
                rb2d.isKinematic = false;
                anim.SetBool("Attack", false);
                anim.SetBool("Dash", false);

                if (dashState == DashState.Cooldown)
                {
                    dashTimer -= Time.deltaTime * .1f;
                }

                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }

                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);

                if (gameObject.name == "MeleeBoss" || gameObject.name == "RangeBoss")
                {
                    if (Player.transform.position.x > transform.position.x)
                    {
                        //face right
                        transform.localScale = new Vector3(2.5f, 2.5f, 1);
                    }
                    else if (Player.transform.position.x < transform.position.x)
                    {
                        //face left
                        transform.localScale = new Vector3(-2.5f, 2.5f, 1);
                    }
                }

                else
                {
                    if (Player.transform.position.x > transform.position.x)
                    {
                        //face right
                        transform.localScale = new Vector3(1.5f, 1.5f, 1);
                    }
                    else if (Player.transform.position.x < transform.position.x)
                    {
                        //face left
                        transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                    }
                }
            }
            
        }

        if (gameObject.tag == "RangeEnemy")
        {
            if (!idle && Player.transform.position.y >= gameObject.transform.position.y - .1f && Player.transform.position.y <= gameObject.transform.position.y + .1f)
            {
                rangeEnemyAttack = true;
            }

            else if (!idle && !anim.GetBool("Death") == true && !rangeEnemyAttack && rb2d.isKinematic == false)
            {
                rb2d.isKinematic = false;
                anim.SetBool("Attack", false);

                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
                if (gameObject.name == "MeleeBoss" || gameObject.name == "RangeBoss")
                {
                    if (Player.transform.position.x > transform.position.x)
                    {
                        //face right
                        transform.localScale = new Vector3(2.5f, 2.5f, 1);
                    }
                    else if (Player.transform.position.x < transform.position.x)
                    {
                        //face left
                        transform.localScale = new Vector3(-2.5f, 2.5f, 1);
                    }
                }

                else
                {
                    if (Player.transform.position.x > transform.position.x)
                    {
                        //face right
                        transform.localScale = new Vector3(1.5f, 1.5f, 1);
                    }
                    else if (Player.transform.position.x < transform.position.x)
                    {
                        //face left
                        transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                    }
                }
            }

        }

        if (controlscript.bossDead == true)
        {
            gameObject.GetComponent<Animator>().SetBool("Death", true);
        }
    }

    void FixedUpdate()
    {

        //Physics2D.gravity = Vector2.zero;

        if (idle == true)
        {
            rb2d.velocity = new Vector3(0f, 0f, 0f);
        }

        else if (gameObject.tag == "Enemy" && moveEnemy == true)
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
                if (transform.localScale.x > 0)
                {
                    //rb2d.MovePosition(transform.position + direction * 20f * Time.deltaTime);
                    rb2d.AddForce(Vector3.right * 50f);
                }
                else
                {
                    //rb2d.MovePosition(transform.position + direction * 20f * Time.deltaTime);
                    rb2d.AddForce(Vector3.left * 50f);
                }
        }
        else
        {
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
    }

    public void Damage()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash_DeepOnes"))
        {
            if (playerAnim.GetBool("Blink") == false)
            {
                Player.GetComponent<Player>().playerHealth -= 20f;
                Player.GetComponent<Player>().playerMadness += enemyMadness;
                controlscript.sounds[1].Play();
                Player.GetComponent<Animator>().SetBool("Hit", true);
                controlDashCollision = false;
            }
        }

        else if (Player.transform.position.y >= gameObject.transform.position.y - .3f && Player.transform.position.y <= gameObject.transform.position.y + .3f)
        {
            if (playerAnim.GetBool("Blink") == false)
            {
                Player.GetComponent<Player>().playerHealth -= enemyDamage;
                Player.GetComponent<Player>().playerMadness += enemyMadness;
                controlscript.sounds[1].Play();
                Player.GetComponent<Animator>().SetBool("Hit", true);
            }
            //controlMeleeCollision = false;
        }
    }

    void SetHit()
    {
        gameObject.GetComponent<Animator>().SetBool("Hit", false);
    }

    private void Destroy()
    {
        enposition = gameObject.GetComponent<EnemyMove>().transform.position;

        if (gameObject.name == "RangeBoss" || gameObject.name == "MeleeBoss")
        {
            bossDied = true;
        }

        else
        {
            bossDied = false;
        }
        Destroy(gameObject);
        randomIndex = Random.Range(1f, 100f);
        /*if (false&&randomIndex <= 60f && randomIndex >= 1f) ///THIS IF NEVER SUCCEEDS BECAUSE THE ITEMS DROPPED BY THE ENEMY ARE LIMITED TO CURRENCY
        {
            //itemIndex = Random.Range(0, collision.gameObject.GetComponent<EnemyMove>().items.Length - 1);
            itemIndex = Random.Range(3, gameObject.GetComponent<EnemyMove>().items.Length);
            GameObject coin = Instantiate(gameObject.GetComponent<EnemyMove>().items[itemIndex], enposition, Quaternion.identity);
            Destroy(coin, 5);
        }*/

       GameObject coin;
       if (!bossDied)
        {
            itemIndex = Random.Range(0, gameObject.GetComponent<EnemyMove>().items.Length);
            coin = Instantiate(gameObject.GetComponent<EnemyMove>().items[itemIndex], enposition, Quaternion.identity);
            coin.GetComponent<Items>().value = 1;
        }

       else
        {
            coin = Instantiate(gameObject.GetComponent<EnemyMove>().items[3], enposition, Quaternion.identity);
            coin.GetComponent<Items>().value = 5;
        }

       Destroy(coin, 5);
      
       Player.GetComponent<Player>().enemiesKilled++;
    }

    void InstantiateBullet()
    {
        rangeEnemyAttack = false;
        rb2d.isKinematic = false;
        //Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        Transform newBullet = Instantiate(bullet.transform, spawnPoint.position, Quaternion.identity) as Transform;
        newBullet.GetComponent<BulletFire>().Enemy = gameObject;
        //newBullet.parent = transform;
        enemyShooting = true;
    }

    void getPlayerPosition()
    {
        playerPosition = Player.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "RangeEnemy" && collision.gameObject.tag == "Player")
        {
            rb2d.isKinematic = true;
        }

        if (gameObject.tag == "RangeEnemy" && collision.gameObject.tag == "Enemy")
        {
            rb2d.isKinematic = true;
        }

        /*if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player" && attackMark && !controlMeleeCollision)
        {
            controlMeleeCollision = true;
            Damage();
        }*/

        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Enemy" && moveEnemy)
        {
            moveEnemy = false;
        }

        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player" && moveEnemy && !controlDashCollision)
        {
            moveEnemy = false;
            controlDashCollision = true;
            Damage();
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.tag == "RangeEnemy")
        {
            rb2d.isKinematic = false;
        }
    }

    private void endDashState()
    {
        anim.SetBool("Dash", false);
    }

    private void moveEnemyDash()
    {
        moveEnemy = true;
    }

    private void endMoveEnemyDash()
    {
        moveEnemy = false;
    }

    private void meleeAttack()
    {
        if (controlscript.dead == false)
        {
            attackTrigger.SetActive(true);
        }
    }

    private void stopMeleeAttack()
    {
        attackTrigger.SetActive(false);
    }

    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown,
        End
    }

}