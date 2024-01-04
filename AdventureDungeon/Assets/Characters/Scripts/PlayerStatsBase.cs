using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using UnityEngine;

[System.Serializable]
public class PlayerStats 
{  
  public float Health = 100f;// { get; set; }
  public float Health_Regen = 1;
  public int Defence=25;
  public int Attack=0;
  public int Evade=5;
  public int Shield=0;
  public float Energy=10;
  public float Energy_Regen=1;
  public float Crit_Hit=5;
  public float Crit_Damage=50; // Add 50% to the base damage
  public float Movement_Speed=4;
  public float Attack_Speed=0;
  public float Ignore_Defence = 0;

  public float Loot_Range=1;
  public float Rare_Find=0;
  public float Epic_Find=0;
  public float Legend_Find=0;
  public float Coin_Multiplier=0;

  public int All_Resist=0;
  public int Fire_Resist=0;
  public int Water_Resist=0;

  // public PlayerStats(
  //   float Health,float Health_Regen,int Defence,int Attack,int Evade,int Shield,
  //   float Energy,float Energy_Regen,float Crit_Hit,float Crit_Damage,float Movement_Speed,
  //   float Attack_Speed,float Ignore_Defence,

  //   float Loot_Range,float Rare_Find,float Epic_Find,float Legend_Find,float Coin_Multiplier,

  //   int All_Resist,int Fire_Resist,int Water_Resist
  // )
  // {
  //   PlayerStats _ps = new PlayerStats();
  //   _ps.Health = Health;
  //   _ps.Health_Regen = Health_Regen;
  //   _ps.Defence=Defence;
  //   _ps.Attack=Attack;
  //   _ps.Evade=Evade;
  //   _ps.Shield=Shield;
  //   _ps.Energy=Energy;
  //   _ps.Energy_Regen=Energy_Regen;
  //   _ps.Crit_Hit=Crit_Hit;
  //   _ps.Crit_Damage=Crit_Damage;
  //   _ps.Movement_Speed=Movement_Speed;
  //   _ps.Attack_Speed=Attack_Speed;
  //   _ps.Ignore_Defence = Ignore_Defence;

  //   _ps.Loot_Range=Loot_Range;
  //   _ps.Rare_Find=Rare_Find;
  //   _ps.Epic_Find=Epic_Find;
  //   _ps.Legend_Find=Legend_Find;
  //   _ps.Coin_Multiplier=Coin_Multiplier;

  //   _ps.All_Resist=All_Resist;
  //   _ps.Fire_Resist=Fire_Resist;
  //   _ps.Water_Resist=Water_Resist;

  //   return _ps;
  // }
  public PlayerStats() {  }

  public PlayerStats( PlayerStats _ps )
  {
    this.Health =           _ps.Health;
    this.Health_Regen =     _ps.Health_Regen;
    this.Defence =          _ps.Defence;
    this.Attack =           _ps.Attack;
    this.Evade =            _ps.Evade;
    this.Shield =           _ps.Shield;
    this.Energy =           _ps.Energy;
    this.Energy_Regen=      _ps.Energy_Regen;
    this.Crit_Hit=          _ps.Crit_Hit;
    this.Crit_Damage=       _ps.Crit_Damage;
    this.Movement_Speed=    _ps.Movement_Speed;
    this.Attack_Speed=      _ps.Attack_Speed;
    this.Ignore_Defence =   _ps.Ignore_Defence;

    this.Loot_Range=        _ps.Loot_Range;
    this.Rare_Find=         _ps.Rare_Find;
    this.Epic_Find=         _ps.Epic_Find;
    this.Legend_Find=       _ps.Legend_Find;
    this.Coin_Multiplier=   _ps.Coin_Multiplier;

    this.All_Resist=        _ps.All_Resist;
    this.Fire_Resist=       _ps.Fire_Resist;
    this.Water_Resist=      _ps.Water_Resist;

    // return this;
  }

  // [SerializeField]
  // private float _Health = 100;// { get; set; }
  // [SerializeField]
  // private float _Health_Regen = 1;
  // [SerializeField]
  // private int _Defence=25;
  // [SerializeField]
  // private int _Attack=0;
  // [SerializeField]
  // private int _Evade=5;
  // [SerializeField]
  // private int _Shield=0;
  // [SerializeField]
  // private float _Energy=10;
  // [SerializeField]
  // private float _Energy_Regen=1;
  // [SerializeField]
  // private float _Crit_Hit=5;
  // [SerializeField]
  // private float _Crit_Damage=50; // Add 50% to the base damage
  // [SerializeField]
  // private float _Movement_Speed=4;
  // [SerializeField]
  // private float _Attack_Speed=0;
  // [SerializeField]
  // private float _Ignore_Defence = 0;

  // [SerializeField]
  // private float _Loot_Range=1;
  // [SerializeField]
  // private float _Rare_Find=0;
  // [SerializeField]
  // private float _Epic_Find=0;
  // [SerializeField]
  // private float _Legend_Find=0;
  // [SerializeField]
  // private float _Coin_Multiplier=0;

