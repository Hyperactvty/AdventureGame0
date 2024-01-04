using UnityEngine;

// namespace Unity.FPS.Game
// {
    public class Damageable : MonoBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;

        [Range(0, 1)] [Tooltip("Multiplier to apply to self damage")]
        public float SensibilityToSelfdamage = 0f;

        public Health Health { get; private set; }

        /* prior private */ public float m_LastTimeDamaged = float.NegativeInfinity;
        /* prior private */ public float invulnerabilityThresh=0.3f;

        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
        }

        public void InflictDamage(float damage, bool isCritical, bool isExplosionDamage, GameObject damageSource)
        {
            if (Health)
            {
                // Invulnerability Threshhold so that the object doesn't receive multiple damage per frame
                if(((Time.time - m_LastTimeDamaged) / invulnerabilityThresh) < 1) {
                  // Debug.Log($"Attempted to Hit @ {m_LastTimeDamaged} ->\t {(Time.time - m_LastTimeDamaged) / invulnerabilityThresh} remains...");
                  return;
                }
                m_LastTimeDamaged = Time.time;

                var totalDamage = damage;

                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                {
                    totalDamage *= DamageMultiplier;
                }

                // potentially reduce damages if inflicted by self
                if (Health.gameObject == damageSource)
                {
                    totalDamage *= SensibilityToSelfdamage;
                }

                // apply the damages
                Health.TakeDamage(totalDamage, damageSource);
            }
        }
    }
// }