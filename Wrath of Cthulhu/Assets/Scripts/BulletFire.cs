using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour {

    public float bulletForce = 500f;
    private GameObject Player;
    private GameObject Enemy;
    private float bulletDamage;
    private Vector3 enposition;
    private int itemIndex;
    private float randomIndex;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").gameObject;
        Enemy = GameObject.FindWithTag("Enemy").gameObject;
        bulletDamage = 100f;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "FirePoint")
        {
            if (Player.transform.localScale.x > 0)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.right * bulletForce);
            }

            else
            {
                GetComponent<Rigidbody2D>().AddForce(-transform.right * bulletForce);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyMove>().health -= bulletDamage;
            Destroy(gameObject);

            if (collision.gameObject.GetComponent<EnemyMove>().health <= 0)
            {
                enposition = collision.gameObject.GetComponent<EnemyMove>().transform.position;
                Destroy(collision.gameObject);
                Destroy(gameObject);
                randomIndex = Random.Range(1f, 100f);
                if (randomIndex <= 10f && randomIndex >= 1f)
                {
                    //itemIndex = Random.Range(0, collision.gameObject.GetComponent<EnemyMove>().items.Length - 1);
                    GameObject coin = Instantiate(collision.gameObject.GetComponent<EnemyMove>().items[3], enposition, Quaternion.identity);
                    Destroy(coin, 5);
                }

                else
                {
                    itemIndex = Random.Range(0, collision.gameObject.GetComponent<EnemyMove>().items.Length - 1);
                    GameObject coin = Instantiate(collision.gameObject.GetComponent<EnemyMove>().items[itemIndex], enposition, Quaternion.identity);
                    Destroy(coin, 5);
                }

            }
        }

        else
        {
            Destroy(gameObject);
        }
    }

}
