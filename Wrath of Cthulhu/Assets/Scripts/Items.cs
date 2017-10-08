using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

	//PlayerCurrency
	public Transform PageCount;
	Currency pagescript;

	// Use this for initialization
	void Start () {
		PageCount = GameObject.Find ("PageCount").transform;
		pagescript = PageCount.GetComponent<Currency> ();
	}
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Currency")
        {
            if (collision.tag == "Player")
            {
				pagescript.count += 1;
                Destroy(gameObject);
            }
        }

        if (gameObject.tag == "Elixir")
        {
            if (collision.tag == "Player")
            {
                if (collision.gameObject.GetComponent<Player>().item != "elixir")
                {
                    collision.gameObject.GetComponent<Player>().item = "elixir";
                }
                //collision.gameObject.GetComponent<Player>().playerHealth += 50f;
                Destroy(gameObject);
            }
        }
    }
}
