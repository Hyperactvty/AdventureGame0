using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

using System.Text;
using UnityEngine.Networking;

public class RoomBase : MonoBehaviour
{
  public GameObject dTrigLeft;
  public GameObject dTrigRight;
  public GameObject dTrigUp;
  public GameObject dTrigDown;
  public EdgeCollider2D roomFloor;

  public List<GameObject> enemySpawn = new List<GameObject>();
  public int enemySpawnCount;

  public bool roomCleared = false;

#region DB

    /* FOR THE DB COMMUNICATIONS */
    private ToDB.UserData _ud;

    IEnumerator Download(string id, System.Action<ToDB.UserData> callback = null)
    {
      //https://us-east-2.aws.data.mongodb-api.com/app/data-ggtgs/endpoint/data/v1
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/users/" + id))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(ToDB.UserData.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    IEnumerator Upload(string profile, System.Action<bool> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://localhost:3000/users", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if(callback != null) 
                {
                    callback.Invoke(false);
                }
            }
            else
            {
                if(callback != null) 
                {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
        }
    }

#endregion //DB

    public enum ROOMTYPE
    {
        Standard,
        Start,
        Elite,
        Boss,

        Treasure,
        TravellingMerchant, // This guy sells items that cannot be found anywhere else
    }

    public ROOMTYPE roomType = ROOMTYPE.Standard;

    public NotificationToast displayMessagePrefab;
    public Transform HUD;
    public EnemySpawn enemySpawnObject;

    bool isDoingRoomClear = false;
    private System.Random rand = new System.Random();


    // RoomClearEvent rcevt = Events.RoomClearEvent;

    // IEnumerator SpawnCoroutine(Vector2 _pos) {
    //     float spawnRandomTime = Random.Range(0f,1.25f);
    //     //Declare a yield instruction.
    //     WaitForSeconds wait = new WaitForSeconds(spawnRandomTime);
    //     yield return wait; 
    //     var enemyToSpawn = enemySpawn[rand.Next(enemySpawn.Count)];

    //     GameObject nes = Instantiate(enemySpawnObject.gameObject, new Vector2(0,0),Quaternion.identity, transform);
    //     nes.transform.localPosition = _pos;
    //     nes.transform.localScale = enemyToSpawn.transform.localScale;

    //     wait = new WaitForSeconds(1.5f);
    //     yield return wait; 
        
    //     GameObject newEnemySpawn = Instantiate(enemyToSpawn, new Vector2(0,0),Quaternion.identity, transform);
    //     newEnemySpawn.transform.localPosition = _pos;
    //     Destroy(newEnemySpawn);
    // }



    public void OnRoomEnter()
    {
      if(roomCleared==false)
      {
        RoomClearEvent rcevt = Events.RoomClearEvent;
        rcevt.EnableNextRooms = false;
        // EventManager.Broadcast(rcevt);
        EventManager.AddListener<RoomClearEvent>(OnRoomClear);
        isDoingRoomClear = false;

        Transform roomBaseplate = gameObject.transform.Find("Base");
        Bounds spawnableArea = roomBaseplate.GetComponent<EdgeCollider2D>().bounds;//roomFloor.bounds;

        // Spawn the enemies
        for (int i = 0; i < enemySpawnCount; i++)
        {
          Vector2 _spawnPos = new Vector2(
            Random.Range(spawnableArea.min.x+1, spawnableArea.max.x-1),
            Random.Range(spawnableArea.min.y+1, spawnableArea.max.y-1)
          );
          // Debug.Log($"RANDOM POSITION : {roomBaseplate.InverseTransformPoint(_spawnPos)}");
          
          // var enemyToSpawn = enemySpawn[rand.Next(enemySpawn.Count)];
          // StartCoroutine(SpawnCoroutine(_spawnPos));
          GameObject nes = Instantiate(enemySpawnObject.gameObject, new Vector2(10,-5),Quaternion.identity, transform);
          nes.transform.localPosition = _spawnPos;
          // nes.transform.localScale = enemyToSpawn.transform.localScale;
          // enemySpawnObject.enemy = enemyToSpawn;
          enemySpawnObject._pos= _spawnPos;
          
          /* THIS WORKS */ /* if it doesn't work, though, just use `BoxCollider2d` instead of the `EdgeCollider2D`*/
          // GameObject newEnemySpawn = Instantiate(enemySpawn[rand.Next(enemySpawn.Count)], new Vector2(0,0),Quaternion.identity, transform);
          // newEnemySpawn.transform.localPosition = _spawnPos;
        }
      }

      EnableDoorTransport(roomCleared);
    }

