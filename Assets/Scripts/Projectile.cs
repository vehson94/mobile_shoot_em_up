using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int baseDamage = 1;

    public int damage = 1;
    public int pierce = 0;

    public enum ProjectileType { Normal, Double }
    public ProjectileType projectileType = ProjectileType.Normal;

    public Sprite normalSprite;
    public Sprite doubleSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    public void Init()
    {
        damage = baseDamage;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * speed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (projectileType)
        {
            case ProjectileType.Normal:
                spriteRenderer.sprite = normalSprite;
                break;
            case ProjectileType.Double:
                spriteRenderer.sprite = doubleSprite;
                damage *= 2;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 5f)
        {
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        if (pierce > 0)
        {
            pierce -= 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
