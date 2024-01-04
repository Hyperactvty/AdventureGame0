// using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

// namespace Unity.FPS.Gameplay
// {
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputHandler), typeof(AudioSource))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [Header("References")] [Tooltip("Reference to the Game Manager used for the player")]
        public GameObject UserGameManager;

        [Tooltip("Reference to the main camera used for the player")]
        public Camera PlayerCamera;

        // [Tooltip("Audio source for footsteps, jump, etc...")]
        // public AudioSource AudioSource;

        [Header("Movement")] [Tooltip("Max movement speed when grounded (when not sprinting)")]
        public float MaxSpeedOnGround = 10f;

        public float moveSpeed;
        public Rigidbody2D rb;

        private Vector2 moveDirection;

        [Tooltip(
            "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
        public float MovementSharpnessOnGround = 15;

        [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
        public float SprintSpeedModifier = 2f;

        [Header("Rotation")] [Tooltip("Rotation speed for moving the camera")]
        public float RotationSpeed = 200f;

        // [Header("Audio")] [Tooltip("Amount of footstep sounds played when moving one meter")]
        // public float FootstepSfxFrequency = 1f;

        // [Tooltip("Amount of footstep sounds played when moving one meter while sprinting")]
        // public float FootstepSfxFrequencyWhileSprinting = 1f;

        // [Tooltip("Sound played for footsteps")]
        // public AudioClip FootstepSfx;


        public Vector2 CharacterVelocity { get; set; }
        public bool IsDead { get; private set; }
        public float DamageDealt=0;// { get; private set; }

        Health m_Health;
        PlayerInputHandler m_InputHandler;
        PlayerWeaponsManager m_WeaponsManager;

        float m_LastTimeDamaged = float.NegativeInfinity;

        // Invulnerability time
        readonly float invulnerableTime = 0.3f;

        PlayerStatsBase psb; [SerializeField]float prevMoveValue;
        // Actor m_Actor;

        // For registering the player so enemies can find the player
        EnemyManager m_EnemyManager;
        WeaponBase m_WeaponBase;

        void Awake()
        {
            // ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            // if (actorsManager != null)
            //     actorsManager.SetPlayer(gameObject);
        }

        void Start()
        {
            // For registering the player so enemies can find the player
            m_EnemyManager = FindObjectOfType<EnemyManager>();
            m_EnemyManager.RegisterPlayer(this);

            m_WeaponBase = gameObject.GetComponentInChildren<WeaponBase>();

            psb = GetComponent<PlayerStatsBase>();
            prevMoveValue = psb.playerStats.Movement_Speed/20;

            // fetch components on the same gameObject

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, PlayerCharacterController>(m_InputHandler,
                this, gameObject);

            m_WeaponsManager = GetComponent<PlayerWeaponsManager>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, PlayerCharacterController>(
                m_WeaponsManager, this, gameObject);

            m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerCharacterController>(m_Health, this, gameObject);

            // m_Actor = GetComponent<Actor>();
            // DebugUtility.HandleErrorIfNullGetComponent<Actor, PlayerCharacterController>(m_Actor, this, gameObject);

            m_Health.OnDie += OnDie;

        }

        void Update()
        {
            
            HandleCharacterMovement();
            ProcessInputs();

            // Looking for `GameManager` with UUID
            // if(UserGameManager==null) 
            // {
            //   string ugmID="";
            //   foreach (var tmpitm in gameObject.GetComponents(typeof(Component)))
            //   {
            //     if(tmpitm.ToString().IndexOf("PlayerStatsBase") != -1) {
            //       ugmID = tmpitm.name;
            //       Debug.Log($"FOUND UGMID? {tmpitm}");
            //       break;
            //     }
            //   }
            //   // Debug.Log($"Found ? {gameObject.GetComponentInParent<PlayerStatsBase>()}");
            //   var locateUGC = GameObject.FindGameObjectsWithTag("UserGameController");
            //   foreach (var item in locateUGC)
            //   {
            //     if( item.name == $"GameManager_{ugmID}" ) {
            //       Debug.Log($"We have found the UGM > {item}");
            //       UserGameManager = item;
            //     }
            //   }
            // }
        }

        void OnDamaged(float damage, GameObject damageSource)
        {
            // Debug.Log($"{this.gameObject.transform.name} was damaged by {damageSource.name}...");
            Debug.Log($"Player `{this.gameObject.transform.name}` was damaged by {damageSource.name}...");

            // Debug.Log($"Hit @ {m_LastTimeDamaged} ->\t {(Time.time - m_LastTimeDamaged) / invulnerabilityThresh}");
            if(((Time.time - m_LastTimeDamaged) / invulnerableTime) < 1) {
              Debug.Log($"Attempted to Hit @ {m_LastTimeDamaged} ->\t {(Time.time - m_LastTimeDamaged) / invulnerableTime} remains...");
              return;
            }

            // test if the damage source is an enemy
            if (damageSource && damageSource.GetComponent<EnemyController>())
            {
                
                // onDamaged?.Invoke();
                m_LastTimeDamaged = Time.time;

                // update health bar value
                /* If the bar goes over the area, use `Mathf.Clamp(m_Health.GetRatio(), 0, 1);` */
                // healthBar.transform.GetChild(0).Find("Value").GetComponent<RectTransform>().localScale = new Vector3(m_Health.GetRatio(),1f,1f);
            
                // play the damage tick sound
                // if (DamageTick && !m_WasDamagedThisFrame)
                // {
                //    AudioUtility.CreateSFX(DamageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f);
                // }
            
                // m_WasDamagedThisFrame = true;

                // Temporary for single player, will have list for all players that damaged enemy
                //contributingToDamage[contributingToDamage.FindIndex( p => p.name == contributingToDamage)] // UUID
                // contributingToDamage.Add(damageSource);
            }
        }

        void OnDie()
        {
            IsDead = true;

            // Tell the weapons manager to switch to a non-existing weapon in order to lower the weapon
            m_WeaponsManager.SwitchToWeaponIndex(-1, true);

            // tells the game flow manager to handle the player (enemy) destuction
            m_EnemyManager.UnregisterPlayer(this);

            EventManager.Broadcast(Events.PlayerDeathEvent);
        }


        void HandleCharacterMovement()
        {
          if(prevMoveValue!=psb.playerStats.Movement_Speed)
          {
            prevMoveValue = psb.playerStats.Movement_Speed/20;
          }
            return;
            // character movement handling
            bool isSprinting = m_InputHandler.GetSprintInputDown();
            {
                Debug.Log("Is now sprinting");
                float speedModifier = isSprinting ? SprintSpeedModifier : 1f;

                // converts move input to a worldspace vector based on our character's transform orientation
                Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());


                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround * speedModifier;
                // reduce speed if slowed by slowed speed ratio
                // if (IsSlowed)
                //     targetVelocity *= MaxSpeedSlowedRatio;
                // targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                //                   targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                    MovementSharpnessOnGround * Time.deltaTime);

                
            }

            // // apply the final calculated velocity value as a character movement
            // Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            // Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_Controller.height);
            // m_Controller.Move(CharacterVelocity * Time.deltaTime);

            // // detect obstructions to adjust velocity accordingly
            // m_LatestImpactSpeed = Vector3.zero;
            // if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_Controller.radius,
            //     CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
            //     QueryTriggerInteraction.Ignore))
            // {
            //     // We remember the last impact speed because the fall damage logic might need it
            //     m_LatestImpactSpeed = CharacterVelocity;

            //     CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
            // }
        }

        void FixedUpdate()
        {
          // Do Physics
          Move();
        }

        void ProcessInputs()
        {
          float moveX = Input.GetAxisRaw("Horizontal");
          float moveY = Input.GetAxisRaw("Vertical");

          moveDirection = new Vector2(moveX, moveY).normalized;

          // float whereYaGoing = moveDirection.x || m_WeaponBase.direction.normalized.x

          switch (m_WeaponBase.direction.normalized.x)
          {
            case >=0f:
              if( transform.localScale.x != -1f) {break;}
              transform.localScale = new Vector3(1, 1, 1);
              break;
            case <0f:
              if( transform.localScale.x != 1f) {break;}
              // transform.localScale.x = -1;
              transform.localScale = new Vector3(-1, 1, 1);
              break;
            default:
              break;
          }
        }

        void Move()
        {
          rb.velocity = new Vector2(moveDirection.x * (moveSpeed + prevMoveValue), moveDirection.y * (moveSpeed + prevMoveValue));
        }

        // void OnRoomClear(RoomClearEvent evt) => UpdateRoomStats();

        // void UpdateRoomStats()
        // {
        //   RoomClearEvent rcevt = Events.RoomClearEvent;
        //   Debug.Log($"[PCC.UpdateRoomStats] Event Dmg Dealt -> {rcevt.DamageDealt} | Damage Dealt -> {DamageDealt}");
        //   rcevt.DamageDealt+=DamageDealt;
        // }

    }
// }