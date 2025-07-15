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
        AudioSource audioSource;
        public DropTower dropTower;
        public AudioClip[] audioClip;

        public float rayDistance;
        public string charName;

        private bool isAttack = false;
        private bool isDie = false;
        private bool isSpell = false;

        public void Init(DropTower dropTower)
        {
            this.dropTower = dropTower;
        }
        public void InitTower(TowerData data)
        {
            damage = data.damage;
            maxHp = data.MaxHp;
            currentHp = maxHp;
            charName = data.towerName;

            Debug.Log($" InitTower 완료: {charName}, 공격력: {damage}, 체력: {maxHp}");
        }
        void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            Vector2 rayStartpos = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(rayStartpos, Vector2.right, rayDistance);
            Debug.DrawRay(rayStartpos, Vector2.right * rayDistance, new Color(0, 1, 0, 1), rayDistance);

            if (Time.time > lastAttackTime + attackInterval)
            {
                if(charName != "priest")
                {
                    Attack(rayHit);
                    lastAttackTime = Time.time;
                }
                else if(charName == "priest")
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
                switch(charName)
                {
                    case "knight":
                        audioSource.PlayOneShot(audioClip[0]);
                        break;
                    case "archer":
                        audioSource.PlayOneShot(audioClip[1]);
                        break;
                    case "soldier":
                        audioSource.PlayOneShot(audioClip[0]);
                        break;
                    case "thief":
                        audioSource.PlayOneShot(audioClip[0]);
                        break;
                }
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
            if (isDie) return;
            if(currentHp <= 0)
            {
                isDie = true;
                animator.SetBool("isDie", isDie);
                StartCoroutine(DieAnimation()); 
                switch (charName)
                {
                    case "knight":
                        audioSource.PlayOneShot(audioClip[2]);
                        break;
                    case "archer":
                        audioSource.PlayOneShot(audioClip[3]);
                        break;
                    case "Priest":
                        audioSource.PlayOneShot(audioClip[4]);
                        GameManager.priestNum--;
                        break;
                    case "":
                        audioSource.PlayOneShot(audioClip[2]);
                        break;
                    case "thief":
                        audioSource.PlayOneShot(audioClip[2]);
                        break;
                }
            }
        }
        void Revive()
        {
            currentHp = maxHp;
            isDie = false;
            dropTower.receivingImage.overrideSprite = null;
        }
        IEnumerator DieAnimation()
        {
            yield return new WaitForSeconds(1f);
            Revive();
            LeanPool.Despawn(this);
        }
    }
}
