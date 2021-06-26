using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public class Enemy
    {
        protected Transform enemyObj;

        protected enum EnemyState
        {
            Idle,
            Attack,
            Walk,
            Hit,
            Dead,
            Block
        }

        public virtual void UpdateEnemy(Transform transform) { }


        Animator anim;
        protected float timer = 0;
        protected void DoAction(Transform transform, EnemyState state)
        {
            anim = enemyObj.GetComponentInChildren<Animator>();
            
            switch (state)
            {
                case EnemyState.Idle:
                    anim.SetBool("Idle", true);
                    anim.SetBool("Walk", false);
                    anim.SetBool("Block", false);
                    enemyObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, enemyObj.GetComponent<Rigidbody2D>().velocity.y);
                    break;
                case EnemyState.Walk:
                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);
                    anim.SetBool("Block", false);
                    enemyObj.GetComponent<Rigidbody2D>().velocity = new Vector2(enemyObj.localScale.x*2f, enemyObj.GetComponent<Rigidbody2D>().velocity.y);
                    break;
                case EnemyState.Attack:
                    if (timer > 0f)
                    {
                        anim.SetBool("Idle", true);
                        anim.SetBool("Walk", false);
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        if (transform.localScale.x == enemyObj.localScale.x)
                            enemyObj.localScale = new Vector3(transform.localScale.x * -1f, 1, 1);

                        anim.SetTrigger("Attack");
                        timer = Random.Range(1f, 3f);
                    }
                    break;
                case EnemyState.Dead:
                    anim.SetBool("Block", false);
                    anim.SetTrigger("Dead");
                    break;
                case EnemyState.Block:
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        anim.SetBool("Block", true);
                    }
                    else
                    {
                        DoAction(transform, EnemyState.Idle);
                    }
                    break;
            }
        }
    }
}

