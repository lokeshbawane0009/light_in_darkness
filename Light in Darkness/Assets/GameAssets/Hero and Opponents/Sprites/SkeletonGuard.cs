using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public class SkeletonGuard : Enemy
    {
        EnemyState currentState = EnemyState.Idle;

        Transform player;

        bool dead = false;

        float waitTime = 0;

        HealthSystem healthSystem;

        public SkeletonGuard(Transform skeletonObj, HealthSystem hs)
        {
            base.enemyObj = skeletonObj;
            healthSystem = hs;
        }

        protected EnemyState CurrentState { get => currentState; set => currentState = value; }


        public override void UpdateEnemy(Transform transform)
        {
            if (dead)
                return;

            if (healthSystem.health <= 0)
            {
                currentState = EnemyState.Dead;
                dead = true;
            }

            player = transform;
            float distance = Vector2.Distance(transform.position, enemyObj.position);

            if (healthSystem.health < 50 && distance<3.5f && transform.localScale!=enemyObj.localScale)
            {
                waitTime=base.timer = Random.Range(1f, 3f);
                currentState = EnemyState.Block;
            }

            switch (currentState)
            {
                case EnemyState.Idle:
                    if (distance < 5f && distance > 3.5f)
                    {
                        currentState = EnemyState.Walk;
                    }
                    else if (distance < 3.5f)
                    {
                        currentState = EnemyState.Attack;
                    }
                    break;
                case EnemyState.Walk:
                    DoTurnCheck();
                    base.timer = 1f;
                    if (distance > 10f)
                    {
                        currentState = EnemyState.Idle;
                    }
                    else if (distance < 3.5f)
                    {
                        currentState = EnemyState.Attack;
                    }
                    break;
                case EnemyState.Attack:
                    if (distance > 3.5f)
                    {
                        currentState = EnemyState.Walk;
                    }
                    else if (distance > 10f)
                    {
                        currentState = EnemyState.Idle;
                    }
                    break;
                case EnemyState.Dead:
                    break;
                case EnemyState.Hit:
                    break;
                case EnemyState.Block:
                    if (waitTime > 0)
                    {
                        waitTime -= Time.deltaTime;
                    }
                    else
                    {
                        currentState = EnemyState.Idle;
                    }
                    break;
            }
            DoAction(transform, currentState);
        }

        void DoTurnCheck()
        {
            if (base.enemyObj.position.x < player.position.x)
            {
                base.enemyObj.localScale = new Vector3(1, 1, 1);
            }
            else if (base.enemyObj.position.x > player.position.x)
            {
                base.enemyObj.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}

