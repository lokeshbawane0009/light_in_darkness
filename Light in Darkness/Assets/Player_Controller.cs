using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D player_rigidbody;

    [SerializeField]
    float input_x, input_y;

    // Start is called before the first frame update
    void Start()
    {
        player_rigidbody = GetComponent<Rigidbody2D>();
        input_x = 0;
        input_y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        Vector2 player_direction = new Vector2(input_x, input_y);
        player_rigidbody.AddForce(player_direction * 2f);
    }
}
