using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D player_rigidbody;
    public Animator anim;
    public Flashlight_Controller flashlight_Controller;
    public ParticleSystem footstepdust,footstepdust_cloud;
    public float smoothing = 0.5f;
    private Vector3 v = Vector3.zero;
    [SerializeField]
    float input_x, input_y,jumpforce,speed;
    bool canTriggerDust = false;
    public Transform foot_location;
    bool isGrounded;
    public bool flashlight_toggle = false;
    public AudioSource audioSource;
    public AudioClip footsteps;
    public Material damage_Material;
    float heath = 100f;
    

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Original_Material = GetComponent<SpriteRenderer>().material;
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        player_rigidbody = GetComponent<Rigidbody2D>();
        flashlight_Controller.crosshair.GetComponent<SpriteRenderer>().enabled = false;
        flashlight_Controller.gameObject.SetActive(false);
        input_x = 0;
        input_y = 0;
    }

    Vector2 scale;

    void SetFlashLight_State(bool on)
    {
        flashlight_Controller.crosshair.GetComponent<SpriteRenderer>().enabled = on;
        flashlight_Controller.gameObject.SetActive(on);
    }

    // Update is called once per frame
    void Update()
    {
        

        isGrounded = Physics2D.OverlapCircle(foot_location.position, 0.1f);
        input_x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.T))
        {
            flashlight_toggle = !flashlight_toggle;
            SetFlashLight_State(flashlight_toggle);
            anim.SetBool("Flashlight_ON", flashlight_toggle);
        }

        Vector2 player_direction = new Vector2(input_x * speed, player_rigidbody.velocity.y);
        player_rigidbody.velocity = Vector3.Lerp(player_rigidbody.velocity , player_direction ,smoothing);

        scale = transform.localScale;

        if (flashlight_toggle)
        {
            if (flashlight_Controller.crosshair.transform.position.x > transform.position.x)
            {
                scale.x = 1;
            }
            else if ( flashlight_Controller.crosshair.transform.position.x < transform.position.x)
            {
                scale.x = -1;
            }
        }
        else
        {
            if (input_x > 0)
            {
                scale.x = 1;
            }
            else if (input_x < 0 )
            {
                scale.x = -1;
            }
        }
        

        transform.localScale = footstepdust.transform.localScale =scale;
        

        if (player_rigidbody.velocity.x!=0 && isGrounded)
        {
            anim.SetBool("Walk", true);
            if (audioSource.clip != footsteps || !audioSource.isPlaying)
            {
                audioSource.clip = footsteps;
                audioSource.Play();
            }
            
            footstepdust.enableEmission = true;
            flashlight_Controller.Setmoving(true);
        }
        else
        {
            audioSource.Stop();
            anim.SetBool("Walk", false);
            footstepdust.enableEmission = false;
            flashlight_Controller.Setmoving(false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            player_direction.y = 1;
            player_rigidbody.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
            
            anim.SetBool("Jump", true);
        }
        anim.SetFloat("Y_Velocity", player_rigidbody.velocity.y);
        if (player_rigidbody.velocity.y < -3 || player_rigidbody.velocity.y > 3)
            canTriggerDust = true;

        if (player_rigidbody.velocity.y == 0 && canTriggerDust  && isGrounded)
        {
            anim.SetBool("Jump", false);
            Instantiate(footstepdust_cloud, foot_location.position, Quaternion.identity);
            canTriggerDust = false;
        }

    }

    public Scrollbar healtbar;
    Material Original_Material;

    public IEnumerator DamagePlayer()
    {
        SpriteRenderer sp= GetComponent<SpriteRenderer>();
        heath -= 10f;
        healtbar.size = heath * 0.01f;
        sp.material = damage_Material;
        yield return new WaitForSeconds(.05f);
        sp.material = Original_Material;
        yield return new WaitForSeconds(.05f);
        sp.material = damage_Material;
        yield return new WaitForSeconds(.05f);
        sp.material = Original_Material;
        yield return new WaitForSeconds(.05f);
        sp.material = damage_Material;
        yield return new WaitForSeconds(.05f);
        sp.material = Original_Material;
    }

    
}
