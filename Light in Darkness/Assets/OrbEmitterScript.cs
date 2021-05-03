using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbEmitterScript : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject orb;
    bool triggered = false;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.IsAlive(true)&&triggered==false)
        {
            StartCoroutine(SpawnOrb());
        }
        
    }

    IEnumerator SpawnOrb()
    {
        triggered = true;
        yield return new WaitForSecondsRealtime(2.3f);
        Instantiate(orb, transform.position, Quaternion.identity);
    }
}
