using System;
using UnityEngine;
using UnityEngine.Events;

namespace ToolSpace
{
    public class Damageable : MonoBehaviour
    {
        [Serializable]
        public class HealthEvent : UnityEvent<Damageable>
        { }

        [Serializable]
        public class DamageEvent : UnityEvent<Damager, Damageable>
        { }

        [Serializable]
        public class HealEvent : UnityEvent<int, Damageable>
        { }

        [Serializable]
        public class HealRateEvent : UnityEvent<float>
        { }

        [ShowName("初始生命值")]
        public int startingHealth = 5;
        [ShowName("受伤后无敌")]
        public bool invulnerableAfterDamage = true;
        [ShowName("无敌时间")]
        public float invulnerabilityDuration = 3f;
        [ShowName("死亡后隐藏")]
        public bool disableOnDeath = false;
        [ShowName("攻击后恢复能量")]
        public int canIncreaseEnegy = 1;
        [Tooltip("An offset from the obejct position used to set from where the distance to the damager is computed")]
        public Vector2 centreOffset = new Vector2(0f, 1f);
        public HealthEvent OnHealthSet;
        public DamageEvent OnTakeDamage;
        public DamageEvent OnDie;
        public HealEvent OnGainHealth;
        public HealRateEvent OnHealthRateChange;

        protected bool m_Invulnerable;
        protected float m_InulnerabilityTimer;
        protected int m_CurrentHealth;
        protected Vector2 m_DamageDirection;
        protected bool m_ResetHealthOnSceneReload;

        public int CurrentHealth
        {
            get { return m_CurrentHealth; }
        }

        public bool IsInvulerable(){
            return m_Invulnerable;
        }


        private void Start()
        {
            m_CurrentHealth = startingHealth;
            OnHealthSet.Invoke(this);
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
            //technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
            m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
        }

        public void DisableInvulnerability()
        {
            m_Invulnerable = false;
        }

        public Vector2 GetDamageDirection()
        {
            return m_DamageDirection;
        }

        public void OnTouchSkill()
        {

        }

        public void TakeDamage(Damager damager, bool ignoreInvincible = false)
        {
            //print("TakeDamage/"+ m_Invulnerable+"/"+!ignoreInvincible+"/"+ m_CurrentHealth);
            if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
                return;

            //we can reach that point if the damager was one that was ignoring invincible state.
            //We still want the callback that we were hit, but not the damage to be removed from health.
            if (!m_Invulnerable)
            {
                m_CurrentHealth -= damager.damage;
                OnHealthSet.Invoke(this);
            }

            m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

            OnTakeDamage.Invoke(damager, this);
            EnableInvulnerability();
            if (m_CurrentHealth <= 0)
            {
                OnDie.Invoke(damager, this);
                m_ResetHealthOnSceneReload = true;

                if (disableOnDeath) gameObject.SetActive(false);
            }
            OnHealthRateChange.Invoke((float)m_CurrentHealth / (float)startingHealth);
        }

        public void GainHealth(int amount)
        {
            m_CurrentHealth += amount;

            if (m_CurrentHealth > startingHealth)
                m_CurrentHealth = startingHealth;

            OnHealthSet.Invoke(this);

            OnGainHealth.Invoke(amount, this);
            OnHealthRateChange.Invoke((float)m_CurrentHealth / (float)startingHealth);
        }

        public void SetHealth(int amount)
        {
            startingHealth = amount;
            m_CurrentHealth = amount;

            if (m_CurrentHealth <= 0)
            {
                OnDie.Invoke(null, this);
                m_ResetHealthOnSceneReload = true;
                EnableInvulnerability();
                if (disableOnDeath) gameObject.SetActive(false);
            }

            OnHealthSet.Invoke(this);
            OnHealthRateChange.Invoke((float)m_CurrentHealth / (float)startingHealth);
        }
    }
}
