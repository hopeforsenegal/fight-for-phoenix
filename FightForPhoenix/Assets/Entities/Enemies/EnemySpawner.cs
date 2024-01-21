using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float baseRange;
    public float rangeMod;
    [SerializeField] GameObject[] enemies;

    void Start()
    {
        SpawnEnemy();
    }

    Vector3 RandomSpawnPos() {
        float randomX = Random.Range(-baseRange, baseRange);
        float randomY = Random.Range(-baseRange, baseRange);

        if(randomX < 1) randomX -= rangeMod;
        else randomX += rangeMod;
        if(randomY < 1) randomY -= rangeMod;
        else randomY += rangeMod;

        return new Vector3(randomX, randomY, 0f);
    }

    public void SpawnEnemy() {
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length-1)];
        Instantiate(randomEnemy, RandomSpawnPos(), Quaternion.identity);
    }
}