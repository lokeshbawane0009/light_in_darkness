using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class SwordWeapon : MonoBehaviour
{
    public string enemeyTag;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemeyTag && collision.GetComponent<HealthSystem>().health>0)
        {
            if(collision.GetComponentInChildren<Animator>().GetBool("Block")==false || collision.transform.localScale.x==transform.parent.localScale.x)
            {
                collision.GetComponent<HealthSystem>().health -= 10;
                collision.GetComponentInChildren<Animator>().SetTrigger("Hit");
            }
        }
    }
}
