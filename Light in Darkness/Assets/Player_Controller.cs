using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D player_rigidbody;
    public Animator anim;
    public Flashlight_Controller flashlight_Controller;
    public ParticleSystem footsteepdust;

    [SerializeField]
    float input_x, input_y,force;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player_rigidbody = GetComponent<Rigidbody2D>();
        input_x = 0;
        input_y = 0;
    }

    Vector2 scale;

    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        
        Vector2 player_direction = new Vector2(input_x, input_y);
        player_rigidbody.AddForce(player_direction * force, ForceMode2D.Force);

        scale = transform.localScale;

        if (input_x > 0 || flashlight_Controller.mouseScreenPosition.x > transform.position.x)
        {
            scale.x = 1;
        }
        else if (input_x < 0 || flashlight_Controller.mouseScreenPosition.x < transform.position.x)
        {
            scale.x = -1;
        }

        transform.localScale = footsteepdust.transform.localScale =scale;

        if (player_rigidbody.velocity.x==0)
        {
            footsteepdust.gameObject.SetActive(false);
        }
        else
        {
            footsteepdust.gameObject.SetActive(true);
        }

        if (player_rigidbody.velocity.x != 0)
        {
            flashlight_Controller.Setmoving(true);
            anim.SetBool("Walk", true);
        }
        else
        {
            flashlight_Controller.Setmoving(false);
            anim.SetBool("Walk", false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player_rigidbody.AddForce(transform.up * 5f, ForceMode2D.Impulse);
        }

    }
}
