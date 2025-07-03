using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Character : MonoBehaviour
    {
        public float maxHp;
        public float currentHp;
        public float damage;

        public float lastAttackTime;
        public float attackInterval;

        protected virtual void Start()
        {
            currentHp = maxHp;
        }

        public virtual void TakeDamage(float damage)
        {
            currentHp -= damage;
        }
    }
}