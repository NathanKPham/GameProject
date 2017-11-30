using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<Animator>().GetBool("Blink") == false && player.GetComponent<Animator>().GetBool("Hit") == false && player.GetComponent<Player>().dead == false && player.GetComponent<Player>().reviving == false)
        {
            player.GetComponent<Player>().playerHealth -= 25f;
            player.GetComponent<Player>().playerMadness += 15;
            player.GetComponent<Player>().sounds[1].Play();
            player.GetComponent<Animator>().SetBool("Hit", true);
        }
    }
}
