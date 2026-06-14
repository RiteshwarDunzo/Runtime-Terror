using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform firePoint;    
    [SerializeField] private GameObject[] bullets;    // Pre-placed bullets for Object Pooling 

    private Animator ani;
    private PlayerControl playerControl;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
    }

    private void Update()
    {
        // Always update the cooldown timer
        cooldownTimer += Time.deltaTime;

        // Check if player can attack
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && PlayerControl.canAttack())
        {
            Attack();
        }
    }

    private void Attack()
    {
        ani.SetTrigger("attack");
        cooldownTimer = 0f;

        int bulletIndex = FindAvailableBullet();
        if (bulletIndex == -1) return; // No available bullet

        GameObject bullet = bullets[bulletIndex];

        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPosition = firePoint.position; // Firepoint in front of player

        // Position and fire the bullet
        bullet.transform.position = spawnPosition;
        bullet.GetComponent<Projectile>().SetDirection(direction);
    }

    private int FindAvailableBullet()
    {
        // Return first inactive bullet in the pool
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1; // All bullets are active
    }
}
