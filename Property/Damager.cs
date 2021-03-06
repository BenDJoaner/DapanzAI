﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace DapanzAI
{
    public class Damager : MonoBehaviour
    {
        [Serializable]
        public class DamagableEvent : UnityEvent<Damager, Damageable>
        { }


        [Serializable]
        public class NonDamagableEvent : UnityEvent<Damager>
        { }

        //call that from inside the onDamageableHIt or OnNonDamageableHit to get what was hit.
        public Collider2D LastHit { get { return m_LastHit; } }

        ////[Name("伤害值")]
        /// <summary>
        /// 伤害值
        /// </summary>
        public int damage = 1;
        [Header("调整攻击效果范围")]
        ////[Name("坐标偏移")]
        public Vector2 offset = new Vector2(1.5f, 1f);
        ////[Name("大小缩放")]
        public Vector2 size = new Vector2(2.5f, 1f);

        /// <summary>
        /// 攻击后Damanger 失效
        /// </summary>
        public bool disableDamageAfterHit = false;
        /// <summary>
        /// 忽视无敌状态
        /// </summary>
        public bool ignoreInvincibility = false;
        ////[Name("可以伤害的Layer")]
        public LayerMask hittableLayers;
        public DamagableEvent OnDamageableHit;
        public NonDamagableEvent OnNonDamageableHit;
        public NonDamagableEvent OnActiveDamage;

        protected bool m_SpriteOriginallyFlipped;
        protected bool m_CanDamage = true;
        protected ContactFilter2D m_AttackContactFilter;
        protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
        protected Transform m_DamagerTransform;
        protected Collider2D m_LastHit;

        void Awake()
        {
            m_AttackContactFilter.layerMask = hittableLayers;//设置层级
            m_AttackContactFilter.useLayerMask = true;//根据层级

            m_DamagerTransform = transform;
        }

        public void EnableDamage()
        {
            m_CanDamage = true;
            OnActiveDamage.Invoke(this);
        }

        public void DisableDamage()
        {
            m_CanDamage = false;
        }

        void FixedUpdate()
        {
            if (!m_CanDamage)
                return;

            Vector2 scale = m_DamagerTransform.lossyScale;

            Vector2 facingOffset = Vector2.Scale(offset, scale);
            Vector2 scaledSize = Vector2.Scale(size, scale);

            Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
            Vector2 pointB = pointA + scaledSize;

            int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);

            for (int i = 0; i < hitCount; i++)
            {
                m_LastHit = m_AttackOverlapResults[i];
                Damageable damageable = m_LastHit.GetComponent<Damageable>();

                if (damageable && damageable.CurrentHealth > 0)
                {
                    OnDamageableHit.Invoke(this, damageable);
                    damageable.TakeDamage(this, ignoreInvincibility);
                    if (disableDamageAfterHit)
                        DisableDamage();
                }
                else
                {
                    OnNonDamageableHit.Invoke(this);
                }
            }
        }
    }
}
