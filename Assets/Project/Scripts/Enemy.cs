using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Enemy : Character
    {

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (Time.time < lastAttackTime + attackInterval) return;
                lastAttackTime = Time.time;
                TakeDamage(other.GetComponent<Character>().damage);
            }
        }
    }
}
