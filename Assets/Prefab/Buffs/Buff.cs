using UnityEngine;
using TMPro;

public class Buff : MonoBehaviour
{
    public float moveSpeed;
    public int hp;
    public TMPro.TextMeshPro HP_text;
    public GameObject iconCotainer;
    
    public enum BuffType
    {
        DoubleBullets,
        DamageUp,
        RapidFire,
        Heal,
        ShadowClone,
        Freeze,
    }

    public BuffType buffType;

    public float commonHPMultiplier = 1f;

    [Header("HP multiplier per type")]
    public int doubleBulletsHP;
    public int damageUpHP;
    public int rapidFireHP;
    public int healHP;
    public int shadowCloneHP;
    public int freezeHP;

    [Header("Sprites per type")]
    public Sprite doubleBulletsSprite;
    public Sprite damageUpSprite;
    public Sprite rapidFireSprite;
    public Sprite healSprite;
    public Sprite shadowCloneSprite;
    public Sprite freezeSprite;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Init(BuffType type)
    {
        buffType = type;
        SpriteRenderer spriteRenderer = iconCotainer.GetComponent<SpriteRenderer>();
        switch (buffType)
        {
            case BuffType.DoubleBullets:
                spriteRenderer.sprite = doubleBulletsSprite;
                hp = doubleBulletsHP;
                break;
            case BuffType.DamageUp:
                spriteRenderer.sprite = damageUpSprite;
                hp = damageUpHP;
                break;
            case BuffType.RapidFire:
                spriteRenderer.sprite = rapidFireSprite;
                hp = rapidFireHP;
                break;
            case BuffType.Heal:
                spriteRenderer.sprite = healSprite;
                hp = healHP;
                break;
            case BuffType.ShadowClone:
                spriteRenderer.sprite = shadowCloneSprite;
                hp = shadowCloneHP;
                break;
            case BuffType.Freeze:
                spriteRenderer.sprite = freezeSprite;
                hp = freezeHP;
                break;
        }
        hp = Mathf.RoundToInt(hp * commonHPMultiplier);
        HP_text.text = hp.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, -moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            projectile.OnHit();

            hp -= projectile.damage;
            HP_text.text = hp.ToString();
            if (hp <= 0)
            {
                ApplyBuff();
                Destroy(gameObject);
            }
        }
    }

    private void ApplyBuff()
    {
        switch (buffType)
        {
            case BuffType.DoubleBullets:
                gameManager.ActivateDoubleBullets();
                break;
            case BuffType.DamageUp:
                gameManager.ActivateDamageUp();
                break;
            case BuffType.RapidFire:
                gameManager.ActivateRapidFire();
                break;
            case BuffType.Heal:
                gameManager.ActivateHeal();
                break;
            case BuffType.ShadowClone:
                gameManager.ActivateShadowClone();
                break;
            case BuffType.Freeze:
                gameManager.ActivateFreeze();
                break;
            default:
                break;
        }
    }
}