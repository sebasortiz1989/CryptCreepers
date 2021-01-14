using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float angle;
    Vector2 direction;
    Vector3 directionUnit;
    Animator anim;
    private GameObject player;

    [SerializeField] int health = 3;
    [SerializeField] float enemySpeed = 0.5f;
    [SerializeField] int scorePoints = 10;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip enemyDeath;

    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
        transform.position = spawnPoint[randomSpawnPoint].transform.position;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        direction = player.transform.position - transform.position;
        directionUnit = (Vector3)direction / direction.magnitude; //direction.normalized hace lo mismo!!!
        transform.position += directionUnit * Time.deltaTime * enemySpeed;
        angle = Mathf.Atan2(directionUnit.y, directionUnit.x) * Mathf.Rad2Deg;
        anim.SetFloat("angle", angle);
    }

    public void TakeDamage()
    {
        AudioSource.PlayClipAtPoint(impactClip, Camera.main.transform.position, 0.1f);
        health--;
        if (FindObjectOfType<Player>().powerShotEnabled)
        {
            health--;
        }
        
        if (health <= 0)
        {
            GameManager.sharedInstance.Score += scorePoints;
            Destroy(gameObject, 0.05f);
            AudioSource.PlayClipAtPoint(enemyDeath, Camera.main.transform.position, 0.1f);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage();
        }
    }
}
