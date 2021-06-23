using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float RunSpeed = 10f;
    [SerializeField]
    private float JumpSpeed = 100f;
    [SerializeField]
    private float input_x, input_y;
    [SerializeField]
    private bool isGrounded;

    public Transform feet_location;

    public bool air_attack;

    public Animator anim;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        air_attack = false;
    }

    public void Update()
    {
        InputManager();
        AttackManager();

        //Looking Direction
        if (input_x > 0 && !Attack)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(input_x < 0 && !Attack)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !Attack)
        {
            //Set Animation State
            anim.SetBool("Jump", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);

            //Reset Air Attack
            air_attack = false;

            //ApplyForce
            rb.AddForce(new Vector2(input_x*RunSpeed, JumpSpeed), ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feet_location.position, 0.1f);

        //Apply Force to Move only if not in Attack State
        if (!Attack && input_x != 0)
            rb.velocity = new Vector2(input_x * RunSpeed, rb.velocity.y);
        else if (Attack)
            rb.velocity = new Vector2(transform.localScale.x * 0.5f, 0);
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (isGrounded)
        {
            air_attack = false;
            if (input_x != 0)
            {
                //Set Animation States
                anim.SetBool("Jump", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Run", true);
            }
            else
            {
                //Set Animation State
                anim.SetBool("Idle", true);
                anim.SetBool("Run", false);
                anim.SetBool("Jump", false);
            }
        }
        else
        {
            anim.SetBool("Jump", true);
            anim.SetFloat("Y_Vel", rb.velocity.y);
        }
    }

    void InputManager()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
    }

    float timer;
    bool Attack = false;

    void AttackManager()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Attack = false;
            anim.SetBool("Con_Attack", false);
            anim.ResetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (air_attack == true)
                return;

            timer = 0.5f;

            if (!Attack)
            {
                anim.SetTrigger("Attack");
                Attack = true;
                StepForward();
            }
            else
            {
                anim.SetBool("Con_Attack", true);
                StepForward();
            }
        }
    }

    void StepForward()
    {
        if (air_attack == false && !isGrounded)
        {
            air_attack = true;
            rb.velocity = new Vector2(transform.localScale.x * 5f, 0);
            Debug.Log("Air Attack");
        }
        else if (isGrounded)
        {
            rb.velocity = new Vector2(transform.localScale.x * 5f, 0);
            Debug.Log("Ground Attack");
        }
    }

}
