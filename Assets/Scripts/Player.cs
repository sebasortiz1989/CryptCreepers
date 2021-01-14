using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float horizontal;
    float vertical;
    float originalSpeed;
    int playerHealth;
    public bool powerShotEnabled = false;
    bool takeDamageCooldown = false;
    bool gunLoaded = true;

    Vector3 moveDirection; //Porque transform.position es Vector3
    Vector2 facingDirection;

    GameObject bulletFired;
    Animator anim;
    CameraController camController;

    [SerializeField] float moveSpeed = 7f;
    [SerializeField] Transform aim;
    [SerializeField] Camera cameraX;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate = 3;
    [SerializeField] float takeDamageCooldownTime = 3f;
    [SerializeField] Image[] hearts;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Text fireRateText;
    [SerializeField] float blinkRate = 0.01f;
        
    private void Start()
    {
        originalSpeed = moveSpeed;
        playerHealth = hearts.Length;
        anim = GetComponentInChildren<Animator>();
        camController = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Aim();
    }

    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        moveDirection.x = horizontal;
        moveDirection.y = vertical;
        moveDirection.z = 0;
        transform.position += (moveDirection * Time.deltaTime * moveSpeed);
        anim.SetFloat("Speed", moveDirection.magnitude);
    }

    private void Aim()
    {
        facingDirection = cameraX.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Traducir posicion del puntero a espacio del mundo
        aim.position = transform.position + (Vector3)facingDirection.normalized*2.5f;
        
        if (facingDirection.x < 0)
        {
            sprite.flipX = false;
        }
        if (facingDirection.x > 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetMouseButton(0) && gunLoaded)
        {
            
            gunLoaded = false;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bulletFired = Instantiate(bullet, transform.position, targetRotation);
            bulletFired.transform.parent = this.transform;
            if (powerShotEnabled)
                bulletFired.GetComponent<Bullet>().powerShot = true;

            StartCoroutine(ReloadGun());
        } 
    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1/fireRate);
        gunLoaded = true;
    }

    public void TakeDamage()
    {
        if (!takeDamageCooldown)
        {
            playerHealth--;
            EmptyHearts();
            camController.Shake();
        }
            
        takeDamageCooldown = true;
        StartCoroutine(TakeDamageCooldown());

        if (playerHealth <= 0)
        {
            UIManager.sharedInstance.ShowGameOverScreen();
        }
    }

    IEnumerator TakeDamageCooldown()
    {
        StartCoroutine(BlinkRoutine());
        yield return new WaitForSeconds(takeDamageCooldownTime);
        takeDamageCooldown = false;
    }

    IEnumerator BlinkRoutine()
    {
        int t = 10;
        while (t > 0)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(t * blinkRate);
            sprite.enabled = true;
            yield return new WaitForSeconds(t * blinkRate);
            t--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            switch (collision.GetComponent<PowerUp>().powerupType)
            {
                case PowerUp.PowerUpType.FireRateIncrease:
                    fireRate = fireRate + 1;
                    fireRateText.text = fireRate - 3 + " X";
                    break;
                case PowerUp.PowerUpType.PowerShot:
                    powerShotEnabled = true;
                    StopCoroutine(PowerShotTimer());
                    StartCoroutine(PowerShotTimer());
                    break;
                case PowerUp.PowerUpType.Potion:
                    if (playerHealth == 5) { return; }          
                    playerHealth++;
                    for (int i = 0; i < hearts.Length; i++)
                    {
                        if (playerHealth - 1 == i)
                        {
                            hearts[i].gameObject.SetActive(true);
                        }
                    }
                    break;
            }
            Destroy(collision.gameObject, 0.1f);
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            moveSpeed = moveSpeed / 2;
        }
    }

    IEnumerator PowerShotTimer()
    {
        sprite.color = new Color32(255, 107, 107, 255);
        yield return new WaitForSeconds(20f);
        sprite.color = Color.white;
        powerShotEnabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            moveSpeed = originalSpeed;
        }
    }

    private void EmptyHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (playerHealth - 1 < i)
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
