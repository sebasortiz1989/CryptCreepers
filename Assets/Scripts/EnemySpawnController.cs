using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesPrefab;
    float spawnRate = 3;
    private int enemieIDX;
    [SerializeField] AudioClip itemSpawn;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnNewEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            spawnRate = Random.Range(3, 10)/GameManager.sharedInstance.difficulty;
            enemieIDX = Random.Range(0, enemiesPrefab.Length);
            GameObject enemy = Instantiate(enemiesPrefab[enemieIDX], transform.position, transform.rotation);
            enemy.transform.parent = this.transform;
        }
    }
}
