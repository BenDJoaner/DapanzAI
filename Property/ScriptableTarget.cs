using System;
using UnityEngine.Events;

namespace DapanzAI
{
    class ScriptableTarget:Damageable
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

        [Name("设置生命值")]
        public HealthEvent OnHealthSet;
        [Name("受到伤害")]
        public DamageEvent OnTakeDamage;
        [Name("死亡")]
        public DamageEvent OnDead;
        [Name("获得治疗")]
        public HealEvent OnGainHealth;

        protected override void Init()
        {
            OnHealthSet.Invoke(this);
        }

        protected override void OnGetHealth(int amount)
        {
            OnGainHealth.Invoke(amount, this);
            OnHealthSet.Invoke(this);
        }

        protected override void OnGetHit(Damager damager)
        {
            OnTakeDamage.Invoke(damager, this);
            OnHealthSet.Invoke(this);
        }

        protected override void OnDie(Damager damager)
        {
            OnDead.Invoke(damager, this);
        }

        protected override void OnSetHealth()
        {
            OnHealthSet.Invoke(this);
        }

    }
}