    void OnRoomClear(RoomClearEvent evt) => DoRoomClearRoutine(evt.EnableNextRooms);
    void DoRoomClearRoutine(bool _isClear)
    {
      if(!_isClear || isDoingRoomClear) { return; }
      isDoingRoomClear=true;
      EventManager.RemoveListener<RoomClearEvent>(OnRoomClear);
      // RoomClearEvent rcevt = Events.RoomClearEvent;
      // rcevt.EnableNextRooms = true;
      // EventManager.Broadcast(rcevt);

      Debug.Log("The room has been cleared");
      
      var message = Instantiate(displayMessagePrefab, new Vector2(0, 125), Quaternion.identity, HUD);
      // message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Room Clear";
      message.transform.localPosition = new Vector2(0, 125);
      // message.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Room Clear";
      message.Initialize("Room Clear");

      GameObject[] tempUserList = GameObject.FindGameObjectsWithTag("Player");
      foreach (var tu in tempUserList)
      {
        // TEMP -> Testing to see how DB works
        _ud = new ToDB.UserData();
        _ud.UserId = "ff9c4df2-863c-46d1-b6e2-1349c0192b3d";
        _ud.Username = "Testing";
        StartCoroutine(Upload(_ud.Stringify(), result => {
            Debug.Log(result);
        }));
        // List<string> tempFileContents = new List<string>();
        // UserBase tUB = GameObject.Find("GameManager").GetComponent<UserBase>();
        // // foreach (var cu in tUB.CurrentUser)
        // // {
        // //   tempFileContents.Add(cu);
        // // }
        // tempFileContents.Add($"UserId,{tUB.CurrentUser.UserId}");
        // tempFileContents.Add($"Username,{tUB.CurrentUser.Username}");
        // System.IO.File.WriteAllText(GameEvent.tempDataFile, tempFileContents.ToString());
      }

      roomCleared = true;
      EnableDoorTransport(roomCleared);
      
    }


    // Start is called before the first frame update
    void Start()
    {
        AssignDoorTriggers();
        roomFloor = gameObject.GetComponentInChildren<EdgeCollider2D>();
        // OnRoomEnter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignDoorTriggers()
    {
      Debug.Log($"<color=f00>{gameObject.GetComponentsInChildren<DoorTrigger>(true).Length}</color>");
      foreach (var _dt in gameObject.GetComponentsInChildren<DoorTrigger>(true))
      {
        switch(_dt.doorDirection)
        {
          case DoorTrigger.ROOMDIRECTION.Left:
            dTrigLeft = _dt.gameObject;
            break;
          case DoorTrigger.ROOMDIRECTION.Right:
            dTrigRight = _dt.gameObject;
            break;
          case DoorTrigger.ROOMDIRECTION.Up:
            dTrigUp = _dt.gameObject;
            break;
          case DoorTrigger.ROOMDIRECTION.Down:
            dTrigDown = _dt.gameObject;
            break;
          default: break;
        }
      }
      
    }

    void EnableDoorTransport(bool b)
    {
      GameObject[] _doors = {dTrigLeft,dTrigRight,dTrigUp,dTrigDown};
      foreach (var _d in _doors)
      {
        if(_d!=null)
        {
          var _doorComp = _d.GetComponent<DoorTrigger>();
          _doorComp.ActivateDoor(b);
        }
      }
      
    }

    public void DoTransport(DoorTrigger.ROOMDIRECTION dir, Transform _player)
    {
      Debug.Log($"<color=#fafafa>[RoomBase]</color> {dir}");
      switch(dir)
      {
        case DoorTrigger.ROOMDIRECTION.Left:
          _player.position = (Vector2)dTrigRight.transform.position+new Vector2(-2,0);
          break;
        case DoorTrigger.ROOMDIRECTION.Right:
          _player.position = (Vector2)dTrigLeft.transform.position+new Vector2(2,0);
          break;
        case DoorTrigger.ROOMDIRECTION.Up:
          _player.position = (Vector2)dTrigDown.transform.position+new Vector2(0,-2);
          break;
        case DoorTrigger.ROOMDIRECTION.Down:
          _player.position = (Vector2)dTrigUp.transform.position+new Vector2(0,2);
          break;
        default: break;
      }
    }

}
