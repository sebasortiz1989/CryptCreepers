using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject checkpointPrefab;
    [SerializeField] float checkpointSpawnDelay = 10;
    [SerializeField] float spawnRadius = 8;
    Vector2 randomPosition;
    [SerializeField] GameObject[] powerUpPrefab;
    [SerializeField] float powerUpSpawnDelay = 10;
    [SerializeField] AudioClip itemSpawned;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCheckpointRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnCheckpointRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(checkpointSpawnDelay);
            checkpointSpawnDelay = Random.Range(5, 20);
            randomPosition = Random.insideUnitCircle * spawnRadius;
            GameObject checkpoint = Instantiate(checkpointPrefab, (Vector3)randomPosition, Quaternion.identity);
            checkpoint.transform.parent = this.transform;
            Destroy(checkpoint, 10f);
        }
        
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerUpSpawnDelay);
            powerUpSpawnDelay = Random.Range(15, 30);
            randomPosition = Random.insideUnitCircle * spawnRadius;
            int random = Random.Range(0, powerUpPrefab.Length);
            GameObject powerUp = Instantiate(powerUpPrefab[random], (Vector3)randomPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(itemSpawned, Camera.main.transform.position, 0.2f);
            powerUp.transform.parent = this.transform;
            Destroy(powerUp, 10f);
        }
    }
}
