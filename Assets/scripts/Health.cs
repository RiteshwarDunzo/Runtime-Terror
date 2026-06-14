using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100f;
    public float currentHealth { get; private set; }

    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // Player hurt animation
            anim.SetTrigger("hurt");

            // TODO: Add iFrames logic here if needed
        }
        else
        {
            if (!dead)
            {
                // Trigger death animation
                anim.SetTrigger("die");

                // Disable player movement
                GetComponent<PlayerControl>().enabled = false;

                dead = true;
            }
        }
    }

    private void Update()
    {
        // Test damage with E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(0f);
        }
    }
}