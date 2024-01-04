using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlayerActionButtons : MonoBehaviour
{
    public GameObject b_Attack;
    public GameObject b_Skill1;
    public GameObject b_Skill2;
    public GameObject b_Skill3;
    public GameObject b_Skill4;

    public Weapon displayedWeapon;

    public bool showGrab=false; public bool showWeapon=true;
    PlayerStatsBase psb;

    // Start is called before the first frame update
    void Start()
    {
        var _psb = FindObjectsByType<PlayerStatsBase>(FindObjectsSortMode.None).Select(_u => _u);
        psb = _psb.Where(_u => _u.UserId == GetComponentInParent<UserBase>().CurrentUser.UserId ).FirstOrDefault();

        b_Attack.GetComponent<Button>().onClick.AddListener(HandleAttackButtonClick);

    }

    // Update is called once per frame
    void Update()
    {
      if(showGrab)
      {
        // Change this to a `grab` icon later
        var _txt = b_Attack.GetComponentInChildren<TextMeshProUGUI>();
        _txt.text = $"Pick Up";
        showWeapon=false;
      }
      else 
      {
        if(displayedWeapon != psb.playerEquipSlots.WeaponSlot1 || !showWeapon)
        {
          displayedWeapon = psb.playerEquipSlots.WeaponSlot1;
          string rarityColour = GameConstants.RarityColourDict[displayedWeapon.Rarity.ToString()];
          var _txt = b_Attack.GetComponentInChildren<TextMeshProUGUI>();
          _txt.text = $"<color={rarityColour}>{displayedWeapon.Name}</color>";
          showWeapon=true;
        }
      }
    }

    void HandleAttackButtonClick()
    {
      if(showGrab)
      {
        // Pickup Weapon
        Debug.Log($"[PlayerActionButtons] Pickup Button Hit");
        // PickupEvent evt = Events.PickupEvent;
        // evt.UserId = null;
        // EventManager.Broadcast(rcevt);
      }
      else 
      {
        // Trigger `Attack`
        Debug.Log($"[PlayerActionButtons] Attack Button Hit");
      }
    }
}
