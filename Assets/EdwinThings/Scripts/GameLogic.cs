using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Header("Prefabs and Spawn Points")]
    public GameObject[] enemyPrefabs;      
    public GameObject bossPrefab;         
    public Transform[] spawnPoints;       

    [Header("Timing Settings")]
    public float waveDuration = 60f;     
    public float waitDuration = 60f;    
    public float spawnInterval = 5f;       

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int waveCycle = 0;         
    private bool bossSpawned = false;
    float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(WaveCycleRoutine());
    }

    private void Update()
    {
        Debug.Log(elapsedTime);
    }

    IEnumerator WaveCycleRoutine()
    {
        while (true)
        {

            while (elapsedTime < waveDuration)
            {
                SpawnWave();
                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
            }

            DespawnEnemies();

            waveCycle++;

            if (waveCycle >= 5 && !bossSpawned)
            {
                SpawnBoss();
                bossSpawned = true;
                // Optionally break out if you only want one boss wave:
                break;
            }

            yield return new WaitForSeconds(waitDuration);
        }
    }

    void SpawnWave()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (enemyPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);
                spawnedEnemies.Add(enemy);
            }
        }
    }

    void DespawnEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && spawnPoints.Length > 0)
        {
            Instantiate(bossPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
            Debug.Log("Boss Spawned!");
        }
    }
}
