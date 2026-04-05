using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    public float reloadTime;
    public GameObject bulletPrefab;
    public HP_bar hpBar;
    
    private float lastShotTime;

    public GameManager gameManager;

    public bool doubleBullets = false;
    public int flatDamageUp = 0;


    Rigidbody2D rb;
    Vector3 respawnPosition;
    Quaternion respawnRotation;

    // Start is called once before the first frame update
    void Start()
    {
        lastShotTime = -reloadTime;
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null && gameManager.IsPaused())
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touchPos.x < 0)
            {
                rb.AddForce(Vector2.left * moveSpeed);
            }
            else
            {
                rb.AddForce(Vector2.right * moveSpeed);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (Time.time - lastShotTime >= reloadTime)
        {
            GameObject projectileObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if (gameManager != null)
            {
                gameManager.RegisterLevelObject(projectileObject);
            }

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            if (doubleBullets)
            {
                projectile.projectileType = Projectile.ProjectileType.Double;
                
            }
            projectile.damage += flatDamageUp;
            projectile.Init();
            lastShotTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            if (hpBar.GetHP() > 1)
            {
                hpBar.changeHP(-1);
                collision.gameObject.SetActive(false);
            }
            else
            {
                if (gameManager != null)
                {
                    hpBar.changeHP(-1);
                    gameManager.GameOver();
                }
            }
        }
    }

    public void Heal(int amount)
    {
        hpBar.changeHP(amount);
    }

    public void Respawn()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        transform.position = respawnPosition;
        transform.rotation = respawnRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (hpBar != null)
        {
            hpBar.changeHP(hpBar.hp_max - hpBar.GetHP());
        }
    }
}