  // [SerializeField]
  // private int _All_Resist=0;
  // [SerializeField]
  // private int _Fire_Resist=0;
  // [SerializeField]
  // private int _Water_Resist=0;

  public float _Health {
      get { return Health; }
      set { if (Health != value) { Health = value; } }
  }
  // public float _Health_Regen {
  //     get { return Health_Regen; }
  //     set { if (Health_Regen != value) { Health_Regen = value; } }
  // }
  // public int Defence {
  //     get { return _Defence; }
  //     set { if (_Defence != value) { _Defence = value; } }
  // }
  public float _Attack {
      get { return Attack; }
      set { if (Attack != value) { Attack = (int)value/*Mathf.Ceil(value)*/; } }
  }
  // public int Evade {
  //     get { return _Evade; }
  //     set { if (_Evade != value) { _Evade = value; } }
  // }
  // public int Shield {
  //     get { return _Shield; }
  //     set { if (_Shield != value) { _Shield = value; } }
  // }
  // public float Energy {
  //     get { return _Energy; }
  //     set { if (_Energy != value) { _Energy = value; } }
  // }
  // public float Energy_Regen {
  //     get { return _Energy_Regen; }
  //     set { if (_Energy_Regen != value) { _Energy_Regen = value; } }
  // }
  // public float Crit_Hit {
  //     get { return _Crit_Hit; }
  //     set { if (_Crit_Hit != value) { _Crit_Hit = value; } }
  // }
  // public float Crit_Damage {
  //     get { return _Crit_Damage; }
  //     set { if (_Crit_Damage != value) { _Crit_Damage = value; } }
  // }
  public float _Movement_Speed {
      get { return Movement_Speed; }
      set { if (Movement_Speed != value) { Movement_Speed = value; } }
  }
  // public float Attack_Speed {
  //     get { return _Attack_Speed; }
  //     set { if (_Attack_Speed != value) { _Attack_Speed = value; } }
  // }
  // public float Ignore_Defence {
  //     get { return _Ignore_Defence; }
  //     set { if (_Ignore_Defence != value) { _Ignore_Defence = value; } }
  // }

  // public float Loot_Range {
  //     get { return _Loot_Range; }
  //     set { if (_Loot_Range != value) { _Loot_Range = value; } }
  // }
  // public float Rare_Find {
  //     get { return _Rare_Find; }
  //     set { if (_Rare_Find != value) { _Rare_Find = value; } }
  // }
  // public float Epic_Find {
  //     get { return _Epic_Find; }
  //     set { if (_Epic_Find != value) { _Epic_Find = value; } }
  // }
  // public float Legend_Find {
  //     get { return _Legend_Find; }
  //     set { if (_Legend_Find != value) { _Legend_Find = value; } }
  // }
  // public float Coin_Multiplier {
  //     get { return _Coin_Multiplier; }
  //     set { if (_Coin_Multiplier != value) { _Coin_Multiplier = value; } }
  // }

  // public int All_Resist {
  //     get { return _All_Resist; }
  //     set { if (_All_Resist != value) { _All_Resist = value; } }
  // }
  // public int Fire_Resist {
  //     get { return _Fire_Resist; }
  //     set { if (_Fire_Resist != value) { _Fire_Resist = value; } }
  // }
  // public int Water_Resist {
  //     get { return _Water_Resist; }
  //     set { if (_Water_Resist != value) { _Water_Resist = value; } }
  // }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
  public static PlayerStats Parse(string json)
  {
      return JsonUtility.FromJson<PlayerStats>(json);
  }

}

[System.Serializable]
public class EquipSlots
{
  public Equippable HeadSlot;
  public Equippable ChestSlot;
  public Equippable LegSlot;
  public Equippable CharmSlot;
  public Equippable AccessorySlot;
  public Equippable FootSlot;
  
  public Weapon WeaponSlot1;
  public Weapon WeaponSlot2;
  public Weapon WeaponSlot3;

  public Equippable[] GetSlots()
  {
    Equippable[] s = {
      HeadSlot,
      ChestSlot,
      LegSlot,
      CharmSlot,
      AccessorySlot,
      FootSlot,
      // WeaponSlot1,
      // WeaponSlot2,
      // WeaponSlot3
    };
    return s;
  }

  public Weapon[] GetWeaponSlots()
  {
    Weapon[] s = {
      WeaponSlot1,
      WeaponSlot2,
      WeaponSlot3
    };
    return s;
  }

  public string Stringify() 
  {
      return JsonUtility.ToJson(this);
  }
  public static EquipSlots Parse(string json)
  {
      return JsonUtility.FromJson<EquipSlots>(json);
  }
}

public class PlayerStatsBase : MonoBehaviour
{

  class StatCalculations
  {
    public float Rare_Find(int _rf) => _rf/20;
    public float Epic_Find(int _ef) => _ef/35;
    public float Legend_Find(int _lf) => _lf/20; //original : 20, maybe 50..?
  }
    
    private System.Random rand = new System.Random();
    Health m_Health;

