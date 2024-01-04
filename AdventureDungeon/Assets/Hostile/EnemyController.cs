using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EnemyStats 
{
  public float Health;
  public float Health_Regen;// { get; set; }
  public int Defence;//  { get; set; }
  // int? _attack;
  public int Attack;// { get {return _attack ?? 5;} set { _attack = value;} }
  // public int Evade;//  { get; set; }
  public int Shield;//  { get; set; }
  public float Energy;//  { get; set; }
  public float Energy_Regen;//  { get; set; }
  public float Crit_Hit;//  { get; set; }
  public float Crit_Damage;//  { get; set; }
  public float Attack_Speed;
  public float Movement_Speed;//  { get; set; }

  public int All_Resist;//  { get; set; }
  public int Fire_Resist;//  { get; set; }
  public int Water_Resist;//  { get; set; }

  // public /*override*/ EnemyStats(bool new)
  public static EnemyStats New()
  {
    EnemyStats es = new EnemyStats();
    es.Health=100;
    es.Health_Regen=0;
    es.Defence=0;
    es.Attack=5;
    es.Shield=0;
    es.Energy=10;
    es.Energy_Regen=1;
    es.Crit_Hit=5;
    es.Crit_Damage=50; // Add 50% to the base damage
    es.Attack_Speed=0;
    es.Movement_Speed=4;

    es.All_Resist=0;
    es.Fire_Resist=0;
    es.Water_Resist=0;

    return es;
  }

  // For syncing for the `enemyMovement`
  public static EnemyStats GetStats()
  {
    return new EnemyStats();
  }
}

public class EnemyDropLoot 
{
  [Header("Loot")] [Tooltip("The object this enemy can drop when dying")]
  public GameObject LootPrefab;

  [Tooltip("The chance the object has to drop")] [Range(0, 1)]
  public float DropRate = 1f;
}

[RequireComponent(typeof(Health), typeof(EnemyMovement))]
public class EnemyController : MonoBehaviour
{
    public string enemyName;
    public EnemyStats enemyStats;// = EnemyStats.New();
    
    [Header("Enemy Parameters")] 
    [Tooltip("The amount of experience dropped on enemy death")]
    public int ExperienceGainOnDeath = 10;

    public List<EnemyDropLoot> dropLoot = new List<EnemyDropLoot>();

    public enum ENEMYRARITY { Common, Uncommon, Rare, Epic, Legendary, Mythic }
    [Tooltip("The enemy rarity")]
    public ENEMYRARITY Rarity = ENEMYRARITY.Common;

    [Tooltip("Is the enemy a boss")]
    public bool IsBoss = false;
    [Tooltip("Is the enemy a sub or mini boss")]
    public bool IsElite = false; //TODO: Have random custom names for `Elites`

    [Header("Sounds")] [Tooltip("Sound played when recieving damages")]
    public AudioClip DamageTick;

