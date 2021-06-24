using System;
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
    [SerializeField]
    float length;
    [SerializeField]
    bool noheadroom = false;

    public Transform feet_location;

    public bool air_attack;
    float timer;
    bool Attack = false;
    bool Shield = false;
    bool Dodge = false;
    public bool isWall;

    public Animator anim;
    public Rigidbody2D rb;
    public GameObject headClearance;

    #region StartFunction
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        air_attack = false;
    }
    #endregion

    #region UpdateFunction
    public void Update()
    {
        InputManager();
        LookDirectionManager();
        JumpManager();
        
        AttackManager();
        ShieldBlockManager();
        DodgeRollManager();
    }

    private void JumpManager()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !Attack && !Shield && !Dodge)
        {
            //Set Animation State
            anim.SetBool("Jump", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);

            //Reset Air Attack
            air_attack = false;

            //ApplyForce
            rb.AddForce(new Vector2(input_x * RunSpeed, JumpSpeed), ForceMode2D.Impulse);
        }
    }

    private void LookDirectionManager()
    {
        //Looking Direction
        if (input_x > 0 && !Attack && !Shield && !Dodge &&!ledgeGrabbed)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (input_x < 0 && !Attack && !Shield && !Dodge && !ledgeGrabbed)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    #endregion

    #region FixedUpdateFunction
    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feet_location.position, 0.1f);
        LedgeGrabManager();

        //Apply Force to Move only if not in Attack State
        if (!Attack && !Shield && !Dodge && input_x != 0 &&!ledgeGrabbed)
        {
            rb.velocity = new Vector2(input_x * RunSpeed, rb.velocity.y);
        }
        else if (Attack)
        {
            rb.velocity = new Vector2(transform.localScale.x * 0.5f, 0);
        }
        else if (Dodge || noheadroom)
        {
            rb.velocity = new Vector2(transform.localScale.x * 7f, rb.velocity.y);
        }
        else if (ledgeGrabbed)
        {
            rb.velocity = new Vector2(0,0);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (isGrounded)
        {
            air_attack = false;
            if (input_x != 0)
            {
                //Set Animation States - Running
                anim.SetBool("Jump", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Run", true);
            }
            else
            { 
                //Set Animation State - Idle
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
    #endregion

    #region InputManager
    void InputManager()
    {
        //Get inputs
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
    }
    #endregion

    #region AttackManager
    void AttackManager()
    {

        //wait for net attack event
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

        //Trigger attack
        if (Input.GetKeyDown(KeyCode.J) &&!Dodge)
        {
            //Check If already used airattack
            if (air_attack == true)
                return;

            timer = 0.5f;

            //Check if attack event is triggered
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
    #endregion

    #region StepForward
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
    #endregion

    #region ShieldBlock
    void ShieldBlockManager()
    {
        //Conditions for shield block state
        if (Input.GetKey(KeyCode.L) && isGrounded && !Attack && !Shield &&!Dodge)
        {
            anim.SetTrigger("Shield_Block");
            Shield = true;
        }
        else if (Input.GetKey(KeyCode.L) && isGrounded && !Attack && Shield && !Dodge)
        {
            anim.SetBool("shielded", true);
        }
        else
        {
            Shield = false;
            anim.SetBool("shielded", false);
            anim.ResetTrigger("Shield_Block");
        }

    }
    #endregion

    #region DodgeRollManager
    void DodgeRollManager()
    {
        HeadClearanceCheck();
        if (Input.GetKeyDown(KeyCode.I) && !Dodge && !Attack && !Shield && isGrounded)
        {
            StartCoroutine(DodgeRoll());
        }
    }

    IEnumerator DodgeRoll()
    {
        Dodge = true;
        anim.SetTrigger("Roll");
        yield return new WaitForSeconds(0.9f);
        yield return new WaitWhile(() => HeadClearanceCheck());
        anim.ResetTrigger("Roll");
        Dodge = false;
    }

    
    public bool HeadClearanceCheck()
    {
        if (Physics2D.Raycast(headClearance.transform.position, Vector2.up, length, LayerMask.GetMask("Platforms"))&& Dodge)
        {
            noheadroom = true;
            anim.SetBool("Con_Roll", noheadroom);
            rb.velocity = new Vector2(transform.localScale.x * 7f, rb.velocity.y);
        }
        else
        {
            noheadroom = false;
            anim.SetBool("Con_Roll", noheadroom);
        }

        Debug.DrawRay(headClearance.transform.position, Vector2.up*length, Color.red);
        return noheadroom;
    }
    #endregion

    #region LedgeGrabManager
    public float ledgelength = 2f;
    public GameObject ledgeGrabFinder;
    RaycastHit2D hit;
    public bool pointfound = false;
    public bool ledgeGrabbed = false;
    Vector3 dir;

    IEnumerator ClimbUp()
    {
        anim.SetTrigger("LedgeClimbUp");
        anim.ResetTrigger("LedgeGrab");
        yield return new WaitForSeconds(1f);
        ledgeGrabbed = false;
        anim.ResetTrigger("LedgeClimbUp");
    }

    void LedgeGrabManager()
    {
        dir = transform.localScale;
        dir.y = dir.z = 0;

        if (ledgeGrabbed)
        {
            Debug.DrawRay(hit.point, hit.normal * 5f, Color.yellow);
            if (input_y < 0)
            {
                transform.position = transform.Find("PlayerCharacter").position;
                ledgeGrabbed = false;
                rb.gravityScale = 1.5f;
                anim.SetTrigger("LedgeClimbDown");
                anim.ResetTrigger("LedgeGrab");
            }
            if (input_y > 0)
            {
                StartCoroutine(ClimbUp());
            }
        }
        else if (!ledgeGrabbed && isGrounded)
        {
            pointfound = false;
        }
        else if (isGrounded)
        {
            pointfound = false;
        }

        if (!pointfound)
        {
            WallFinder();
            if (rb.velocity.y < 0f && !isGrounded)
            {
                if (Physics2D.Raycast(ledgeGrabFinder.transform.position, dir, ledgelength, LayerMask.GetMask("Platforms")) && !pointfound && !isWall && !ledgeGrabbed)
                {
                    hit = Physics2D.Raycast(ledgeGrabFinder.transform.position, dir, ledgelength, LayerMask.GetMask("Platforms"));

                    if (Physics2D.Raycast(headClearance.transform.position, Vector2.up, length, LayerMask.GetMask("Platforms")) == true)
                        return;

                    if (Physics2D.Raycast(ledgeGrabFinder.transform.position+new Vector3(0,0.5f,0), dir, ledgelength, LayerMask.GetMask("Platforms")) == true)
                        return;

                    if (hit.normal.y == 0)
                        pointfound = true;
                }
                
            }


            if (hit && pointfound && !ledgeGrabbed)
            {
                ledgeGrabbed = true;
                transform.position = hit.point;
                rb.gravityScale = 0;
                anim.SetTrigger("LedgeGrab");
            }
        }
            
        else
            return;

        
    }

    void WallFinder()
    {
        Debug.DrawRay(ledgeGrabFinder.transform.position + new Vector3(0, 0.15f, 0), dir* (ledgelength), Color.green);
        Debug.DrawRay(ledgeGrabFinder.transform.position + new Vector3(0, -0.15f, 0), dir* (ledgelength), Color.green);
        Debug.DrawRay(ledgeGrabFinder.transform.position, dir* (ledgelength), Color.green);
        if (Physics2D.Raycast(ledgeGrabFinder.transform.position, dir, ledgelength, LayerMask.GetMask("Platforms")) && !isGrounded
            && Physics2D.Raycast(ledgeGrabFinder.transform.position + new Vector3(0, 0.5f, 0), dir, ledgelength, LayerMask.GetMask("Platforms"))
            && Physics2D.Raycast(ledgeGrabFinder.transform.position + new Vector3(0, -0.5f, 0), dir, ledgelength, LayerMask.GetMask("Platforms")))
        {
            isWall = true;
        }
        else
        {
            isWall = false;
        }
    }
    #endregion
}
