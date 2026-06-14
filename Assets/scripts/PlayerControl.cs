using UnityEngine;
public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D body;

    private Animator ani;
    
    private static bool grounded;
    
    private static bool run;
    

    [SerializeField]private float speed;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        
        ani= GetComponent<Animator>();
        
        
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal")*speed, body.linearVelocity.y);// input for movement
        if (h > 0.01f)
        {
            transform.localScale = new Vector2(1,1);
        }

        if (h < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        
        
        if(Input.GetKey(KeyCode.Space)&&grounded)
        {
            Jump(); //input for jump
        }

        /*  if (Input.GetAxis("Horizontal") != 0 && grounded)
          {
              run_attack();
          }*/
        



        ani.SetBool("run", h != 0.0f);// triggers player run animation
        
        ani.SetBool("grounded", grounded);// triggers jump animation
        
        
    }

    
    
    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.tag == "Base")
        {
            grounded = true;
        }
    }

    /* private void run_attack()
     {

             run = true;
             grounded = true;

     }*/
    public static bool canAttack()
    {
        return  grounded;
    }
    
}