    [Header("VFX")] [Tooltip("The VFX prefab spawned when the enemy dies")]
    public GameObject DeathVfx;
    [Tooltip("The point at which the death VFX is spawned")]
    public Transform DeathVfxSpawnPoint;
    [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
    public float DeathDuration = 0f;

    [Tooltip("The enemy's healthbar (If enemy is a boss, select all player-used canvas')")]
    public GameObject healthBar;

    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public UnityAction onDamaged;

    float m_LastTimeDamaged = float.NegativeInfinity;
    EnemyManager m_EnemyManager;
    Health m_Health;
    bool m_WasDamagedThisFrame;
    // Invulnerability time
    readonly float invulnerableTime = 0.3f;

    /*The list of all players who damaged this enemy */
    List<GameObject> contributingToDamage = new List<GameObject>();

    Pathfinding.AIDestinationSetter destSet;

    #region Enemy Attack / Projectile

    public GameObject Owner { get; set; }
    public UnityAction OnShoot;
    public event Action OnShootProcessed;
    float m_LastTimeShot = Mathf.NegativeInfinity;
    EnemySpriteController _esc;
    /* The variables for how the weapon will be affected based on the player's stats */
    float Attack_Bonus;
    float Attack_Speed;
    float Crit_Hit;
    float Crit_Damage;

    [Tooltip("The projectile prefab")] 
    public ProjectileBase ProjectilePrefab;
    [Tooltip("The attacks per second")]
    public float AttackPerSecond = 1.5f;
    [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
    public float BulletSpreadAngle = 0f;
    [Tooltip("Amount of bullets per shot")]
    public int BulletsPerShot = 1;

    #endregion //Enemy Attack / Projectile

  
    // Start is called before the first frame update
    void Start()
    {
      Owner = this.gameObject;
      m_EnemyManager = FindObjectOfType<EnemyManager>();
      DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyController>(m_EnemyManager, this);

      // m_ActorsManager = FindObjectOfType<ActorsManager>();
      // DebugUtility.HandleErrorIfNullFindObject<ActorsManager, EnemyController>(m_ActorsManager, this);
      if(IsBoss) { m_EnemyManager.RegisterBoss(this); }
      else if(IsElite) { }
      else { m_EnemyManager.RegisterEnemy(this); }

      m_Health = GetComponent<Health>();
      DebugUtility.HandleErrorIfNullGetComponent<Health, EnemyController>(m_Health, this, gameObject);

      m_Health.MaxHealth = enemyStats.Health;
      healthBar = healthBar!=null ? healthBar : this.gameObject.transform.Find("Healthbar").gameObject;

      // Subscribe to damage & death actions
      m_Health.OnDie += OnDie;
      m_Health.OnDamaged += OnDamaged;

      // Sync enemy stats for attacking
      Attack_Bonus = enemyStats.Attack;
      Attack_Speed = enemyStats.Attack_Speed;
      Crit_Hit = enemyStats.Crit_Hit;
      Crit_Damage = enemyStats.Crit_Damage;
      _esc = gameObject.GetComponent<EnemySpriteController>();

      destSet = GetComponent<Pathfinding.AIDestinationSetter>();
      DebugUtility.HandleErrorIfNullGetComponent<Pathfinding.AIDestinationSetter, EnemyController>(destSet, this, gameObject);
      

      // Sets the time to current time so that the player doesn't get one-shot immediatly after entering a room
      m_LastTimeShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
      /* For the sprite to flash on hit */
      // Color currentColor = OnHitBodyGradient.Evaluate((Time.time - m_LastTimeDamaged) / FlashOnHitDuration /* or `invulnerabilityThresh` */ );
      // m_BodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
      // foreach (var data in m_BodyRenderers)
      // {
      //     data.Renderer.SetPropertyBlock(m_BodyFlashMaterialPropertyBlock, data.MaterialIndex);
      // }

      if(_esc.lookingAtPlayer)
      {
        // if(m_LastTimeShot + ((( AttackPerSecond + (Attack_Speed/50f)) * /* alpha */(3f/10f))) < Time.time) {
          TryShoot();
        // }
      }

      if(destSet.target != _esc.nearestPlayer)
      {
        destSet.target = _esc.nearestPlayer;
      }
      
      m_WasDamagedThisFrame = false;
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // Debug.Log($"{this.gameObject.transform.name} was damaged by {damageSource.name}...");
        Debug.Log($"{this.gameObject.transform.name} was damaged by {damageSource.name}...");

        // Debug.Log($"Hit @ {m_LastTimeDamaged} ->\t {(Time.time - m_LastTimeDamaged) / invulnerabilityThresh}");
        if(((Time.time - m_LastTimeDamaged) / invulnerableTime) < 1 && false) {
          Debug.Log($"Attempted to Hit @ {m_LastTimeDamaged} ->\t {(Time.time - m_LastTimeDamaged) / invulnerableTime} remains...");
          return;
        }

        // test if the damage source is the player
        if (damageSource && !damageSource.GetComponent<EnemyController>())
        {
            
            onDamaged?.Invoke();
            m_LastTimeDamaged = Time.time;

            // update health bar value
            /* If the bar goes over the area, use `Mathf.Clamp(m_Health.GetRatio(), 0, 1);` */
            healthBar.transform.GetChild(0).Find("Value").GetComponent<RectTransform>().localScale = new Vector3(m_Health.GetRatio(),1f,1f);
        
            // play the damage tick sound
            // if (DamageTick && !m_WasDamagedThisFrame)
            // {
            //    AudioUtility.CreateSFX(DamageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f);
            // }
        
            m_WasDamagedThisFrame = true;

            // Temporary for single player, will have list for all players that damaged enemy
            //contributingToDamage[contributingToDamage.FindIndex( p => p.name == contributingToDamage)] // UUID
            contributingToDamage.Add(damageSource);
        }
    }

    void OnDie()
    {
        if(DeathVfx != null)
        {
          // spawn a particle system when dying
          var vfx = Instantiate(DeathVfx, DeathVfxSpawnPoint.position, Quaternion.identity);
          Destroy(vfx, 5f);
        }

        // tells the game flow manager to handle the enemy destuction
        if(IsBoss) { m_EnemyManager.UnregisterBoss(this); }
        else if(IsElite) { }
        else { m_EnemyManager.UnregisterEnemy(this); }

        // drop experience to player ([TODO:] or all players who damaged (use list of users who damaged along with for how much))
        double droppedXP = ExperienceGainOnDeath;
        Debug.Log($"Dropping {droppedXP} experience...");

        foreach (GameObject cu in contributingToDamage)
        {
          /** On death, send the data to a server (gameobject) and then have that distribute the exp to players */
          Debug.Log(cu.name);
          // Debug.Log(cu.GetComponents(typeof(Component)).Length);
          // foreach (var tmpitm in cu.GetComponents(typeof(Component)))
          // {
          //   if(tmpitm.ToString().IndexOf("PlayerStatsBase") != -1) {
          //     var tempGetType = GameObject.Find("GameManager"); // Rename the `GameManager` to the  UserId then attempt to find by that user id name

          //     Debug.Log($"tmpitm = {tmpitm}");
          //     ServerClientBridge scb = cu.AddComponent(typeof(ServerClientBridge)) as ServerClientBridge;
          //     scb.DoExperienceGain(cu.name, droppedXP);
          //     Debug.Log($"SCBridge > {scb}");
          //     break;
          //   }
          // }
        }

        // loot an object
        foreach (EnemyDropLoot l in dropLoot)
        {
          if (TryDropItem(l))
          {
              Instantiate(l.LootPrefab, transform.position, Quaternion.identity);
          }
        }

        // this will call the OnDestroy function
        Destroy(gameObject, DeathDuration);
    }

    void OnDestroy()
    {
        // tells the game flow manager to handle the enemy destuction
        if(IsBoss) { m_EnemyManager.UnregisterBoss(this); }
        else if(IsElite) { }
        else { m_EnemyManager.UnregisterEnemy(this); }
        Debug.Log($"[EnemyController.OnDestroy()] Unregistering enemy `{gameObject.name}`");
    }

    public bool TryDropItem(EnemyDropLoot l)
    {
      
        if (l.DropRate == 0 || l.LootPrefab == null)
            return false;
        else if (l.DropRate == 1)
            return true;
        else
            return (UnityEngine.Random.value <= l.DropRate);
    }

    bool TryShoot()
    {
        float _calculatedAPS = ( 1/ (AttackPerSecond + (Attack_Speed/50f)) ) ;
        /* (( 1.4 + (0/50)) * (3/10)) */
        // float _calculatedAPS = AttackPerSecond>=1 ? (( AttackPerSecond + (Attack_Speed/50f)) * (3f/10f)) : (( AttackPerSecond + (Attack_Speed/50f)) / (3f/10f));
        // if(!WeaponUsesAmmo && m_LastTimeShot + DelayBetweenShots < Time.time) {
        if(m_LastTimeShot + (
          _calculatedAPS
        ) < Time.time) {
          // Debug.Log("\n\tShooting...");
          HandleShoot();
          return true;
        }
        // if (m_CurrentAmmo >= 1f
        //     && m_LastTimeShot + AttackPerSecond < Time.time)
        // {
        //     HandleShoot();
        //     m_CurrentAmmo -= 1f;

        //     return true;
        // }

        return false;
    }

    void HandleShoot()
    {
        int bulletsPerShotFinal = BulletsPerShot;
        // = ShootType == WeaponShootType.Charge
        //     ? Mathf.CeilToInt(/*CurrentCharge * */BulletsPerShot)
        //     : BulletsPerShot;

        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShotFinal; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(transform); //new Vector3 (0, 0, _esc.angle) 
            float angle = Vector2.SignedAngle(Vector2.up, transform.eulerAngles);

            ProjectileBase newProjectile = Instantiate(ProjectilePrefab, transform.position,
                // Quaternion.Euler(new Vector3 (0, 0, angle)));
                // newProjectile.transform.eulerAngles = new Vector3 (0, 0, angle);
                Quaternion.LookRotation(shotDirection)); // <-------- This is the culprit of the issue (maybe), check `ProjectileStandard` as well
                // Quaternion.LookRotation(new Vector3 (0, 0, transform.eulerAngles.z))); // <-------- This is the culprit of the issue (maybe), check `ProjectileStandard` as well
        
            
            newProjectile.EnemyShoot(this);
            
        }


        m_LastTimeShot = Time.time;


        OnShoot?.Invoke();
        OnShootProcessed?.Invoke();
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = BulletSpreadAngle / 180f;
        // Debug.Log($"ESCAngle : {_esc.angle}");
        Vector3 spreadWorldDirection = Vector3.Lerp(shootTransform.forward, new Vector3 (0, 0, 90),
            spreadAngleRatio);
        // Vector3 spreadWorldDirection = Vector3.Lerp(shootTransform.up, UnityEngine.Random.insideUnitSphere,
        //     spreadAngleRatio);
        // Debug.Log($"GetShotDir > {spreadWorldDirection}");
        return spreadWorldDirection;
    }
}
