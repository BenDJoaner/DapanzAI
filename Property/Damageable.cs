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
        //[Name("最大生命值")]
        public int startingHealth = 5;
        //[Name("受伤后无敌")]
        /// <summary>
        /// 受伤后无敌
        /// </summary>
        public bool invulnerableAfterDamage = true;
        //[Name("无敌时间")]
        /// <summary>
        /// 无敌时间
        /// </summary>
        public float invulnerabilityDuration = 3f;
        private bool m_Invulnerable;
        private float m_InulnerabilityTimer;
        private int m_CurrentDefend;
        private int m_CurrentHealth;
        private float m_defendRecoverTimer;
        protected bool m_ResetHealthOnSceneReload;//重置场景时候需要初始化标记，用于对象池回收
        /// <summary>
        /// 当前生命
        /// </summary>
        public int CurrentHealth => m_CurrentHealth;
        /// <summary>
        /// 不会受到伤害
        /// </summary>
        public bool IsInvulerable => m_Invulnerable;
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
        }

        public void ActivtDefend()
        {

        }

        public void BreakDefend()
        {

        }
    }
}
