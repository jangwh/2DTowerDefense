using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense
{
    public class Playerable : Character
    {

        Animator animator;

        public float rayDistance;
        private bool isAttack = false;
        private bool isDie = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, LayerMask.GetMask("Enemy"));

            if(LayerMask.GetMask("Enemy") == 6)
            {
                Attack(rayHit);
            }

            Die();
            //TODO: 프리스트는 소환되자마자 animator.SetBool("isSpell", isSpell); 애니메이션 동작, GameManager에서 소환 담당?
        }
        void Attack(RaycastHit2D rayHit)
        {
            Character enemyInfo = rayHit.collider.GetComponent<Character>();
            if (Time.time < lastAttackTime + attackInterval) return;
            lastAttackTime = Time.time;
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
            enemyInfo.TakeDamage(damage);
        }
        void Die()
        { 
            if(currentHp <= 0)
            {
                isDie = true;
                animator.SetBool("isDie", isDie);
                //TODO: 오브젝트풀에서 회수
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                isAttack = false;
                animator.SetBool("isAttack", isAttack);
            }
        }
    }
}
