using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemPickup : MonoBehaviour
{
    public enum ITEM_PICKUP_AVAILABLE_TYPES { Weapon, Equippable, Item };
    public ITEM_PICKUP_AVAILABLE_TYPES itemType;
    public string ItemName;

    PlayerActionButtons _pab;
    // Start is called before the first frame update
    void Start()
    {
      // EventManager.AddListener<PickupEvent>(OnPickupEvent);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnPickupEvent(PickupEvent evt) => PickupGameItem(evt);


    void OnTriggerEnter2D(Collider2D col)
    {
      if(col.gameObject.tag!="Player") { return; }

      var _psb = col.gameObject.GetComponent<PlayerStatsBase>();
      var foundPlayerGameManagers = FindObjectsByType<UserBase>(FindObjectsSortMode.None).Select(_u => _u);
      UserBase grabbedGM = foundPlayerGameManagers.Where(_u => _u.CurrentUser.UserId == _psb.UserId ).FirstOrDefault();
      _pab = grabbedGM.GetComponentInChildren<PlayerActionButtons>();
      

      _pab.showGrab = true;
    }

    // void PickupGameItem(PickupEvent evt)
    // {
    //   Debug.Log("")
    // }


    void OnTriggerExit2D(Collider2D col)
    {
      if(col.gameObject.tag!="Player") { return; }
      _pab.showGrab = false;
    }

    void OnDestroy()
    {
      try
      {
        _pab.showGrab = false;
        // EventManager.RemoveListener<PickupEvent>(OnPickupEvent);
      }
      catch (System.Exception) {}
    }

}