    public string UserId;
    public PlayerStats playerStats = new PlayerStats();
    public PlayerStats basePlayerStats; 
    bool updateHealthBarFirstTime = true;
    // Note: May have to replace playerStats for security
    public PlayerStats _assignPlayerStats { 
      get {return _aps;} 
      set {
        if (_aps != value)
        {
            // DO SOMETHING HERE
            Debug.Log("UPDATE USER STATS");
            playerStats = value;
            
            Debug.Log($"_aps Assign > {value.Attack}");
            Debug.Log(value.Attack);
            m_Health.MaxHealth = value.Health;
            if(updateHealthBarFirstTime) {
              m_Health.CurrentHealth = value.Health;
              updateHealthBarFirstTime = false;
            }
        }
        _aps = value;
      }
    }
    private PlayerStats _aps;

    public EquipSlots playerEquipSlots = new EquipSlots();
    public EquipSlots priorPlayerEquipSlots = new EquipSlots();
    // Note: May have to replace playerEquipSlots for security
    public EquipSlots _assignPlayerEquipSlots { 
      get {return _apes;} 
      set {
        if (_apes != value)
        {
            // DO SOMETHING HERE
            Debug.Log("UPDATE USER EQUIPS");
            // foreach (var _stat in value.itemStats)
            // foreach (var _stat in value.itemStats)
            // {
            //   CalculateItemStat(_stat);
            // }
            
            // playerStats = value;

            
            
            Debug.Log($"_apes Assign > {value}");
        }

        Equippable tpEq;

        // Debug.Log($"_apes Value > {value}");
            string[] s = {"HeadSlot","ChestSlot","LegSlot","CharmSlot","AccessorySlot","FootSlot","WeaponSlot1"}; bool doAddStat=false;
            var pPES = priorPlayerEquipSlots;
          
            Debug.Log($"\nPrior Head : {priorPlayerEquipSlots.HeadSlot.Name}\t|\tCurrent Head : {playerEquipSlots.HeadSlot.Name}");
            Debug.Log($"\nPrior Chest : {priorPlayerEquipSlots.ChestSlot.Name}\t|\tCurrent Chest : {playerEquipSlots.ChestSlot.Name}");
            Debug.Log($"\nPrior Leg : {priorPlayerEquipSlots.LegSlot.Name}\t|\tCurrent Leg : {playerEquipSlots.LegSlot.Name}");
            Debug.Log($"\nPrior Foot : {priorPlayerEquipSlots.FootSlot.Name}\t|\tCurrent Foot : {playerEquipSlots.FootSlot.Name}");
            Debug.Log($"\nPrior Weapon1 : {priorPlayerEquipSlots.WeaponSlot1.Name}\t|\tCurrent Weapon1 : {playerEquipSlots.WeaponSlot1.Name}");

            Equippable _i; Equippable _pI;
            Weapon _w; Weapon _pW;
            // for (int i = 0; i < 2; i++)
            // {
              foreach (var _eq in s)
              {
                List<ItemStats> modItemStats = null;
                // Debug.Log($"PARSING > {JObject.Parse(value.Stringify())[_eq]}");
                // Debug.Log($"PARSING OTHER > {JObject.Parse(_apes.Stringify())[_eq]}");

                // Debug.Log($"PARSING 2 > {Equippable.Parse(JObject.Parse(value.Stringify())[_eq].ToString()).ItemId}");
                // tpEq = Equippable.Parse(JObject.Parse(value.Stringify())[_eq].ToString());
                // Debug.Log($"OTHER TYPE > {Equippable.Parse(JObject.Parse(_apes.Stringify())[_eq].ToString()).ItemId}");
                // Debug.Log($"COMPARING > {tpEq.ItemId == Equippable.Parse(JObject.Parse(_apes.Stringify())[_eq].ToString()).ItemId}");


                // Debug.Log($"COMPARING > {tpEq.ItemId == Equippable.Parse(JObject.Parse(_apes.Stringify())[_eq].ToString())}");

                // tpEq = JsonConvert.DeserializeObject<Equippable>(JObject.Parse(value.Stringify())[_eq].ToString()/*, out tpEq*/);

                // Debug.Log($"Parsed Result > {tpEq.ItemId}");

                switch (_eq)
                {
                  case "HeadSlot": 
                    _pI = pPES.HeadSlot;
                    _i = value.HeadSlot;
                    // _i = !doAddStat ? pPES.HeadSlot : value.HeadSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats);

                    // CalculateItemStat(!doAddStat ? pPES.HeadSlot.itemStats : value.HeadSlot.itemStats, doAddStat, out playerStats);
                    // CalculateItemStat(pPES.HeadSlot.itemStats, value.HeadSlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "ChestSlot": 
                    _pI = pPES.ChestSlot;
                    _i = value.ChestSlot;
                    // _i = !doAddStat ? pPES.ChestSlot : value.ChestSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats);

                    // CalculateItemStat(!doAddStat ? pPES.ChestSlot.itemStats : value.ChestSlot.itemStats, doAddStat, out playerStats);
                    // CalculateItemStat(pPES.ChestSlot.itemStats, value.ChestSlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "LegSlot": 
                    _pI = pPES.LegSlot;
                    _i = value.LegSlot;
                    // _i = !doAddStat ? pPES.LegSlot : value.LegSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats);
                    if(modItemStats!=null)
                    {
                      Debug.Log("Does in fact have modified stats (output the `.Stringify()` stats...)");
                      value.LegSlot.itemStats = modItemStats;
                    }

                    // CalculateItemStat(!doAddStat ? pPES.LegSlot.itemStats : value.LegSlot.itemStats, doAddStat, out playerStats);
                    // CalculateItemStat(pPES.LegSlot.itemStats, value.LegSlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "CharmSlot": 
                    _pI = pPES.CharmSlot;
                    _i = value.CharmSlot;
                    // _i = !doAddStat ? pPES.CharmSlot : value.CharmSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats);

                    // CalculateItemStat(!doAddStat ? pPES.CharmSlot.itemStats : value.CharmSlot.itemStats, doAddStat, out playerStats);
                    // CalculateItemStat(pPES.CharmSlot.itemStats, value.CharmSlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "AccessorySlot": 
                    _pI = pPES.AccessorySlot;
                    _i = value.AccessorySlot;
                    // _i = !doAddStat ? pPES.AccessorySlot : value.AccessorySlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats);

                    // CalculateItemStat(!doAddStat ? pPES.AccessorySlot.itemStats : value.AccessorySlot.itemStats, doAddStat, out playerStats);
                    // CalculateItemStat(pPES.AccessorySlot.itemStats, value.AccessorySlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "FootSlot": 
                    _pI = pPES.FootSlot;
                    _i = value.FootSlot;
                    // _i = !doAddStat ? pPES.FootSlot : value.FootSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);  

                    CalculateItemStat(_i.itemStats, _i, _pI.itemStats, _pI, doAddStat, out playerStats, out modItemStats); 
                    if(modItemStats!=null)
                    {
                      Debug.Log("Does in fact have modified stats");
                      value.FootSlot.itemStats = modItemStats;
                    }

                    // CalculateItemStat(!doAddStat ? pPES.FootSlot.itemStats : value.FootSlot.itemStats, doAddStat, out playerStats);                    
                    // CalculateItemStat(pPES.FootSlot.itemStats, value.FootSlot.itemStats, doAddStat, out playerStats);
                    break;
                  case "WeaponSlot1": 
                    _pW = pPES.WeaponSlot1;
                    _w = value.WeaponSlot1;
                    // _i = !doAddStat ? pPES.FootSlot : value.FootSlot;
                    // CalculateItemStat(_i.itemStats, _i, doAddStat, out playerStats);  

                    CalculateWeaponStat(_w.itemStats, _w, _pW.itemStats, _pW, doAddStat, out playerStats, out modItemStats); 
                    if(modItemStats!=null)
                    {
                      Debug.Log("Does in fact have modified stats");
                      value.WeaponSlot1.itemStats = modItemStats;
                    }

                    // CalculateItemStat(!doAddStat ? pPES.FootSlot.itemStats : value.FootSlot.itemStats, doAddStat, out playerStats);                    
                    // CalculateItemStat(pPES.FootSlot.itemStats, value.FootSlot.itemStats, doAddStat, out playerStats);
                    break;
                  default: break;
                }
                // Debug.Log($"_apes Current Value > {_eq.Name}");
                // CalculateItemStat(_stat);
              }
            //   doAddStat = true;
            // }
            
        
        _apes = value;
        // priorPlayerEquipSlots = _apes;
      }
    }
    private EquipSlots _apes;

