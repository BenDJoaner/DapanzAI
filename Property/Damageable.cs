using System;
using UnityEngine;
using UnityEngine.Events;

namespace DapanzAI
{
    public class Damageable : MonoBehaviour
    {


        [ShowName("初始生命值")]
        public int startingHealth = 5;
        [ShowName("受伤后无敌")]
        public bool invulnerableAfterDamage = true;
        [ShowName("无敌时间")]
        public float invulnerabilityDuration = 3f;

        protected bool m_Invulnerable;
        protected float m_InulnerabilityTimer;
        protected int m_CurrentHealth;
        protected Vector2 m_DamageDirection;
        protected bool m_ResetHealthOnSceneReload;//重置场景时候需要初始化标记，用于对象池回收

        public int CurrentHealth => m_CurrentHealth;

        public bool IsInvulerable => m_Invulnerable;

        protected virtual void Init() { }
        protected virtual void OnGetHealth(int amount) { }
        protected virtual void OnGetHit(Damager damager) { }
        protected virtual void OnDie(Damager damager) { }
        protected virtual void OnSetHealth() { }
        private void Start()
        {
            m_CurrentHealth = startingHealth;
            Init();
        }
        void OnEnable()
        {
            DisableInvulnerability();
        }

        void Update()
        {
            if (m_Invulnerable)
            {
                m_InulnerabilityTimer -= Time.deltaTime;

                if (m_InulnerabilityTimer <= 0f)
                {
                    m_Invulnerable = false;
                }
            }
        }

        public void EnableInvulnerability(bool ignoreTimer = false)
        {
            m_Invulnerable = true;
            m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
        }

        public void DisableInvulnerability()
        {
            m_Invulnerable = false;
        }

        public void TakeDamage(Damager damager, bool ignoreInvincible = false)
        {

            if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
                return;

            m_CurrentHealth -= damager.damage;
            OnGetHit(damager);
            EnableInvulnerability();
            if (m_CurrentHealth <= 0)
            {
                OnDie(damager);
                m_ResetHealthOnSceneReload = true;
            }
        }

        public void GainHealth(int amount)
        {
            m_CurrentHealth += amount;

            if (m_CurrentHealth > startingHealth)
                m_CurrentHealth = startingHealth;

            OnGetHealth(amount);
        }

        public void SetHealth(int amount)
        {
            startingHealth = amount;
            m_CurrentHealth = amount;

            if (m_CurrentHealth <= 0)
            {
                OnDie(null);
                m_ResetHealthOnSceneReload = true;
                EnableInvulnerability();
            }
            OnSetHealth();
        }
    }
}
