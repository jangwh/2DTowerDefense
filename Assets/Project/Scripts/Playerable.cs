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
        public string charName;
        private bool isAttack = false;
        private bool isDie = false;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            Vector2 rayStartpos = new Vector2(transform.position.x, transform.position.y + 0.2f);
            //TODO: raycasthit 수정 완료하기
            RaycastHit2D rayHit = Physics2D.Raycast(rayStartpos, Vector2.right, rayDistance);
            Debug.DrawRay(rayStartpos, Vector2.right * rayDistance, new Color(0, 1, 0, 1), rayDistance);

            if (Time.time > lastAttackTime + attackInterval)
            {
                if(charName != "Priest")
                {
                    Attack(rayHit);
                    lastAttackTime = Time.time;
                }
            }

            Die();
            //TODO: 프리스트는 소환되자마자 animator.SetBool("isSpell", isSpell); 애니메이션 동작, GameManager에서 소환 담당?
        }
        void Attack(RaycastHit2D ray)
        {
            //Debug.Log(">> Player 공격 시도");

            if (ray.collider != null && ray.collider.CompareTag("Enemy"))
            {
                //Debug.Log("Player Ray Hit: " + ray.collider.name);

                Enemy enemyInfo = ray.collider.GetComponent<Enemy>();
                isAttack = true;
                animator.SetBool("isAttack", isAttack);
                //Debug.Log(">> Enemy에게 데미지 줌!");
                enemyInfo.TakeDamage(damage);
            }
            else
            {
                isAttack = false;
                animator.SetBool("isAttack", isAttack);
            }

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
    }
}