    // public BuffsBase[] buffs = new BuffsBase();[]

    // void CalculateItemStat(List<ItemStats> _stat,/* List<ItemStats> _priorStat,*/ Equippable _i, bool _doAddStat, out PlayerStats _playerStats)
    // {
    //   /* TODO: have a better system for retrieving base stats */
    //   playerStats = basePlayerStats;

    //   playerStats.Health += _doAddStat ? _i.baseHealth : -_i.baseHealth;
    //   playerStats.Defence += _doAddStat ? _i.baseDefence : -_i.baseDefence;
    //   playerStats.Attack += _doAddStat ? _i.baseAttack : -_i.baseAttack;

    //   System.Reflection.PropertyInfo pinfo;
    //   foreach (var _s in _stat)
    //   {
    //     float parsedStat = (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()];
    //     switch (_s.modifier.ToString())
    //     {
    //       case "Add":
    //         pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
    //         pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue, null);
    //         /* Might Work */
    //         // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

    //         break;
    //       case "Subtract":
    //         pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
    //         pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + (-_s.statValue) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (-_s.statValue), null);
    //         /* Might Work */
    //         // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

    //         break;
    //       case "Percent":
    //         pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
    //         var _do = _doAddStat?"+":"-";
    //         var _r = _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f))) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f)));
    //         Debug.Log($"Modifying (%) : {parsedStat} {_do} ({parsedStat} * (({_s.statValue}/100)+1))");
    //         Debug.Log($"Modifying (%) Total : {_r}");
    //         pinfo.SetValue(playerStats, _doAddStat ? parsedStat + (parsedStat * ((_s.statValue/100.0f))) : parsedStat - (parsedStat * ((_s.statValue/100.0f))), null);
    //         /* Might Work */
    //         // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

