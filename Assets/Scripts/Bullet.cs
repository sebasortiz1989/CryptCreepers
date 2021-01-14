using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 15f;
    public int healthBullet = 3;
    public bool powerShot;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Enemy")
        {
            otherCollider.GetComponent<Enemy>().TakeDamage();
            if (!powerShot)
                Destroy(gameObject);
            else
                healthBullet--;

            if (healthBullet <= 0)
                Destroy(gameObject);
        }
    }
}
