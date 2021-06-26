using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health = 100;
    bool dead = false;

    void Update()
    {
        if (dead)
            return;

        if (health <= 0)
        {
            dead = true;
            foreach (Collider2D c in GetComponents<Collider2D>())
            {
                c.enabled = false;
            }
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