    //         break;
    //       // case "SubtractPercent":
    //       //   pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
    //       //   pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] * ((-_s.statValue/100)+1) : (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] / ((-_s.statValue/100)+1), null);
    //       //   /* Might Work */
    //       //   // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

    //       //   break;
    //       default: break;
    //     }
    //   }

    //   // playerStats = editingStats;
    //   // Debug.Log($"After All stat change > {playerStats.Health}");
    //   _playerStats = playerStats;
    // }

    void CalculateItemStat(List<ItemStats> _stat, Equippable _i, List<ItemStats> _priorStat, Equippable _pI, bool _doAddStat, out PlayerStats _playerStats, out List<ItemStats> _modItemStat)
    {
      /* TODO: have a better system for retrieving base stats */
      playerStats = basePlayerStats;

      playerStats.Health += _i.baseHealth + (-_pI.baseHealth);
      playerStats.Defence += _i.baseDefence + (-_pI.baseDefence);
      playerStats.Attack += _i.baseAttack + (-_pI.baseAttack);
      
      // PlayerStats editStats = new PlayerStats(playerStats);

      System.Reflection.PropertyInfo pinfo; bool doAddStat=false; bool hasModifiedStat=false;
      List<ItemStats>[] _iS_P_C = {_priorStat, _stat};
      for (int i = 0; i < 2; i++)
      {
        foreach (var _s in _iS_P_C[i])
        {
          // DEBUG ONLY
          var _do="";
          var _r =0f;
          float parsedStat = (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()];
          ItemStats editStat = null;//new ItemStats();
          switch (_s.modifier.ToString())
          {
            case "Add":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");

              _do = doAddStat?"+":"-";
              _r = doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue;
              Debug.Log($"Modifying (+) : {parsedStat} {_do} {_s.statValue}");
              Debug.Log($"Modifying (+) Total : {_r}");

              // pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue, null);
              pinfo.SetValue(playerStats, doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue, null);

              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            case "Subtract":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
              // pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + (-_s.statValue) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (-_s.statValue), null);
              pinfo.SetValue(playerStats, doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + (-_s.statValue) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (-_s.statValue), null);
              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            case "Percent":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
              if(doAddStat)
              {
                var sing = _stat.SingleOrDefault(s => s == _s);
                Debug.Log($"sing ({_s.stat}) : {sing.Stringify()}");
                Debug.Log($"Note: Find item in list and remove it, then add the stat in it's place.");

                editStat = sing;

                _s.underlyingStatValue = ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f)));
                // _modItemStat = _s;
                Debug.Log($"Modified UnderlyingStatValue -> {_s.underlyingStatValue}");
                hasModifiedStat=true;
                
                // _stat = _s;
              }

              _do = doAddStat?"+":"-";
              _r = doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f))) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (_s.underlyingStatValue);
              string pctStr = doAddStat ? $"({parsedStat} * (({_s.statValue}/100)+1))" : $"{_s.underlyingStatValue}";
              Debug.Log($"Modifying {_s.stat} (%) : {parsedStat} {_do} {pctStr}");
              Debug.Log($"Modifying {_s.stat} (%) Total : {_r}");
              pinfo.SetValue(playerStats, Mathf.Abs(doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f))):parsedStat - _s.underlyingStatValue), null);
              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            // case "SubtractPercent":
            //   pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
            //   pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] * ((-_s.statValue/100)+1) : (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] / ((-_s.statValue/100)+1), null);
            //   /* Might Work */
            //   // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

            //   break;
            default: break;
          }
        }
        doAddStat = true;
      }

      // playerStats = editingStats;
      // Debug.Log($"After All stat change > {playerStats.Health}");
      _playerStats = playerStats;
      _modItemStat = hasModifiedStat ? _stat : null;
      // _modItemStat= hasModifiedStat ? _modItemStat : null;
    }

    void CalculateWeaponStat(List<ItemStats> _stat, Weapon _i, List<ItemStats> _priorStat, Weapon _pI, bool _doAddStat, out PlayerStats _playerStats, out List<ItemStats> _modItemStat)
    {
      /* TODO: have a better system for retrieving base stats */
      playerStats = basePlayerStats;

      playerStats.Attack += _i.baseAttack + (-_pI.baseAttack);
      
      // PlayerStats editStats = new PlayerStats(playerStats);

      System.Reflection.PropertyInfo pinfo; bool doAddStat=false; bool hasModifiedStat=false;
      List<ItemStats>[] _iS_P_C = {_priorStat, _stat};
      for (int i = 0; i < 2; i++)
      {
        foreach (var _s in _iS_P_C[i])
        {
          // DEBUG ONLY
          var _do="";
          var _r =0f;
          float parsedStat = (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()];
          ItemStats editStat = null;//new ItemStats();
          switch (_s.modifier.ToString())
          {
            case "Add":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");

              _do = doAddStat?"+":"-";
              _r = doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue;
              Debug.Log($"Modifying WEAPON (+) : {parsedStat} {_do} {_s.statValue}");
              Debug.Log($"Modifying WEAPON (+) Total : {_r}");

              // pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue, null);
              pinfo.SetValue(playerStats, doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + _s.statValue : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - _s.statValue, null);

              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            case "Subtract":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
              // pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + (-_s.statValue) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (-_s.statValue), null);
              pinfo.SetValue(playerStats, doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + (-_s.statValue) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (-_s.statValue), null);
              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            case "Percent":
              pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
              if(doAddStat)
              {
                var sing = _stat.SingleOrDefault(s => s == _s);
                Debug.Log($"sing WEAPON ({_s.stat}) : {sing.Stringify()}");
                Debug.Log($"Note: Find item in list and remove it, then add the stat in it's place.");

                editStat = sing;

                _s.underlyingStatValue = ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f)));
                // _modItemStat = _s;
                Debug.Log($"Modified UnderlyingStatValue -> {_s.underlyingStatValue}");
                hasModifiedStat=true;
                
                // _stat = _s;
              }

              _do = doAddStat?"+":"-";
              _r = doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f))) : (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] - (_s.underlyingStatValue);
              string pctStr = doAddStat ? $"({parsedStat} * (({_s.statValue}/100)+1))" : $"{_s.underlyingStatValue}";
              Debug.Log($"Modifying WEAPON {_s.stat} (%) : {parsedStat} {_do} {pctStr}");
              Debug.Log($"Modifying WEAPON {_s.stat} (%) Total : {_r}");
              pinfo.SetValue(playerStats, doAddStat ? (float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] + ((float)JObject.Parse(playerStats.Stringify())[_s.stat.ToString()] * ((_s.statValue/100.0f))):parsedStat - _s.underlyingStatValue, null);
              /* Might Work */
              // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

              break;
            // case "SubtractPercent":
            //   pinfo = playerStats.GetType().GetProperty($"_{_s.stat}");
            //   pinfo.SetValue(playerStats, _doAddStat ? (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] * ((-_s.statValue/100)+1) : (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] / ((-_s.statValue/100)+1), null);
            //   /* Might Work */
            //   // pinfo.SetValue(playerStats, (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()] + _s.statValue - (float)JObject.Parse(basePlayerStats.Stringify())[_s.stat.ToString()], null);

            //   break;
            default: break;
          }
        }
        doAddStat = true;
      }

      // playerStats = editingStats;
      // Debug.Log($"After All stat change > {playerStats.Health}");
      _playerStats = playerStats;
      _modItemStat = hasModifiedStat ? _stat : null;
      // _modItemStat= hasModifiedStat ? _modItemStat : null;
    }

    void UpdateBasePlayerStats() =>  basePlayerStats = new PlayerStats(playerStats); 

    // Start is called before the first frame update
    void Start()
    {
      m_Health = GetComponent<Health>();

      UpdateBasePlayerStats();
      
      string query =
      @"query ($refUserId: String!) {
          getUserPlayer(refUserId: $refUserId) {
            playerStats {
              Health
              Health_Regen
              Defence
              Attack
              Evade
              Shield
              Energy
              Energy_Regen
              Crit_Hit
              Crit_Damage
              Movement_Speed
              Attack_Speed
              Ignore_Defence
              Loot_Range
              Rare_Find
              Epic_Find
              Legend_Find
              Coin_Multiplier
              All_Resist
              Fire_Resist
              Water_Resist
            }
            equipSlots {
              HeadSlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
              ChestSlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
              LegSlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
              BootSlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
              CharmSlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
              AccessorySlot {
                refItemId
                itemLevel
                itemStats {stat, modifier,statValue}
              },
            }
          }
      }";
      GraphQL.APIGraphQL.Query(query, new {refUserId = UserId}, response => _assignPlayerStats = response.Get<PlayerStats>("getUserPlayer")); 
      Debug.Log($"_aps > {_assignPlayerStats}");

      _assignPlayerEquipSlots = playerEquipSlots;
      
      // priorPlayerEquipSlots = playerEquipSlots;

      // foreach (var _stat in _assignPlayerStats)
      // {
      //   Debug.Log($"Stat `{_stat}`");
      // }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
          List<Equippable> EL = UserBase.EquippableList;
          List<Weapon> WL = UserBase.WeaponList;
          Debug.Log($"GetButtonDown (PlayerStatsBase) > Change user's gear");
          string[] s = {"HeadSlot","ChestSlot","LegSlot","CharmSlot","AccessorySlot","FootSlot", "WeaponSlot1"}; int slotIndex = 0;
          // foreach (var _slotOption in playerEquipSlots.GetSlots())
          Debug.Log($"Prior Head : {priorPlayerEquipSlots.HeadSlot.Name}\t|\tCurrent Head : {playerEquipSlots.HeadSlot.Name}");
          // priorPlayerEquipSlots = playerEquipSlots;

          foreach (var _slotOption in priorPlayerEquipSlots.GetSlots())
          {
            try
            {

              switch (s[slotIndex])
              {
                case "HeadSlot": 
                  priorPlayerEquipSlots.HeadSlot = playerEquipSlots.HeadSlot;
                  // _assignPlayerEquipSlots.HeadSlot = _equippableForSlot;
                  break;
                case "ChestSlot": 
                  priorPlayerEquipSlots.ChestSlot = playerEquipSlots.ChestSlot;
                  // _assignPlayerEquipSlots.ChestSlot = _equippableForSlot;
                  break;
                case "LegSlot": 
                  priorPlayerEquipSlots.LegSlot = playerEquipSlots.LegSlot;
                  // _assignPlayerEquipSlots.LegSlot = _equippableForSlot;
                  break;
                case "CharmSlot": 
                  priorPlayerEquipSlots.CharmSlot = playerEquipSlots.CharmSlot;
                  // _assignPlayerEquipSlots.CharmSlot = _equippableForSlot;
                  break;
                case "AccessorySlot": 
                  priorPlayerEquipSlots.AccessorySlot = playerEquipSlots.AccessorySlot;
                  // _assignPlayerEquipSlots.AccessorySlot = _equippableForSlot;
                  break;
                case "FootSlot": 
                  priorPlayerEquipSlots.FootSlot = playerEquipSlots.FootSlot;
                  // _assignPlayerEquipSlots.FootSlot = _equippableForSlot;
                  break;
                default: break;
              }

              // Debug.Log($"PlayerSlot ({s[slotIndex]}) > {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
            }
            catch (System.Exception e) { Debug.Log($"Error (PlayerStatBase_PriorSlotAssign) > {e}"); }
            finally { slotIndex++; }
          }

          foreach (var _slotOption in priorPlayerEquipSlots.GetWeaponSlots())
          {
            try
            {

              switch (s[slotIndex])
              {
                case "WeaponSlot1": 
                  priorPlayerEquipSlots.WeaponSlot1 = playerEquipSlots.WeaponSlot1;
                  // _assignPlayerEquipSlots.FootSlot = _equippableForSlot;
                  break;
                default: break;
              }

              // Debug.Log($"PlayerSlot ({s[slotIndex]}) > {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
            }
            catch (System.Exception e) { Debug.Log($"Error (PlayerStatBase_PriorSlotAssign) > {e}"); }
            finally { slotIndex++; }
          }

          slotIndex=0;

          foreach (var _slotOption in playerEquipSlots.GetSlots())
          {
            try
            {
              // string tempRes = _slotOption.ToString().Remove(_slotOption.ToString().IndexOf("Slot"));
              string tempRes = s[slotIndex].ToString().Remove(s[slotIndex].ToString().IndexOf("Slot"));
              EQUIPPABLESLOT _slotAssign;
              Enum.TryParse<EQUIPPABLESLOT>(tempRes, out _slotAssign);
              List<Equippable> _slotEquippables = EL.Where(_e => _e.EquippableSlot == _slotAssign).ToList();
              // var _slotString = EL.Where(_e => _e.EquippableSlot == EQUIPPABLESLOT.Chest).Select(_e => _e.Name);
              var _slotString = _slotEquippables.Select(_e => _e.Name);
              Debug.Log($"Items available for type [{tempRes}] : {string.Join<string>(", ", _slotString)}");
              Equippable _equippableForSlot = _slotEquippables[rand.Next(_slotEquippables.Count)];

              List<Weapon> _slotWeapons = WL.Select(_w => _w).ToList();
              Weapon _weaponForSlot = _slotWeapons[rand.Next(_slotWeapons.Count)];

              Debug.Log($"Equippable chosen for type [{tempRes}] : {_equippableForSlot.Name}");
              
              // Debug.Log($"playerEquipSlots.GetType().GetProperty({s[slotIndex]}).SetValue(playerEquipSlots, {_equippableForSlot}, null)");
              // Debug.Log($"Property (GT) : {playerEquipSlots.GetType()}");
              // Debug.Log($"Property : {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
              // playerEquipSlots.GetType().GetProperty(s[slotIndex]).SetValue(playerEquipSlots.GetType().GetProperty(s[slotIndex]), _equippableForSlot, null);
              // Debug.Log($"Property 2 : {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");

              // playerEquipSlots.GetType().GetProperty(s[slotIndex]).SetValue(playerEquipSlots, _equippableForSlot, null);
              // // playerEquipSlots.ChestSlot = _equippableForSlot;

              switch (s[slotIndex])
              {
                case "HeadSlot": 
                  playerEquipSlots.HeadSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.HeadSlot = _equippableForSlot;
                  break;
                case "ChestSlot": 
                  playerEquipSlots.ChestSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.ChestSlot = _equippableForSlot;
                  break;
                case "LegSlot": 
                  playerEquipSlots.LegSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.LegSlot = _equippableForSlot;
                  break;
                case "CharmSlot": 
                  playerEquipSlots.CharmSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.CharmSlot = _equippableForSlot;
                  break;
                case "AccessorySlot": 
                  playerEquipSlots.AccessorySlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.AccessorySlot = _equippableForSlot;
                  break;
                case "FootSlot": 
                  playerEquipSlots.FootSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.FootSlot = _equippableForSlot;
                  break;
                default: break;
              }

              // Debug.Log($"PlayerSlot ({s[slotIndex]}) > {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
            }
            catch (System.Exception e) { Debug.Log($"Error (PlayerStatBase) > {e}"); }
            finally { slotIndex++; }
          }

          foreach (var _slotOption in playerEquipSlots.GetWeaponSlots())
          {
            try
            {
              // string tempRes = _slotOption.ToString().Remove(_slotOption.ToString().IndexOf("Slot"));
              string tempRes = s[slotIndex].ToString().Remove(s[slotIndex].ToString().IndexOf("Slot"));
              EQUIPPABLESLOT _slotAssign;
              Enum.TryParse<EQUIPPABLESLOT>(tempRes, out _slotAssign);
              List<Equippable> _slotEquippables = EL.Where(_e => _e.EquippableSlot == _slotAssign).ToList();
              // var _slotString = EL.Where(_e => _e.EquippableSlot == EQUIPPABLESLOT.Chest).Select(_e => _e.Name);
              var _slotString = _slotEquippables.Select(_e => _e.Name);
              Debug.Log($"Items available for type [{tempRes}] : {string.Join<string>(", ", _slotString)}");
              Equippable _equippableForSlot = _slotEquippables[rand.Next(_slotEquippables.Count)];

              List<Weapon> _slotWeapons = WL.Select(_w => _w).ToList();
              Weapon _weaponForSlot = _slotWeapons[rand.Next(_slotWeapons.Count)];

              Debug.Log($"Equippable chosen for type [{tempRes}] : {_equippableForSlot.Name}");
              
              // Debug.Log($"playerEquipSlots.GetType().GetProperty({s[slotIndex]}).SetValue(playerEquipSlots, {_equippableForSlot}, null)");
              // Debug.Log($"Property (GT) : {playerEquipSlots.GetType()}");
              // Debug.Log($"Property : {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
              // playerEquipSlots.GetType().GetProperty(s[slotIndex]).SetValue(playerEquipSlots.GetType().GetProperty(s[slotIndex]), _equippableForSlot, null);
              // Debug.Log($"Property 2 : {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");

              // playerEquipSlots.GetType().GetProperty(s[slotIndex]).SetValue(playerEquipSlots, _equippableForSlot, null);
              // // playerEquipSlots.ChestSlot = _equippableForSlot;

              switch (s[slotIndex])
              {
                case "HeadSlot": 
                  playerEquipSlots.HeadSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.HeadSlot = _equippableForSlot;
                  break;
                case "ChestSlot": 
                  playerEquipSlots.ChestSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.ChestSlot = _equippableForSlot;
                  break;
                case "LegSlot": 
                  playerEquipSlots.LegSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.LegSlot = _equippableForSlot;
                  break;
                case "CharmSlot": 
                  playerEquipSlots.CharmSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.CharmSlot = _equippableForSlot;
                  break;
                case "AccessorySlot": 
                  playerEquipSlots.AccessorySlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.AccessorySlot = _equippableForSlot;
                  break;
                case "FootSlot": 
                  playerEquipSlots.FootSlot = _equippableForSlot;
                  // _assignPlayerEquipSlots.FootSlot = _equippableForSlot;
                  break;
                case "WeaponSlot1": 
                  playerEquipSlots.WeaponSlot1 = _weaponForSlot;
                  // _assignPlayerEquipSlots.FootSlot = _equippableForSlot;
                  break;
                default: break;
              }

              // Debug.Log($"PlayerSlot ({s[slotIndex]}) > {playerEquipSlots.GetType().GetProperty(s[slotIndex])}");
            }
            catch (System.Exception e) { Debug.Log($"Error (PlayerStatBase) > {e}"); }
            finally { slotIndex++; }
          }

          _assignPlayerEquipSlots = playerEquipSlots;

          // List<Equippable> _slotEquippables = EL.Where(_e => _e.EquippableSlot == EQUIPPABLESLOT.Chest).ToList();
          // var _slotString = _slotEquippables.Select(_e => _e.Name);
          // Debug.Log($"Items available for type [Chest] : {string.Join<string>(", ", _slotString)}");
          // Equippable _equippableForSlot = _slotEquippables[rand.Next(_slotEquippables.Count)];
          // Debug.Log($"Equippable chosen for type [Chest] : {_equippableForSlot.Name}");
          // playerEquipSlots.ChestSlot = _equippableForSlot;
        }
    }

    void OnEquipmentChange()
    {
      foreach (var item in playerEquipSlots.GetSlots())
      {
        // Calculate and add the stats here
      }
    }
}
