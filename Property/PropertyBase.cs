using System;
using System.Collections;
using UnityEngine;

namespace DapanzAI
{
    public class PropertyBase : Damageable
    {

        Coroutine m_FlickeringCoroutine = null;
        Color m_OriginalColor;
        SpriteRenderer playerSprite;
        Animator pAnim;
        protected override void Init()
        {
            pAnim = GetComponentInChildren<Animator>();
            playerSprite = GetComponentInChildren<SpriteRenderer>();
            m_OriginalColor = playerSprite.color;
            pAnim.SetTrigger("dead");
        }

        protected override void OnGetHit(Damager damager)
        {
            if (m_FlickeringCoroutine != null)
            {
                StopCoroutine(m_FlickeringCoroutine);
                playerSprite.color = m_OriginalColor;
            }
            m_FlickeringCoroutine = StartCoroutine(Flicker());
        }

        protected IEnumerator Flicker()
        {
            float timer = 0f;
            float sinceLastChange = 0.0f;

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
    }
}
