using UnityEngine;

namespace DapanzAI
{
    public class Damageable : MonoBehaviour
    {
        /// <summary>
        /// 防御点数
        /// </summary>
        public int startDefend = 0;
        /// <summary>
        /// 破防后恢复防御的时间
        /// </summary>
        public float defendRecoverAfterBreak = 1;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public int startingHealth = 5;
        /// <summary>
        /// 受伤后无敌
        /// </summary>
        public bool invulnerableAfterDamage = true;
        /// <summary>
        /// 无敌时间
        /// </summary>
        public float invulnerabilityDuration = 3f;
        private bool m_Invulnerable;
        private float m_InulnerabilityTimer;
        private int m_CurrentDefend;
        private int m_CurrentHealth;
        private float m_defendRecoverTimer;
        /// <summary>
        /// 当前生命
        /// </summary>
        public int CurrentHealth => m_CurrentHealth;
        /// <summary>
        /// 不会受到伤害
        /// </summary>
        public bool IsInvulerable => m_Invulnerable;
        /// <summary>
        /// 当前防御值
        /// </summary>
        public int CurrentDefend => m_CurrentDefend;
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// 获得生命
        /// </summary>
        /// <param name="amount">值</param>
        protected virtual void OnGetHealth(int amount) { }
        /// <summary>
        /// 格挡
        /// </summary>
        protected virtual void OnDefending(Damager damager) { }
        /// <summary>
        /// 破防
        /// </summary>
        protected virtual void OnBreakDefend(Damager damager) { }
        /// <summary>
        /// 被攻击
        /// </summary>
        /// <param name="damager"></param>
        protected virtual void OnGetHit(Damager damager) { }
        /// <summary>
        /// 死亡
        /// </summary>
        /// <param name="damager"></param>
        protected virtual void OnDie(Damager damager) { }
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

            if (m_CurrentDefend <= 0 && m_defendRecoverTimer >0)
            {
                m_defendRecoverTimer -= Time.deltaTime;
                if (m_defendRecoverTimer <= 0)
                {
                    m_CurrentDefend = startDefend;
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
            if (m_Invulnerable && !ignoreInvincible)
                return;

            

            if (startDefend >0 && m_CurrentDefend > 0)
            {
                ActivtDefend(damager);
                if (m_CurrentDefend >= 0)
                {
                    return;
                }
            }

            //受到伤害部分
            OnGetHit(damager);
            if (m_CurrentHealth > 0)
            {
                EnableInvulnerability();
                m_CurrentHealth -= damager.damage;
                if (m_CurrentHealth <= 0)
                {
                    OnDie(damager);
                }
            }
        }

        /// <summary>
        /// 得到治疗
        /// </summary>
        /// <param name="amount"></param>
        public void GainHealth(int amount)
        {
            m_CurrentHealth += amount;

            if (m_CurrentHealth > startingHealth)
                m_CurrentHealth = startingHealth;

            OnGetHealth(amount);
        }

        /// <summary>
        /// 设置生命值
        /// </summary>
        /// <param name="amount"></param>
        public void SetHealth(int amount)
        {
            startingHealth = amount;
            m_CurrentHealth = amount;

            if (m_CurrentHealth <= 0)
            {
                OnDie(null);
                EnableInvulnerability();
            }
        }

        /// <summary>
        /// 防御
        /// </summary>
        /// <param name="damager"></param>
        public void ActivtDefend(Damager damager)
        {
            OnDefending(damager);
            EnableInvulnerability();
            m_CurrentDefend -= damager.damage;
            if(m_CurrentDefend <= 0)
            {
                m_defendRecoverTimer = defendRecoverAfterBreak;
                OnBreakDefend(damager);
            }
        }
    }
}
