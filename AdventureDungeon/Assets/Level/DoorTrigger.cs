using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public RoomBase roomLink;
    RoomBase baseRoom; // door from getting from `A` to `B`
    public enum ROOMDIRECTION {Up, Left, Down, Right, NULL};
    public ROOMDIRECTION doorDirection; // (1,0) for right, (0,-1) for down, ect.

    public Color enabledColour = Color.white;
    public Color disabledColour = Color.gray;

    bool doorActive=false;
    
    // Start is called before the first frame update
    void Start()
    {
      baseRoom = transform.parent.GetComponent<RoomBase>();
      gameObject.GetComponent<SpriteRenderer>().color=doorActive ? enabledColour : disabledColour; 
      gameObject.GetComponent<BoxCollider2D>().enabled = doorActive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDoor(bool b)
    {
      doorActive = b; 
      gameObject.GetComponent<SpriteRenderer>().color=b ? enabledColour : disabledColour; 
      gameObject.GetComponent<BoxCollider2D>().enabled = b;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
      if(col.gameObject.tag != "Player") { return; }

      RoomLoadScreenEvent rcevt = Events.RoomLoadScreenEvent;
      rcevt.NextRoom = roomLink.gameObject;
      rcevt.BaseRoom = baseRoom.gameObject;
      rcevt._pCC = col.gameObject.GetComponent<PlayerCharacterController>();
      // rcevt.roomDir = doorDirection;
      EventManager.Broadcast(rcevt);

      var _pCC = col.gameObject.GetComponent<PlayerCharacterController>();
      // trigger black screen, unload current room, load new room, then place player near the door.
      return;
      roomLink.gameObject.SetActive(true);
      baseRoom.gameObject.SetActive(false);
      roomLink.DoTransport(doorDirection, _pCC.transform);
    }

}
