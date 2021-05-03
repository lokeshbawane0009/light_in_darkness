using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Script : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
        
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        anim.SetBool("Exit", true);
        Destroy(gameObject, 1f);
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Damage Player");
            StartCoroutine(collision.transform.GetComponent<Player_Controller>().DamagePlayer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
