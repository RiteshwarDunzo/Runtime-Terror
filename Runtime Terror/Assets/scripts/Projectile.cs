using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;       // Bullet speed
    [SerializeField] private float maxLifetime = 5f;  // Max time before auto-disable
    [SerializeField] private int damage = 1;          // Damage dealt to enemy

    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        // Reset state when activated from pool
        hit = false;
        lifetime = 0f;
        if (boxCollider != null) boxCollider.enabled = true;
    }

    private void Update()
    {
        if (hit) return;

        // Move projectile
        transform.Translate(speed * direction * Time.deltaTime, 0, 0);

        // Lifetime check
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        if (collision.CompareTag("Enemy"))
        {
            // Try to get the Health script from the enemy
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    // Sets the direction of the projectile
    public void SetDirection(float _direction)
    {
        lifetime = 0f;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Called at end of explosion animation
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
