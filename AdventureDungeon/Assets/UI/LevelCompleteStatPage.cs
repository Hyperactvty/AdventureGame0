using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelCompleteStatPage : MonoBehaviour
{
    public CanvasGroup LevelCompletePageCanvasGroup;
    public Button confirmButton;

    public float EndSceneLoadDelay = 2.5f;
    public float DelayBeforeFadeToBlack = 4f;

    float m_TimeLoadEndGameScene;
    bool shouldCanvasLoad = false;
    bool canvasLoaded = false;
    bool confirmHit = false;

    // Start is called before the first frame update
    void Start()
    {
      EventManager.AddListener<LevelClearEvent>(OnLevelCleared);

      confirmButton.onClick.AddListener(CloseStatPage);
    }

    void OnLevelCleared(LevelClearEvent evt) => DisplayStatPage(evt.DamageDealt);

    void DisplayStatPage(float _damageDealt)
    {
        
        // unlocks the cursor before leaving the scene, to be able to click buttons
        // Cursor.lockState = CursorLockMode.None;
        shouldCanvasLoad = true;
        Cursor.visible = true;
        LevelCompletePageCanvasGroup.gameObject.SetActive(true);


            m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay /*+ DelayBeforeFadeToBlack*/;

 
    }

    

    void CloseStatPage()
    {
        
        // unlocks the cursor before leaving the scene, to be able to click buttons
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

        m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + 1.5f/* + DelayBeforeFadeToBlack*/;
        confirmHit = true;
        shouldCanvasLoad=false;

        UserCompleteOrExitLevelEvent userReturn = Events.UserCompleteOrExitLevelEvent;
        var _pcc = FindObjectsByType<PlayerStatsBase>(FindObjectsSortMode.None).Select(_u => _u);
        var grabbedPlayer = _pcc.Where(_u => _u.UserId == GetComponentInParent<UserBase>().CurrentUser.UserId ).FirstOrDefault();

        userReturn.PCC = grabbedPlayer.GetComponent<PlayerCharacterController>();
        EventManager.Broadcast(userReturn);


        // LevelCompletePageCanvasGroup.gameObject.SetActive(false);

    }

    // TODO: USE IENUMERABLE COROUTINES FOR HANDLING THE OPEN/CLOSE OF PAGE!


    // Update is called once per frame
    void Update()
    {
        if(!canvasLoaded)
        {
          float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
          LevelCompletePageCanvasGroup.alpha = timeRatio;
        }

        if(confirmHit && canvasLoaded)
        {
          // m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;
          float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
          LevelCompletePageCanvasGroup.alpha = -timeRatio;
          //when fade out, set both bools to `false`
          if(Time.time >= m_TimeLoadEndGameScene)
          {
            canvasLoaded = false;
            confirmHit = false;
            LevelCompletePageCanvasGroup.gameObject.SetActive(false);
          }
        }


        if (Time.time >= m_TimeLoadEndGameScene && !canvasLoaded && shouldCanvasLoad)
        {
          canvasLoaded = true;
        }
    }
}
