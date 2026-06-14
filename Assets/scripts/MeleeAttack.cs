using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float range = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float colliderDistance = 0.5f;

    [Header("References")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private Animator ani;
    private Health playerHealth;
    private bool playerInRange = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        playerInRange = PlayerInSight();

        if (playerInRange && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0f;
            ani.SetTrigger("attack");
        }
        
        if (!playerInRange)
        {
            ani.ResetTrigger("attack");
        }
    }

    private bool PlayerInSight()
    {
        Vector3 boxCenter = boxCollider.bounds.center +
                            transform.right * range * transform.localScale.x * colliderDistance;

        Vector3 boxSize = new Vector3(
            boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y,
            boxCollider.bounds.size.z
        );

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);

        if (hit != null)
        {
            playerHealth = hit.GetComponent<Health>();
            return true;
        }

        return false;
    }

    // Called via Animation Event
    private void DealDamage()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Vector3 boxCenter = boxCollider.bounds.center +
                            transform.right * range * transform.localScale.x * colliderDistance;

        Vector3 boxSize = new Vector3(
            boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y,
            boxCollider.bounds.size.z
        );

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
