using UnityEngine;
using UnityEngine.SceneManagement;

// namespace Unity.FPS.Game
// {
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")] [Tooltip("Duration of the fade-to-black at the end of the game")]
        public float EndSceneLoadDelay = 3f;

        [Tooltip("The canvas group of the fade-to-black screen")]
        public CanvasGroup LoadingScreenFadeCanvasGroup;

        [Header("Win")] [Tooltip("This string has to be the name of the scene you want to load when winning")]
        public string WinSceneName = "HubScene";

        [Tooltip("Duration of delay before the fade-to-black, if winning")]
        public float DelayBeforeFadeToBlack = 4f;

        [Tooltip("Win game message")]
        public string WinGameMessage;
        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Sound played on win")] public AudioClip VictorySound;

        [Header("Lose")] [Tooltip("This string has to be the name of the scene you want to load when losing")]
        public string LoseSceneName = "HubScene";


        public bool GameIsEnding { get; private set; }

        float m_TimeLoadEndGameScene;
        string m_SceneToLoad;

        void Awake()
        {
            EventManager.AddListener<LoadScreenEvent>(OnStartLoadScreen);
            // EventManager.AddListener<RoomClearEvent>(OnAllObjectivesCompleted);
            EventManager.AddListener<UserCompleteOrExitLevelEvent>(OnReturnButtonHit);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);

        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);
        }

        void Update()
        {
            if (GameIsEnding)
            {
                float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
                LoadingScreenFadeCanvasGroup.alpha = timeRatio;

                AudioUtility.SetMasterVolume(1 - timeRatio);

                // See if it's time to load the end scene (after the delay)
                if (Time.time >= m_TimeLoadEndGameScene) // DO THIS WHEN BUTTON PUSH
                {
                  Debug.Log("Load Scene Now");
                    SceneManager.LoadScene(m_SceneToLoad);
                    GameIsEnding = false;


                    FinishedLoading(); 
                }
            }
        }

        void FinishedLoading()
        {
          Debug.Log("Time to do Un-Loading Screen Now");
          m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;

          float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
          LoadingScreenFadeCanvasGroup.alpha = -timeRatio;

          // See if it's time to closethe loading screen (after the delay)
          // if (Time.time >= m_TimeLoadEndGameScene)
          // {
            Debug.Log("Unload Loading Screen Now");
            LoadingScreenFadeCanvasGroup.gameObject.SetActive(false);
          // }
        }

        void OnStartLoadScreen(LoadScreenEvent evt) => FinishedLoading();
        // void OnAllObjectivesCompleted(RoomClearEvent evt) => EndGame(true, evt.DamageDealt);
        void OnReturnButtonHit(UserCompleteOrExitLevelEvent evt) => EndGame(true); // LevelClearEvent
        void OnPlayerDeath(PlayerDeathEvent evt) => EndGame(false);

        void EndGame(bool win, float _damageDealt=-1f)
        {
            
            // unlocks the cursor before leaving the scene, to be able to click buttons
            // Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Remember that we need to load the appropriate end scene after a delay
            GameIsEnding = true;
            LoadingScreenFadeCanvasGroup.gameObject.SetActive(true);
            if (win)
            {
                Debug.Log($"Beat level in (time) dealing {_damageDealt} DMG.");
                m_SceneToLoad = WinSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;

                // play a sound on win
                if(false)
                {
                  var audioSource = gameObject.AddComponent<AudioSource>();
                  audioSource.clip = VictorySound;
                  audioSource.playOnAwake = false;
                  audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
                  audioSource.PlayScheduled(AudioSettings.dspTime + DelayBeforeWinMessage);
                }

                // create a game message
                //var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
                //if (message)
                //{
                //    message.delayBeforeShowing = delayBeforeWinMessage;
                //    message.GetComponent<Transform>().SetAsLastSibling();
                //}

                // DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
                // displayMessage.Message = WinGameMessage;
                // displayMessage.DelayBeforeDisplay = DelayBeforeWinMessage;
                // EventManager.Broadcast(displayMessage);
            }
            else
            {
                m_SceneToLoad = LoseSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay;
            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<LoadScreenEvent>(OnStartLoadScreen);
            // EventManager.RemoveListener<RoomClearEvent>(OnAllObjectivesCompleted);
            EventManager.RemoveListener<UserCompleteOrExitLevelEvent>(OnReturnButtonHit);
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
// }