using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Spider : MonoBehaviour
{
    public GameObject deathparticles;
    public GameObject player;
    public Flashlight_Controller fc;
    public float health=100.0f;
    public bool isDamagable = false;

    public void Damagable(bool state)
    {
        isDamagable = state;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _timer = Random.Range(1f, 3f);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private RaycastHit2D hit;
    //private int layerMask = LayerMask.GetMask("Player", "Default");
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 20f)
        {
            ShootProjectile();
        }
        /*hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, layerMask);
        if (hit.transform.tag=="Player")
        {
            ShootProjectile();
        }*/
    }
    public GameObject projectile;
    private void ShootProjectile()
    {
        if (timer > 0)
        {
            timer -= 1f * Time.deltaTime;
        }
        else
        {
            Instantiate(projectile, transform.position + new Vector3(0,-1.5f,0), new Quaternion(0, 0, 0, 0));
            timer = _timer;
        }
    }

    public float _timer;
    private float timer;

    public void Damage(float intensity, GameObject tx,Vector3 point)
    {
        if (health >= 0)
        {
            if (intensity > 2f)
            {
               Instantiate(tx, point, new Quaternion(0, 0, 0, 0),transform);
                health -= intensity*Time.deltaTime*2f;
            }
        }
        else
        {
            Instantiate(deathparticles, transform.position,deathparticles.transform.rotation,transform.parent);
            Destroy(this.gameObject);
        }
    }
}
