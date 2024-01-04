using UnityEngine;

using System.Text;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ToDB
{
  public class UserData
  {
      public string UserId; // GUID
      public string Username;

      public double Experience;

      /** Currency */
      public int Coin;
      public int Diamond;
      public int Crystal;

      public int Backpack_Max_Size; // 40
      public int Vault_Max_Size; // 100

      public string Stringify() 
      {
          return JsonUtility.ToJson(this);
      }

      public static UserData Parse(string json)
      {
          return JsonUtility.FromJson<UserData>(json);
      }

      // void UploadUserData(string uid)
  }

  public class EquippableChestDB
  {
      public string ItemId;// {get; set;}
      public string Name;
      public string Description;
      public EQUIPPABLESLOT EquippableSlot;
      public ItemRarity Rarity;
      public GEARSETS GearSet;
      public Texture2D ItemSprite;
      public List<ItemStats> itemStats;

      public string Stringify() 
      {
          return JsonUtility.ToJson(this);
      }

      public static EquippableChestDB Parse(string json)
      {
          return JsonUtility.FromJson<EquippableChestDB>(json);
      }

      // void UploadEquippableChestDB(string uid)
  }

    // public class Datum
    // {
    //     public string _id { get; set; }
    //     public string itemid { get; set; }
    //     public string name { get; set; }
    //     public string description { get; set; }
    //     public string equippableslot { get; set; }
    //     public string rarity { get; set; }
    //     public string gearset { get; set; }
    //     public object itemsprite { get; set; }
    //     public List<Itemstat> itemstats { get; set; }
    // }

    // public class Itemstat
    // {
    //     public int stat { get; set; }
    //     public int modifier { get; set; }
    //     public int statValue { get; set; }
    // }

    // public class Root
    // {
    //     public List<Datum> data { get; set; }
    // }

}
