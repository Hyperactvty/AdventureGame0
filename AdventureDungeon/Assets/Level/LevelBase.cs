using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    public RoomBase currentRoom;
    private IEnumerator coroutine_LoadRooms;

    // Start is called before the first frame update
    void Start()
    {
        coroutine_LoadRooms = RoomLoadCoroutine();
        StartCoroutine(coroutine_LoadRooms);

    }

    

    IEnumerator RoomLoadCoroutine() {
      WaitForSeconds wait = new WaitForSeconds(0.5f);
      yield return wait;
      Debug.Log("[LevelBase] Rooms Loaded, unloading...");
      OnLevelFirstLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelFirstLoad()
    {
      // Get all rooms add them to a `List`
      var /*List<RoomBase>*/ roomsInLevel = GameObject.FindGameObjectsWithTag("Level").Where(_r => _r.GetComponent<RoomBase>());
      foreach (var _rm in roomsInLevel)
      {
        try
        {
          Debug.Log($"[LevelBase] <color=#f00f00>{_rm.GetComponent<RoomBase>()}</color> != <color=#0F00F0>{currentRoom}</color>");
          if( _rm.GetComponent<RoomBase>().gameObject != currentRoom.gameObject) {
            _rm.SetActive(false);
          }
        } catch {}
      }
      if(currentRoom.roomType.ToString()!="Start")
      {
        currentRoom.OnRoomEnter();
      }
    }
}
