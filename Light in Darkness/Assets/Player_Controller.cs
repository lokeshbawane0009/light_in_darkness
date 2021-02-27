using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D player_rigidbody;
    public Animator anim;


    [SerializeField]
    float input_x, input_y;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
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
        player_rigidbody.AddForce(player_direction * 2f,ForceMode2D.Force);

        scale = transform.localScale;

        if (input_x > 0)
            scale.x = 1;
        else if (input_x < 0)
            scale.x = -1;

        transform.localScale = scale;

        if (player_rigidbody.velocity.x != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player_rigidbody.AddForce(transform.up * 5f, ForceMode2D.Impulse);
        }
    }
}
