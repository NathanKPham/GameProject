using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float waveCount = 0f;
    public float spawnTime = 4f;
    //The amount of time between each spawn.
    public float spawnDelay = 10f;
    //The amount of time before spawning starts.
    public GameObject[] enemies;
    //Array of enemy prefabs.
    public Vector3 enposition;
    void Start()
    {
        //Start calling the Spawn function repeatedly after a delay.
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
 
    }

    void Spawn()
    {
        //Randomly Spawn
        float spawnPointX = Random.Range(2.12f, 6.75f);
        float spawnPointY = Random.Range(-.57f, 2f);
        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, 0);

        waveCount++;
        //Instantiate a random enemy.
        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], spawnPosition, transform.rotation);
        if (waveCount == 10f)
        {
            CancelInvoke();
        }
    }
}
