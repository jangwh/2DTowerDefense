using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Lean.Pool;


namespace TowerDefense
{
    public class Enemy : Character
    {
        Animator animator;

        private float moveSpeed;
        public float rayDistance;
        public string charName;
        private bool isAttack = false;
        private bool isDead = false;

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
            if (isDead) return;
            Vector2 rayStartpos = new Vector2(transform.position.x - 1f, transform.position.y + 1f);
            RaycastHit2D rayHit = Physics2D.Raycast(rayStartpos, Vector2.left, rayDistance);
            Debug.DrawRay(rayStartpos, Vector2.left * rayDistance, new Color(1, 0, 0, 1));

            Move();

            if(rayHit.collider != null && (rayHit.collider.CompareTag("Player") || rayHit.collider.CompareTag("Enemy")))
            {
                Stop();
            }
            else
            {
                GoSpeed();
            }
            if ((Time.time > lastAttackTime + attackInterval) && !isDead)
            {
                moveSpeed = 0f;
                Attack(rayHit);
                lastAttackTime = Time.time;
            }

            Die();
        }
        public void Move()
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            animator.SetInteger("State", 2);
        }
        void GoSpeed()
        {
            moveSpeed = 0.5f;
        }
        void Stop()
        {
            moveSpeed = 0f;
            animator.SetInteger("State", 0);
        }
        void Attack(RaycastHit2D ray)
        {
            if (ray.collider != null && isDead == false)
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
            if (isDead) return;
            if (currentHp <= 0)
            {
                isDead = true;
                switch (charName)
                {
                    case "Zombie":
                        ScoreSave.currentScore += 10;
                        break;
                    case "Ogre":
                        ScoreSave.currentScore += 30;
                        break;
                    case "Dreadnought":
                        ScoreSave.currentScore += 50;
                        break;
                }
                moveSpeed = 0f;
                animator.SetInteger("State", 9);
                StartCoroutine(DieAnimation());
            }
        }
        public void Init()
        {
            currentHp = maxHp;
            isDead = false;
            animator.SetInteger("State", 0);
            moveSpeed = 0.5f;
        }
        IEnumerator DieAnimation()
        {
            yield return new WaitForSeconds(1.5f);
            GameManager.Instance.enemySpawnCount--;
            LeanPool.Despawn(this);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("LifeZone"))
            {
                GameManager.Instance.currentLifeCount--;
                StartCoroutine(DieAnimation());
            }
        }
    }
}
