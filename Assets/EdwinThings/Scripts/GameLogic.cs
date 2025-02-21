using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Header("Prefabs and Spawn Points")]
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform[] spawnPoints;

    [Header("Timing Settings")]
    public float waveDuration = 30f; // Each wave lasts 30 seconds
    public float waitDuration = 30f; // Waiting period of 30 seconds
    public float spawnInterval = 5f;

    [SerializeField] TMP_Text timer;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool bossSpawned = false;
    private float elapsedTime = 0f;
    private bool isWaiting = false;
    private bool isTiming = false;

    void Start()
    {
        StartCoroutine(WaveCycleRoutine());
    }

    private void Update()
    {
        if (!isTiming) return;

        // Update the timer text based on whether we're waiting or spawning
        if (isWaiting)
        {
            timer.text = Mathf.Max(0, Mathf.Ceil(waitDuration - elapsedTime)).ToString() + "s";
        }
        else
        {
            timer.text = Mathf.Max(0, Mathf.Ceil(waveDuration - elapsedTime)).ToString() + "s";
        }

        // Update elapsedTime every frame for consistent countdown
        if (elapsedTime < (isWaiting ? waitDuration : waveDuration))
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            isTiming = false; // Stop updating time when the duration is reached
        }
    }

    IEnumerator WaveCycleRoutine()
    {
        // First wave for 30 seconds
        isWaiting = false;
        elapsedTime = 0f;
        isTiming = true;
        yield return StartCoroutine(SpawnEnemiesForDuration(waveDuration));

        // Despawn all enemies after the first wave
        DespawnEnemies();

        // Wait for 30 seconds without spawning anything
        isWaiting = true;
        elapsedTime = 0f;
        isTiming = true;
        yield return StartCoroutine(WaitForDuration(waitDuration));

        // Second wave for 30 seconds
        isWaiting = false;
        elapsedTime = 0f;
        isTiming = true;
        yield return StartCoroutine(SpawnEnemiesForDuration(waveDuration));

        // Despawn all enemies again
        DespawnEnemies();

        // Wait for 30 seconds without spawning anything
        isWaiting = true;
        elapsedTime = 0f;
        isTiming = true;
        yield return StartCoroutine(WaitForDuration(waitDuration));

        // Spawn the boss
        if (!bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
            isTiming = false; // Stop the timer when the boss spawns
            timer.text = "0s"; // Ensure the timer shows 0 when complete
        }
    }

    IEnumerator SpawnEnemiesForDuration(float duration)
    {
        while (elapsedTime < duration)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator WaitForDuration(float duration)
    {
        while (elapsedTime < duration)
        {
            yield return null;
        }
    }

    void SpawnWave()
    {
        if (isWaiting) return; // Prevents spawning during the waiting period

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
        Debug.Log("All enemies despawned.");
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && spawnPoints.Length > 0)
        {
            DespawnEnemies(); // Clear all enemies before spawning the boss
            Instantiate(bossPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
            Debug.Log("Boss Spawned!");
        }
    }
}
