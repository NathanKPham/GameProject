using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadnessBall : MonoBehaviour {

    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody2D>().AddForce(-transform.right * 75);
    }

    private void Update()
    {
        if (gameObject.transform.position.x <= 84.45715f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<Animator>().GetBool("Blink") == false && player.GetComponent<Animator>().GetBool("Hit") == false && player.GetComponent<Player>().dead == false && player.GetComponent<Player>().reviving == false)
        {
            player.GetComponent<Player>().playerMadness += 50;
            player.GetComponent<Player>().sounds[1].Play();
            player.GetComponent<Animator>().SetBool("Hit", true);
        }
    }
}
