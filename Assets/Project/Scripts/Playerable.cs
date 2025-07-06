using Lean.Pool;
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

        public float prayTime;
        private bool isAttack = false;
        private bool isDie = false;
        private bool isSpell = false;

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
            RaycastHit2D rayHit = Physics2D.Raycast(rayStartpos, Vector2.right, rayDistance);
            Debug.DrawRay(rayStartpos, Vector2.right * rayDistance, new Color(0, 1, 0, 1), rayDistance);

            if (Time.time > lastAttackTime + attackInterval)
            {
                if(charName != "Priest")
                {
                    Attack(rayHit);
                    lastAttackTime = Time.time;
                }
                else if(charName == "Priest")
                {
                    isSpell = true;
                    animator.SetBool("isSpell", isSpell);
                }
            }

            Die();
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
                StartCoroutine(DieAnimation());
            }
        }
        IEnumerator DieAnimation()
        {
            yield return new WaitForSeconds(1.5f);
            LeanPool.Despawn(this);
        }
    }
}
