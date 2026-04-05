using UnityEngine;

public class Block : MonoBehaviour
{
    public float hp = 3f;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            projectile.OnHit();

            hp -= projectile.damage;
            if (hp <= 0)
            {
                Destroy(gameObject);
                gameManager.AddScore(1);
            }
        }
    }
}
