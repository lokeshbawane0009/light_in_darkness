using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public class EnemyControllerManager : MonoBehaviour
    {
        public GameObject[] skeletons;
        public GameObject[] skeletonsGuard;
        public GameObject playerObj;
        public List<Enemy> enemies = new List<Enemy>();
        // Start is called before the first frame update
        void Start()
        {
            foreach (GameObject e in skeletons)
                enemies.Add(new Skeleton(e.transform,e.GetComponent<HealthSystem>()));
            foreach (GameObject e in skeletonsGuard)
                enemies.Add(new SkeletonGuard(e.transform, e.GetComponent<HealthSystem>()));
        }

        // Update is called once per frame
        void Update()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.UpdateEnemy(playerObj.transform);
            }
        }
    }
}

