using System.Collections.Generic;
// using Unity.FPS.Game;
using UnityEngine;

// namespace Unity.FPS.Gameplay
// {
    public class ProjectileStandard : ProjectileBase
    {
        [Header("General")] [Tooltip("Radius of this projectile's collision detection")]
        public float Radius = 0.01f;

        [Tooltip("Transform representing the root of the projectile (used for accurate collision detection)")]
        public Transform Root;

        [Tooltip("Transform representing the tip of the projectile (used for accurate collision detection)")]
        public Transform Tip;

        [Tooltip("Fixes the angle of the projectile when fired)")]
        public float projectileAngleFix = 90f;

        [Tooltip("LifeTime of the projectile")]
        public float MaxLifeTime = 5f;

        [Tooltip("Determines if the projectile inherits the velocity that the weapon's muzzle had when firing")]
        public bool DestroyOnCollision = false;

        [Tooltip("VFX prefab to spawn upon impact")]
        public GameObject ImpactVfx;

        [Tooltip("LifeTime of the VFX before being destroyed")]
        public float ImpactVfxLifetime = 5f;

        [Tooltip("Offset along the hit normal where the VFX will be spawned")]
        public float ImpactVfxSpawnOffset = 0.1f;

        [Tooltip("Clip to play on impact")] 
        public AudioClip ImpactSfxClip;

        [Tooltip("Layers this projectile can collide with")]
        public LayerMask HittableLayers = -1;

        [Header("Movement")] [Tooltip("Speed of the projectile")]
        public float Speed = 20f;

        [Tooltip("Downward acceleration from gravity")]
        public float GravityDownAcceleration = 0f;

        [Tooltip(
            "Distance over which the projectile will correct its course to fit the intended trajectory (used to drift projectiles towards center of screen in First Person view). At values under 0, there is no correction")]
        public float TrajectoryCorrectionDistance = -1;

        [Tooltip("Determines if the projectile inherits the velocity that the weapon's muzzle had when firing")]
        public bool InheritWeaponVelocity = false;

        [Header("Damage")] [Tooltip("Damage of the projectile")]
        public float Damage = 40f;

        [Tooltip("Area of damage. Keep empty if you don<t want area damage")]
        public DamageArea AreaOfDamage;

        [Header("Debug")] [Tooltip("Color of the projectile radius debug view")]
        public Color RadiusColor = Color.cyan * 0.2f;

        ProjectileBase m_ProjectileBase;
        Vector3 m_LastRootPosition;
        Vector3 m_Velocity;
        bool m_HasTrajectoryOverride;
        float m_ShootTime;
        Vector3 m_TrajectoryCorrectionVector;
        Vector3 m_ConsumedTrajectoryCorrectionVector;
        List<Collider2D> m_IgnoredColliders;

        Vector3 orientTowards;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        void OnEnable()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
            DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ProjectileStandard>(m_ProjectileBase, this,
                gameObject);

            m_ProjectileBase.OnShoot += OnShoot;

            Destroy(gameObject, MaxLifeTime);
        }

        new void OnShoot()
        {
            m_ShootTime = Time.time;
            m_LastRootPosition = Root.position;
            m_Velocity = transform.up * Speed;
            // Make projectile go towards WHERE IT NEEDS TO GO
            Transform wep = m_ProjectileBase.Owner.transform;
            Vector3 oT = new Vector3(0,0,0); Vector2 enemyAngle = new Vector2(0f,0f);
            switch (m_ProjectileBase.Owner.tag)
            {
              case "Player":
                // TODO: GetComponentsInChildren<WeaponBase>()[selectedWeaponIndex]
                wep = m_ProjectileBase.Owner.GetComponentInChildren<WeaponBase>()?.transform;//.FindObjectOfType();//.transform.GetChild(1);
                oT = wep.transform.eulerAngles;
                orientTowards = new Vector3(oT.x, oT.y, oT.z+projectileAngleFix);
                break;
              case "Enemy":
                EnemySpriteController _esc = m_ProjectileBase.Owner.GetComponent<EnemySpriteController>();
                orientTowards = new Vector3(0,0, _esc.angle+projectileAngleFix);
                enemyAngle = new Vector2(Mathf.Cos(((_esc.angle-90f) * Mathf.Deg2Rad)), Mathf.Sin(((_esc.angle-90f) * Mathf.Deg2Rad)));
                break;
              default:
                break;
            }

            if(false)
            {
              if(m_ProjectileBase.Owner.tag=="Player") 
              {
                wep = m_ProjectileBase.Owner.transform.GetChild(1);
              }
              oT = wep.transform.eulerAngles;
              orientTowards = new Vector3(oT.x, oT.y, oT.z+projectileAngleFix);
            }

            /* Working Mostly */ // orientTowards = (m_ProjectileBase.Owner.transform.GetChild(1).transform.eulerAngles);

            // orientTowards = (m_ProjectileBase.Owner.transform.Find("PlayerCenterPoint").transform.eulerAngles);

            m_IgnoredColliders = new List<Collider2D>();
            transform.position = (Vector2)transform.position + m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;

            Vector2 direction = new Vector2(0f,0f);
            switch (m_ProjectileBase.Owner.tag)
            {
              case "Player":
                direction = (Vector2)transform.position - (Vector2)m_ProjectileBase.Owner.transform.position;
                transform.eulerAngles = new Vector3(0, 0, orientTowards.z);
                transform.GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed;
                break;
              case "Enemy":
                direction = ((Vector2)transform.position - (Vector2)m_ProjectileBase.Owner.transform.position) - (enemyAngle/* - Vector2.up*/);
                transform.eulerAngles = new Vector3(0, 0, orientTowards.z);
                transform.GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed;
                break;
              default:
                break;
            }

            // Making the projectile... project
            if(false)
            {
              /*Vector2 */direction = (Vector2)transform.position - (Vector2)m_ProjectileBase.Owner.transform.position;
              transform.eulerAngles = new Vector3(0, 0, orientTowards.z);
              transform.GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed;
            }

            // transform.position += m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;

            // Ignore colliders of owner
            Collider2D[] ownerColliders = m_ProjectileBase.Owner.GetComponentsInChildren<Collider2D>();
            m_IgnoredColliders.AddRange(ownerColliders);

            // Handle case of player shooting (make projectiles not go through walls, and remember center-of-screen trajectory)
            // PlayerWeaponsManager playerWeaponsManager = m_ProjectileBase.Owner.GetComponent<PlayerWeaponsManager>();
            // if (playerWeaponsManager)
            // {
            //     m_HasTrajectoryOverride = true;

            //     Vector3 cameraToMuzzle = (m_ProjectileBase.InitialPosition -
            //                               playerWeaponsManager.WeaponCamera.transform.position);

            //     m_TrajectoryCorrectionVector = Vector3.ProjectOnPlane(-cameraToMuzzle,
            //         playerWeaponsManager.WeaponCamera.transform.forward);
            //     if (TrajectoryCorrectionDistance == 0)
            //     {
            //         transform.position += m_TrajectoryCorrectionVector;
            //         m_ConsumedTrajectoryCorrectionVector = m_TrajectoryCorrectionVector;
            //     }
            //     else if (TrajectoryCorrectionDistance < 0)
            //     {
            //         m_HasTrajectoryOverride = false;
            //     }

            //     if (Physics.Raycast(playerWeaponsManager.WeaponCamera.transform.position, cameraToMuzzle.normalized,
            //         out RaycastHit hit, cameraToMuzzle.magnitude, HittableLayers, k_TriggerInteraction))
            //     {
            //         if (IsHitValid(hit))
            //         {
            //             OnHit(hit.point, hit.normal, hit.collider);
            //         }
            //     }
            // }
        }

        void Update()
        {
            // Move

            // transform.position = (Vector2)transform.position + (Vector2)m_Velocity * Time.deltaTime;
            // transform.position += m_Velocity * Time.deltaTime;

            if (InheritWeaponVelocity)
            {
                transform.position = (Vector2)transform.position + (Vector2)m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;
                // transform.position += m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;
            }

            // Drift towards trajectory override (this is so that projectiles can be centered 
            // with the camera center even though the actual weapon is offset)
            if (m_HasTrajectoryOverride && m_ConsumedTrajectoryCorrectionVector.sqrMagnitude <
                m_TrajectoryCorrectionVector.sqrMagnitude)
            {
                Vector3 correctionLeft = m_TrajectoryCorrectionVector - m_ConsumedTrajectoryCorrectionVector;
                float distanceThisFrame = (Root.position - m_LastRootPosition).magnitude;
                Vector3 correctionThisFrame =
                    (distanceThisFrame / TrajectoryCorrectionDistance) * m_TrajectoryCorrectionVector;
                correctionThisFrame = Vector3.ClampMagnitude(correctionThisFrame, correctionLeft.magnitude);
                m_ConsumedTrajectoryCorrectionVector += correctionThisFrame;

                // Detect end of correction
                if (m_ConsumedTrajectoryCorrectionVector.sqrMagnitude == m_TrajectoryCorrectionVector.sqrMagnitude)
                {
                    m_HasTrajectoryOverride = false;
                }

                transform.position += correctionThisFrame;
            }

            // Orient towards velocity
            /* For hopefully 2D */ //transform.up = m_Velocity.normalized;
            // transform.forward = m_Velocity.normalized;

            // Gravity
            if (GravityDownAcceleration > 0)
            {
                // add gravity to the projectile velocity for ballistic effect
                m_Velocity += Vector3.forward * GravityDownAcceleration * Time.deltaTime;
            }

            // Hit detection
            {
                RaycastHit2D closestHit = new RaycastHit2D();
                closestHit.distance = Mathf.Infinity;
                bool foundHit = false;

                // Sphere cast
                Vector2 displacementSinceLastFrame = Tip.position - m_LastRootPosition;
                RaycastHit2D[] hits = Physics2D.CircleCastAll(m_LastRootPosition, Radius,
                    displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, HittableLayers
                    /*k_TriggerInteraction*/);
                foreach (var hit in hits)
                {
                    if (IsHitValid(hit) && hit.distance < closestHit.distance)
                    {
                        foundHit = true;
                        closestHit = hit;
                    }
                }

                if (foundHit)
                {
                    // Handle case of casting while already inside a collider
                    if (closestHit.distance <= 0f)
                    {
                        closestHit.point = Root.position;
                        closestHit.normal = -transform.forward;
                    }

                    OnHit(closestHit.point, closestHit.normal, closestHit.collider);
                }
            }

            m_LastRootPosition = Root.position;
        }

        bool IsHitValid(RaycastHit2D hit)
        {
            // ignore hits with an ignore component
            if (hit.collider.GetComponent<IgnoreHitDetection>())
            {
                return false;
            }

            // ignore hits with triggers that don't have a Damageable component
            if (hit.collider.isTrigger && hit.collider.GetComponent<Damageable>() == null)
            {
                return false;
            }

            // ignore hits with triggers that are within `invulnerabilityThresh` Damageable component
            if (hit.collider.GetComponent<Damageable>())
            {
              Damageable tdmg = hit.collider.GetComponent<Damageable>();
              if(((Time.time - tdmg.m_LastTimeDamaged) / tdmg.invulnerabilityThresh) < 1) { return false; }
            }

            // ignore hits with specific ignored colliders (self colliders, by default)
            if (m_IgnoredColliders != null && m_IgnoredColliders.Contains(hit.collider))
            {
                return false;
            }

            return true;
        }

        void OnHit(Vector3 point, Vector3 normal, Collider2D collider)
        {
            System.Random rand = new System.Random();
            double rv = rand.NextDouble(/*0f,100f*/);
            bool shouldCrit=false;
            float CalcDamage=0f;

            switch (Owner.tag)
            {
              case "Player":
                PlayerStats psb = Owner.GetComponent<PlayerStatsBase>().playerStats;
                PlayerCharacterController pcc = Owner.GetComponent<PlayerCharacterController>();
                shouldCrit= rv < (psb.Crit_Hit/100f);
                // CalcDamage = Mathf.CeilToInt((Damage + psb.Attack) * (shouldCrit ? (psb.Crit_Damage/100f)+1f : 1f));
                CalcDamage = Mathf.Ceil((Damage + psb.Attack) * (shouldCrit ? (psb.Crit_Damage/100f)+1f : 1f));
                pcc.DamageDealt += CalcDamage;

                LevelClearEvent lcevt = Events.LevelClearEvent;
                lcevt.DamageDealt+=pcc.DamageDealt;

                Debug.Log($"Inflicting {CalcDamage} ({rv} < {(psb.Crit_Hit/100f)}) {(shouldCrit ? "[CRIT]":"")} Damage");
                break;
              case "Enemy":
                EnemyStats esb = Owner.GetComponent<EnemyController>().enemyStats;
                shouldCrit= rv < (esb.Crit_Hit/100f);
                // CalcDamage = Mathf.CeilToInt((Damage + esb.Attack) * (shouldCrit ? (esb.Crit_Damage/100)+1 : 1));
                CalcDamage = Mathf.Ceil((Damage + esb.Attack) * (shouldCrit ? (esb.Crit_Damage/100f)+1f : 1f));
                Debug.Log($"Inflicting {CalcDamage} ({rv} < {(esb.Crit_Hit/100f)}) {(shouldCrit ? "[CRIT]":"")} Damage to {collider.gameObject.name}");
                break;
              default:
                break;
            }
            /* References the player's stats to calculate the projectile behaviour */
            // PlayerStats psb = Owner.GetComponent<PlayerStatsBase>().playerStats;

            // System.Random rand = new System.Random();
            // double rv = rand.NextDouble(/*0f,100f*/);
            // bool shouldCrit= rv < (psb.Crit_Hit/100f);
            // int CalcDamage = Mathf.CeilToInt((Damage + psb.Attack) * (shouldCrit ? (psb.Crit_Damage/100)+1 : 1));
            // Debug.Log($"Inflicting {CalcDamage} ({rv} < {(psb.Crit_Hit/100f)}) {(shouldCrit ? "[CRIT]":"")} Damage");
            // damage
            if (AreaOfDamage)
            {
                // area damage
                AreaOfDamage.InflictDamageInArea(CalcDamage, shouldCrit, point, HittableLayers, k_TriggerInteraction,
                    m_ProjectileBase.Owner);
            }
            else
            {
                // point damage
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.InflictDamage(CalcDamage, shouldCrit, false, m_ProjectileBase.Owner);
                }
            }

            // impact vfx
            if (ImpactVfx)
            {
                GameObject impactVfxInstance = Instantiate(ImpactVfx, point + (normal * ImpactVfxSpawnOffset),
                    Quaternion.LookRotation(normal));
                if (ImpactVfxLifetime > 0)
                {
                    Destroy(impactVfxInstance.gameObject, ImpactVfxLifetime);
                }
            }

            // impact sfx
            if (ImpactSfxClip)
            {
                AudioUtility.CreateSFX(ImpactSfxClip, point, AudioUtility.AudioGroups.Impact, 1f, 3f);
            }

            // Self Destruct
            if(DestroyOnCollision && Owner.tag != collider.gameObject.tag) { Destroy(this.gameObject); }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = RadiusColor;
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
// }