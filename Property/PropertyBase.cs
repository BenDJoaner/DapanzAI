using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;
namespace DapanzAI
{
    public class PropertyBase : Damageable
    {

        [Serializable]
        public class DamageEvent : UnityEvent<Damager, Damageable>
        { }

        [Serializable]
        public class HealEvent : UnityEvent<int, Damageable>
        { }

        [Serializable]
        public class DefendEvent : UnityEvent<Damager>
        { }

        ////[Name("受到伤害")]
        public DamageEvent OnTakeDamage;
        ////[Name("死亡")]
        public DamageEvent OnDead;
        ////[Name("获得治疗")]
        public HealEvent OnGainHealth;
        public DefendEvent OnBreakDefendEvent;
        public DefendEvent OnDefendEvent;

        Coroutine m_FlickeringCoroutine = null;
        private Color m_OriginalColor;
        SpriteRenderer playerSprite;
        Animator pAnim;
        protected override void Init()
        {
            pAnim = GetComponentInChildren<Animator>();
            playerSprite = GetComponentInChildren<SpriteRenderer>();
            m_OriginalColor = playerSprite.color;
        }

        protected override void OnGetHit(Damager damager)
        {
            if (m_FlickeringCoroutine != null)
            {
                StopCoroutine(m_FlickeringCoroutine);
                playerSprite.color = m_OriginalColor;
            }
            m_FlickeringCoroutine = StartCoroutine(Flicker());

            OnTakeDamage.Invoke(damager, this);
        }

        protected IEnumerator Flicker()
        {
            float timer = 0f;
            float sinceLastChange = 0.1f;

            Color transparent = m_OriginalColor;
            transparent.a = 0.2f;
            int state = 1;

            playerSprite.color = transparent;

            while (timer < invulnerabilityDuration)
            {
                yield return null;
                timer += Time.deltaTime;
                sinceLastChange += Time.deltaTime;
                if (sinceLastChange > invulnerabilityDuration)
                {
                    sinceLastChange -= invulnerabilityDuration;
                    state = 1 - state;
                    playerSprite.color = state == 1 ? transparent : m_OriginalColor;
                }
            }

            playerSprite.color = m_OriginalColor;
        }

        protected override void OnDie(Damager damager)
        {
            OnDead.Invoke(damager, this);
        }

        protected override void OnGetHealth(int amount)
        {
            OnGainHealth.Invoke(amount, this);
        }

        protected override void OnBreakDefend(Damager damager)
        {
            OnBreakDefendEvent.Invoke(damager);
        }

        protected override void OnDefending(Damager damager)
        {
            OnBreakDefendEvent.Invoke(damager);
        }
    }
}
