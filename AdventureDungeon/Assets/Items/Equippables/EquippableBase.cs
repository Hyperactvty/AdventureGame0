using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;

public enum EQUIPPABLESLOT { Head, Chest, Leg, Foot, Charm, Accessory/*, Weapon */}

[System.Serializable]
public class ItemStats
{
  public enum STATTYPES {
    Health,Health_Regen,Defence,
    Attack,Evade,Shield,Energy,
    Energy_Regen,Crit_Hit,Crit_Damage,
    Movement_Speed,Attack_Speed,Ignore_Defence,

    Loot_Range,Rare_Find,Epic_Find,Legend_Find,Coin_Multiplier,

    All_Resist,Fire_Resist,Water_Resist
  };
  public enum STATVALUES {
    Add, Percent, Subtract, SubtractPercent
  };
  public STATTYPES stat;
  public STATVALUES modifier;
  public float statValue;
  public float underlyingStatValue; // For stats like `%`, will have actual number instead of percent

  public ItemStats(string stat, string modifier, float statValue)
  {
    STATTYPES _stat; STATVALUES _modifier;
    Enum.TryParse<STATTYPES>(stat, out _stat);
    Enum.TryParse<STATVALUES>(modifier, out _modifier);
    this.stat = _stat;
    this.modifier = _modifier;
    this.statValue = statValue;
  }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
}

[System.Serializable]
public class Equippable
{
  // [SerializeField]
  [JsonProperty("itemid")]public string ItemId;// {get; set;}
  [JsonProperty("name")]public string Name;
  [JsonProperty("description")]public string Description;
  [JsonProperty("equippableslot")]public EQUIPPABLESLOT EquippableSlot;
  [JsonProperty("rarity")]public ItemRarity Rarity;
  [JsonProperty("gearset")]public GEARSETS GearSet;
  [JsonProperty("itemsprite")]public Texture2D ItemSprite;

  [JsonProperty("basedefence")]public int baseDefence;
  [JsonProperty("basehealth")]public int baseHealth;
  [JsonProperty("baseattack")]public int baseAttack;
  
  // Stat Bonuses
  [JsonProperty("itemstats")]public List<ItemStats> itemStats;// = new List<ItemStats>();

  public Equippable(
    string ItemId, string Name, string Description, 
    string EquippableSlot, string Rarity, 
    int baseDefence, int baseHealth, int baseAttack,
    List<ItemStats> itemStats, string gearSet = "GEARSETS.None", Texture2D ItemSprite = null)
  {
    this.ItemId = ItemId;
    this.Name = Name;
    this.Description = Description;
    // this.EquippableSlot = EquippableSlot;
    // this.Rarity = Rarity;
    // this.GearSet = GearSet;
    this.EquippableSlot = EquippableBase.GetEquipSlotMapping().FirstOrDefault(_slot => _slot.Key == EquippableSlot).Value;
    this.Rarity = EquippableBase.GetItemRarityMapping().FirstOrDefault(_rarity => _rarity.Key == Rarity).Value;
    // this.Rarity = Enum.GetName(typeof(Rarity), Rarity);
    // this.GearSet = (GEARSETS)GearSet;
    this.GearSet = (GEARSETS)Enum.Parse(typeof(GEARSETS), gearSet);
    this.ItemSprite = ItemSprite;
    
    this.baseDefence = baseDefence;
    this.baseHealth = baseHealth;
    this.baseAttack = baseAttack;
    
    this.itemStats = itemStats;
  }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
  public static Equippable Parse(string json)
  {
      return JsonUtility.FromJson<Equippable>(json);
  }
}

public enum WEAPONTYPE { Melee, Firearm }
public enum WEAPONSUBTYPE { 
  Melee_Non_Effect, Melee_Effect, 
  Firearm_Shotgun, Firearm_Rifle, Firearm_Cannon
}
// public Dictionary<string, string[]>() WEAPONSUBTYPE { {Melee,{"Non_Effect", "Effect"}}, {Firearm, {"Shotgun", "Rifle", "Cannon"}} }

[System.Serializable]
public class Weapon
{
  public readonly string ItemId;
  public string Name;
  public string Description;
  // public WEAPONTYPE WeaponType;
  // public WEAPONSUBTYPE WeaponSubtype;
  public ItemRarity Rarity;
  public GEARSETS GearSet;
  public Texture2D ItemSprite;

  public int baseAttack;
  public float baseAPS; // Attack per second

  public List<DroppableSource> droppableSources;

  public List<ItemStats> itemStats;
  

  public int weaponLevel;
  // public int stars; // Math.Floor(weaponLevel/10)

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
  public static Weapon Parse(string json)
  {
      return JsonUtility.FromJson<Weapon>(json);
  }
}

public class EquippableBase : MonoBehaviour
{

  /* private */
    public static Dictionary<string, EQUIPPABLESLOT> GetEquipSlotMapping()
    {
      return new Dictionary<string, EQUIPPABLESLOT>()
      {
        { "Head", EQUIPPABLESLOT.Head },
        { "Chest", EQUIPPABLESLOT.Chest },
        { "Leg", EQUIPPABLESLOT.Leg },
        { "Foot", EQUIPPABLESLOT.Foot },
        { "Charm", EQUIPPABLESLOT.Charm },
        { "Accessory", EQUIPPABLESLOT.Accessory },
        // { "Weapon", EQUIPPABLESLOT.Weapon }
      };
    }

    public static Dictionary<string, ItemRarity> GetItemRarityMapping()
    {
      return new Dictionary<string, ItemRarity>()
      {
        { "Common", ItemRarity.Common }, 
        { "Uncommon", ItemRarity.Uncommon }, 
        { "Rare", ItemRarity.Rare },
        { "Epic", ItemRarity.Epic },
        { "Legendary", ItemRarity.Legendary },
        { "Mythic", ItemRarity.Mythic },
        { "Limited", ItemRarity.Limited }
      };
    }

    public static Dictionary<string, ItemStats.STATTYPES> GetStatTypeMapping()
    {
      return new Dictionary<string, ItemStats.STATTYPES>()
      {
        // { "Head", ItemStats.STATTYPES.Head },
        // { "Chest", ItemStats.STATTYPES.Chest },
        // { "Leg", ItemStats.STATTYPES.Leg },
        // { "Boot", ItemStats.STATTYPES.Boot },
        // { "Charm", ItemStats.STATTYPES.Charm },
        // { "Accessory", ItemStats.STATTYPES.Accessory },
      };
    }

    public static Dictionary<string, ItemStats.STATVALUES> GetStatModifierMapping()
    {
      return new Dictionary<string, ItemStats.STATVALUES>()
      {
        { "Add", ItemStats.STATVALUES.Add },
        { "Percent", ItemStats.STATVALUES.Percent },
        { "Subtract", ItemStats.STATVALUES.Subtract },
      };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
