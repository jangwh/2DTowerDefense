using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace TowerDefense
{
    public class Enemy : Character
    {
        Animator animator;

        private float moveSpeed;
        public float rayDistance;
        public string charName;
        private bool isAttack = false;

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
            Vector2 rayStartpos = new Vector2(transform.position.x, transform.position.y + 1f);
            //TODO: raycasthit 수정 완료하기
            RaycastHit2D rayHit = Physics2D.Raycast(rayStartpos, Vector2.left, rayDistance);
            Debug.DrawRay(rayStartpos, Vector2.left * rayDistance, new Color(1, 0, 0, 1));

            Move();

            if(rayHit.collider != null && rayHit.collider.CompareTag("Player"))
            {
                Stop();
            }
            else
            {
                GoSpeed();
            }
            if (Time.time > lastAttackTime + attackInterval)
            {
                moveSpeed = 0f;
                Attack(rayHit);
                lastAttackTime = Time.time;
            }

            Die();
            //TODO: 프리스트는 소환되자마자 animator.SetBool("isSpell", isSpell); 애니메이션 동작, GameManager에서 소환 담당?
        }
        public void Move()
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            animator.SetInteger("State", 2);
        }
        void GoSpeed()
        {
            moveSpeed = 2f;
        }
        void Stop()
        {
            moveSpeed = 0f;
            animator.SetInteger("State", 0);
        }
        void Attack(RaycastHit2D ray)
        {
            if (ray.collider != null)
            {
                //Debug.Log("Enemy Ray Hit: " + ray.collider.name);

                if (ray.collider.CompareTag("Player"))
                {
                    //Debug.Log("Enemy Hit Player!");

                    Playerable playerInfo = ray.collider.GetComponent<Playerable>();
                    playerInfo.TakeDamage(damage);

                    isAttack = true;
                    animator.SetBool("Attack", isAttack);
                }
            }
        }
        void Die()
        {
            if (currentHp <= 0)
            {
                animator.SetInteger("State", 9);
                //TODO: 오브젝트풀에서 회수
            }
        }

    }
}
